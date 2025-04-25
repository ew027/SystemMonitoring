// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/**
 * Systems Monitoring Dashboard JavaScript
 */

// Create a namespace for our dashboard functionality
var Dashboard = (function () {
    // Private variables
    var updateInterval = 5000; // 5 seconds
    var updateTimer;

    // Dashboard initialization
    function init() {
        // Set up event listeners
        setupEventListeners();

        // Start the periodic updates
        startMetricUpdates();
    }

    // Set up all event listeners
    function setupEventListeners() {
        // Handle system header clicks
        $('.system-header').on('click', function () {
            const systemId = $(this).data('system-id');
            toggleSystem(systemId);
        });
    }

    // Toggle system expansion
    function toggleSystem(systemId) {
        const systemContent = $('#' + systemId);
        const systemHeader = systemContent.prev('.system-header');

        // Toggle active class on system content
        systemContent.toggleClass('active');

        // Toggle expanded class on system header for arrow animation
        systemHeader.toggleClass('expanded');

        // Only load content if it's being expanded and doesn't have content yet
        if (systemContent.hasClass('active') && systemContent.find('.panel-header').length === 0) {
            loadSystemDetails(systemId);
        }
    }

    // Load system details via AJAX
    function loadSystemDetails(systemId) {
        $.ajax({
            url: '/Systems/GetSystemDetails',
            data: { id: systemId },
            type: 'GET',
            success: function (result) {
                $('#' + systemId).html(result);

                // Set up tab switching
                setupTabSwitching(systemId);
            },
            error: function (error) {
                $('#' + systemId).html('<div class="error-message">Error loading system details. Please try again.</div>');
                console.error('Error loading system details:', error);
            }
        });
    }

    // Set up tab switching for a system
    function setupTabSwitching(systemId) {
        // Handle tab clicks
        $('#' + systemId + ' .panel-tab').on('click', function () {
            const tabId = $(this).data('tab-id');
            switchTab(systemId, tabId);
        });
    }

    // Switch between tabs within a system
    function switchTab(systemId, tabId) {
        // First update the active tab
        const tabsContainer = $('#' + systemId + ' .panel-tabs');
        tabsContainer.find('.panel-tab').removeClass('active');
        tabsContainer.find('.panel-tab[data-tab-id="' + tabId + '"]').addClass('active');

        // Then update the tab content
        const tabsContent = $('#' + systemId + ' .tab-content');
        tabsContent.find('.tab-pane').removeClass('active');

        // Check if the tab content is already loaded
        const targetTabPane = tabsContent.find('#' + systemId + '-' + tabId);
        if (targetTabPane.length > 0) {
            // Tab content already exists, just activate it
            targetTabPane.addClass('active');
        } else {
            // Load the tab content via AJAX
            loadTabContent(systemId, tabId);
        }
    }

    // Load tab content via AJAX
    function loadTabContent(systemId, tabId) {
        const tabsContent = $('#' + systemId + ' .tab-content');

        // Show loading indicator in the tab content area
        tabsContent.html('<div class="loading-indicator"><i class="fas fa-spinner fa-spin"></i> Loading tab content...</div>');

        $.ajax({
            url: '/Systems/GetSystemTab',
            data: { systemId: systemId, tabId: tabId },
            type: 'GET',
            success: function (result) {
                // Replace with the loaded content
                tabsContent.html(result);

                // Activate the tab
                tabsContent.find('.tab-pane').addClass('active');

                // Initialize any charts or components in the tab
                initCharts();
            },
            error: function (error) {
                tabsContent.html('<div class="error-message">Error loading tab content. Please try again.</div>');
                console.error('Error loading tab content:', error);
            }
        });
    }

    // Start periodic metric updates
    function startMetricUpdates() {
        // Update immediately
        updateMetrics();

        // Then schedule updates every interval
        updateTimer = setInterval(updateMetrics, updateInterval);
    }

    // Stop periodic updates
    function stopMetricUpdates() {
        if (updateTimer) {
            clearInterval(updateTimer);
        }
    }

    // Update metrics with random values (would normally fetch from API)
    function updateMetrics() {
        // Update metrics for all systems
        $('.system-health .health-metric span').each(function () {
            const metricType = $(this).prev('.health-icon').attr('class').split(' ')[1];
            let min, max, unit;

            switch (metricType) {
                case 'cpu':
                    min = 15;
                    max = 95;
                    unit = '%';
                    break;
                case 'memory':
                    min = 30;
                    max = 90;
                    unit = '%';
                    break;
                case 'disk':
                    min = 60;
                    max = 95;
                    unit = '%';
                    break;
                case 'response':
                    min = 40;
                    max = 350;
                    unit = 'ms';
                    break;
                case 'requests':
                    min = 200;
                    max = 500;
                    unit = '/s';
                    break;
                case 'users':
                    min = 80;
                    max = 200;
                    unit = '';
                    break;
                case 'hit-rate':
                    min = 80;
                    max = 99;
                    unit = '%';
                    break;
                case 'traffic':
                    min = 100;
                    max = 500;
                    unit = 'Mbps';
                    break;
                default:
                    min = 10;
                    max = 90;
                    unit = '';
            }

            // Generate a random value
            const value = Math.floor(Math.random() * (max - min + 1)) + min;
            $(this).text(value + unit);
        });

        // Update system status based on metrics
        updateSystemStatus();
    }

    // Update system status based on current metrics
    function updateSystemStatus() {
        // For each system, check its metrics and determine overall status
        $('.system-bar').each(function () {
            const header = $(this).find('.system-header');
            let status = 'healthy';

            // Check all metrics in this system
            $(this).find('.health-metric span').each(function () {
                const text = $(this).text();

                // Extract numeric value
                const value = parseInt(text);

                // Check metric type based on unit
                if (text.includes('%')) {
                    // Percentage metrics
                    if (value > 90) status = 'critical';
                    else if (value > 75 && status === 'healthy') status = 'warning';
                }
                else if (text.includes('ms')) {
                    // Response time
                    if (value > 200) status = 'critical';
                    else if (value > 100 && status === 'healthy') status = 'warning';
                }
            });

            // Update header class
            header.removeClass('status-healthy status-warning status-critical');
            header.addClass('status-' + status);
        });

        // Update status summary
        updateStatusSummary();
    }

    // Update status summary counts
    function updateStatusSummary() {
        const healthyCount = $('.system-header.status-healthy').length;
        const warningCount = $('.system-header.status-warning').length;
        const criticalCount = $('.system-header.status-critical').length;

        // Update the status indicators
        $('.status-indicator:nth-child(1) span').text(healthyCount + ' Healthy');
        $('.status-indicator:nth-child(2) span').text(warningCount + ' Warning');
        $('.status-indicator:nth-child(3) span').text(criticalCount + ' Critical');
    }

    // Initialize mock charts (this would normally use a charting library)
    function initCharts() {
        $('.chart-placeholder').each(function () {
            // Generate ASCII charts for demonstration
            const chartType = $(this).text().toLowerCase();
            let mockChart = '';

            if (chartType.includes('trend') || chartType.includes('time')) {
                // Line chart ASCII art
                mockChart = `
    ╭─────────────────────────╮
    │       ╱╲        ╱╲      │
    │      ╱  ╲      ╱  ╲     │
    │     ╱    ╲    ╱    ╲    │
    │    ╱      ╲  ╱      ╲   │
    │___╱        ╲╱        ╲__│
    ╰─────────────────────────╯
`;
            } else if (chartType.includes('usage') || chartType.includes('space')) {
                // Bar chart ASCII art
                mockChart = `
    ╭─────────────────────────╮
    │ ▓▓       ▓▓▓▓    ▓▓▓▓▓ │
    │ ▓▓       ▓▓▓▓    ▓▓▓▓▓ │
    │ ▓▓▓▓     ▓▓▓▓    ▓▓▓▓▓ │
    │ ▓▓▓▓     ▓▓▓▓▓   ▓▓▓▓▓ │
    │ ▓▓▓▓     ▓▓▓▓▓   ▓▓▓▓▓ │
    ╰─────────────────────────╯
`;
            } else {
                // Generic chart ASCII art
                mockChart = `
    ╭─────────────────────────╮
    │        ▓▓▓              │
    │    ▓▓▓ ▓▓▓              │
    │    ▓▓▓ ▓▓▓  ▓▓▓         │
    │    ▓▓▓ ▓▓▓  ▓▓▓  ▓▓▓    │
    │    ▓▓▓ ▓▓▓  ▓▓▓  ▓▓▓    │
    ╰─────────────────────────╯
`;
            }

            $(this).css({
                'font-family': 'monospace',
                'white-space': 'pre',
                'font-size': '12px',
                'line-height': '1.2',
                'display': 'flex',
                'justify-content': 'center',
                'align-items': 'center'
            }).html(mockChart);
        });
    }

    // Public API
    return {
        init: init,
        toggleSystem: toggleSystem,
        updateMetrics: updateMetrics
    };
})();