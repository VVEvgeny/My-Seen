App.config(function ($stateProvider) {

    $stateProvider
        .state('portal/realt', {
            url: '/portal/realt/?:year&price&deals&salary',
            templateUrl: "Content/Angular/templates/portal/realt.html",
            controller: 'RealtController',
            reloadOnSearch: false
        });
});

var TestWebGLRun = false;

App.controller('RealtController', [
    '$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants', '$log',
    function($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

        $rootScope.pageId = constants.PageIds.Realt;

        //Перевод всех данных на тек. странице
        $scope.translation = {};

        //Перевод таблицы и модальной
        function fillTranslation(page) {
            $scope.translation = page;
            $scope.translation.loaded = true;

            AmCharts.shortMonthNames = [
                $scope.translation.January,
                $scope.translation.February,
                $scope.translation.March,
                $scope.translation.April,
                $scope.translation.May,
                $scope.translation.June,
                $scope.translation.July,
                $scope.translation.August,
                $scope.translation.September,
                $scope.translation.October,
                $scope.translation.November,
                $scope.translation.December
            ];
        }

        $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });

        function getMainPage() {

            $stateParams.year = ($scope.year !== 0 && $scope.year !== '') ? $scope.year : null;
            $stateParams.price = ($scope.price !== '') ? $scope.price : null;
            $stateParams.deals = ($scope.deals !== '') ? $scope.deals : null;
            $stateParams.salary = ($scope.salary !== '') ? $scope.salary : null;

            $rootScope.GetPage(constants.Pages.Main, $http, fillScope, {
                pageId: $rootScope.pageId,
                year: ($stateParams && $stateParams.year !== '' && $stateParams.year !== 0 ? $stateParams.year : null),
                price: ($stateParams && $stateParams.price !== '' ? $stateParams.price : null),
                deals: ($stateParams && $stateParams.deals !== '' ? $stateParams.deals : null),
                salary: ($stateParams && $stateParams.salary !== '' ? $stateParams.salary : null)
            });
        };

        if ($stateParams) {
            if ($stateParams.year) $scope.year = parseInt($stateParams.year);
            else $scope.year = '';
            if ($stateParams.price) $scope.price = parseInt($stateParams.price);
            else $scope.price = '';
            if ($stateParams.deals) $scope.deals = parseInt($stateParams.deals);
            else $scope.deals = '';
            if ($stateParams.salary) $scope.salary = parseInt($stateParams.salary);
            else $scope.salary = '';
        }

        $scope.isParams = false;
        $scope.showParams = function() {
            if ($("#params").hasClass("out")) {
                $("#params").addClass("in");
                $("#params").removeClass("out");
                $scope.isParams = true;
            } else {
                $("#params").addClass("out");
                $("#params").removeClass("in");
                $scope.isParams = false;
            }
        };

        $scope.isSettings = false;
        $scope.showSettings = function () {
            if ($("#settings").hasClass("out")) {
                $("#settings").addClass("in");
                $("#settings").removeClass("out");
                $scope.isSettings = true;
            } else {
                $("#settings").addClass("out");
                $("#settings").removeClass("in");
                $scope.isSettings = false;
            }
        };

        if ($scope.year !== '' && $scope.year !== 0
            || ($scope.year !== '' && $scope.year !== 0 && ($scope.price !== '' || $scope.deals !== '' || $scope.salary !== ''))
            ) {
            $scope.showParams();
        }

        getMainPage();


        $scope.reset = function() {
            $location.search('year', null);
            $location.search('price', null);
            $location.search('deals', null);
            $location.search('salary', null);
            if ($stateParams) {
                $stateParams.year = null;
                $stateParams.price = null;
                $stateParams.deals = null;
                $stateParams.salary = null;
            }
            $scope.year = '';
            $scope.price = '';
            $scope.deals = '';
            $scope.salary = '';

            getMainPage();
            $scope.showParams();
        };
        $scope.calculate = function () {
            $location.search('year', ($scope.year !== 0 && $scope.year !== '') ? $scope.year : null);
            $location.search('price', ($scope.price !== '') ? $scope.price : null);
            $location.search('deals', ($scope.deals !== '') ? $scope.deals : null);
            $location.search('salary', ($scope.salary !== '') ? $scope.salary : null);

            if ($stateParams) {
                $stateParams.year = ($scope.year !== 0 && $scope.year !== '') ? $scope.year : null;
                $stateParams.price = ($scope.price !== '') ? $scope.price : null;
                $stateParams.deals = ($scope.deals !== '') ? $scope.deals : null;
                $stateParams.salary = ($scope.salary !== '') ? $scope.salary : null;
            }
            getMainPage();
        };


        //Основные данные
        function fillScope(page) {

            $scope.data = page.Data;
            $scope.dataSalary = page.DataSalary;

            $scope.LastUpdatedPrice = page.LastUpdatedPrice;
            $scope.LastUpdatedSalary = page.LastUpdatedSalary;

            createChart();
        };

        $scope.viewModeChange = function () {
            createChart();
        };

        $scope.showSalary = true;
        $scope.showDeals = true;

        function createChart() {
            
            var chartData = [];
            if ($scope.data.length > 0) {
                for (var i = 0; i < $scope.data.length; i++) {
                    chartData.push(
                    {
                        "date": new Date($scope.data[i].DateText.split('/')[2], parseInt($scope.data[i].DateText.split('/')[1] - 1), $scope.data[i].DateText.split('/')[0]),
                        "price": $scope.data[i].Price,
                        "count": $scope.data[i].Count,
                        "salary": $scope.dataSalary[i].Amount
                    });
                }

                var chart = new AmCharts.AmSerialChart();
                chart.pathToImages = "Content/amcharts/images/";
                chart.dataProvider = chartData;
                chart.autoMargins = false;
                chart.marginLeft = 10;
                chart.marginRight = 10;
                chart.marginBottom = 25;
                chart.marginTop = 0;
                chart.fontSize = 10;

                chart.columnWidth = 6; // relative
                chart.columnSpacing = 2; // px

                chart.categoryField = "date";
                chart.zoomOutButton = {
                    backgroundColor: '#000000',
                    backgroundAlpha: 0.15
                };

                // listen for "dataUpdated" event (fired when chart is inited) and call zoomChart method when it happens
                //chart.addListener("dataUpdated", zoomChart);
                chart.zoomOutText = $scope.translation.ShowAll || 'Show all';

                // AXES
                // category
                var categoryAxis = chart.categoryAxis;
                categoryAxis.parseDates = true; // as our data is date-based, we set parseDates to true
                categoryAxis.minPeriod = "DD"; // our data is yearly, so we set minPeriod to YYYY / MM
                categoryAxis.gridAlpha = 0.15;
                categoryAxis.axisColor = "#DADADA";

                // value
                var priceAxis = new AmCharts.ValueAxis();
                priceAxis.axisAlpha = 0.5; // no axis line
                priceAxis.position = "left";
                priceAxis.inside = true;
                priceAxis.unit = " " + ($scope.translation.USDM2 || 'USD/m2');
                chart.addValueAxis(priceAxis);

                var salaryAxis = new AmCharts.ValueAxis();
                salaryAxis.axisAlpha = 0.5; // no axis line
                salaryAxis.position = "left";
                salaryAxis.inside = true;
                salaryAxis.unit = " " + ($scope.translation.USD || 'USD');
                if ($scope.showSalary) chart.addValueAxis(salaryAxis);

                var countAxis = new AmCharts.ValueAxis();
                countAxis.axisAlpha = 0; // no axis line
                countAxis.gridAlpha = 0; // no grid
                countAxis.position = "right";
                if ($scope.showDeals) chart.addValueAxis(countAxis);

                /*
                 * price graph
                 */
                var priceGraph = new AmCharts.AmGraph();
                priceGraph.type = "smoothedLine"; // this line makes the graph smoothed line.
                priceGraph.valueAxis = priceAxis; // indicate which axis should be used
                priceGraph.lineColor = "#ff0000";
                priceGraph.negativeLineColor = "#637BB6"; // this line makes the graph to change color when it drops below 0
                priceGraph.bullet = "round";
                priceGraph.bulletBorderColor = "#FFFFFF";
                priceGraph.bulletBorderThickness = 2;
                priceGraph.lineThickness = 1;
                priceGraph.valueField = "price";
                priceGraph.balloonText = "[[value]] " + ($scope.translation.USDM2 || 'USD/m2');
                priceGraph.hideBulletsCount = 55; // this makes the chart to hide bullets when there are more than 55 series in selection
                chart.addGraph(priceGraph);

                var salaryGraph = new AmCharts.AmGraph();
                salaryGraph.type = "smoothedLine"; // this line makes the graph smoothed line.
                if ($scope.showSalary) {
                    if ($scope.priceToSalary) {
                        salaryGraph.valueAxis = salaryAxis; // indicate which axis should be used
                    } else {
                        salaryGraph.valueAxis = priceAxis; // indicate which axis should be used
                    }
                }
                salaryGraph.lineColor = "#00ff00";
                salaryGraph.negativeLineColor = "#637BB6"; // this line makes the graph to change color when it drops below 0
                salaryGraph.bullet = "round";
                salaryGraph.bulletBorderColor = "#FFFFFF";
                salaryGraph.bulletBorderThickness = 2;
                salaryGraph.lineThickness = 1;
                salaryGraph.valueField = "salary";
                salaryGraph.balloonText = "[[value]] "+ ($scope.translation.USD || 'USD');
                salaryGraph.hideBulletsCount = 55; // this makes the chart to hide bullets when there are more than 55 series in selection
                if ($scope.showSalary) chart.addGraph(salaryGraph);

                /*
                 * count graph
                 */
                var countGraph = new AmCharts.AmGraph();
                countGraph.type = "column";
                countGraph.valueAxis = countAxis; // indicate which axis should be used
                countGraph.lineColor = "#000000";
                countGraph.valueField = "count";
                countGraph.balloonText = "[[value]] " + ($scope.translation.Deals || 'Deals');
                countGraph.title = ($scope.translation.Deals || 'Deals');
                countGraph.fillAlphas = 0.1;
                countGraph.lineAlpha = 0;
                if ($scope.showDeals) chart.addGraph(countGraph);

                // CURSOR
                var chartCursor = new AmCharts.ChartCursor();
                chartCursor.cursorAlpha = 0.30;
                chartCursor.cursorPosition = "mouse";
                chartCursor.categoryBalloonDateFormat = "DD MMM YYYY";
                chart.addChartCursor(chartCursor);

                // SCROLLBAR
                var chartScrollbar = new AmCharts.ChartScrollbar();
                chartScrollbar.graph = priceGraph;
                chartScrollbar.scrollbarHeight = 60;
                // chartScrollbar.color = "#FFFFFF";
                // chartScrollbar.backgroundColor = "#DDDDDD";
                // chartScrollbar.selectedBackgroundColor = "#FFFFFF";
                chartScrollbar.autoGridCount = true;
                chart.addChartScrollbar(chartScrollbar);

                /*
                * legend
                */
                var legend = new AmCharts.AmLegend();
                legend.position = "top";
                legend.align = "center";

                legend.data = [];
                legend.data.push({ title: $scope.translation.Price || 'Price', color: "#ff0000" });
                if ($scope.showSalary) legend.data.push({ title: $scope.translation.Salary || 'Salary', color: "#00ff00" });
                if ($scope.showDeals) legend.data.push({ title: $scope.translation.Deals || 'Deals', color: "#666666" });
                
                chart.addLegend(legend);
                // WRITE
                chart.write("chartdiv");
            };
        };
    }
]);
