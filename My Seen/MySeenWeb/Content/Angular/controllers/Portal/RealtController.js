App.config(function($stateProvider) {

    $stateProvider
        .state("portal/realt",
        {
            url: "/portal/realt/?:year&price&deals&salary",
            templateUrl: "Content/Angular/templates/portal/realt.html",
            controller: "RealtController",
            reloadOnSearch: false
        });
});

App.controller("RealtController",
[
    "$scope", "$rootScope", "$state", "$stateParams", "$http", "$location", "Constants", "$log", "$anchorScroll",
    function($scope, $rootScope, $state, $stateParams, $http, $location, constants, $log, $anchorScroll) {

        $anchorScroll();
        $rootScope.pageId = constants.PageIds.Realt;

        //Показать ли кнопку ДОБАВИТЬ
        $scope.pageCanAdd = $rootScope.isAdmin;

        //Перевод всех данных на тек. странице
        $scope.translation = {};

        $scope.currentTab = 1;

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

            $stateParams.year = ($scope.year !== 0 && $scope.year !== "") ? $scope.year : null;
            $stateParams.price = ($scope.price !== "") ? $scope.price : null;
            $stateParams.proposals = ($scope.proposals !== "") ? $scope.proposals : null;
            $stateParams.deals = ($scope.deals !== "") ? $scope.deals : null;
            $stateParams.salary = ($scope.salary !== "") ? $scope.salary : null;

            $rootScope.GetPage(constants.Pages.Main,
                $http,
                fillScope,
                {
                    pageId: $rootScope.pageId,
                    year: ($stateParams && $stateParams.year !== "" && $stateParams.year !== 0
                        ? $stateParams.year
                        : null),
                    price: ($stateParams && $stateParams.price !== "" ? $stateParams.price : null),
                    proposals: ($stateParams && $stateParams.proposals !== "" ? $stateParams.proposals : null),
                    deals: ($stateParams && $stateParams.deals !== "" ? $stateParams.deals : null),
                    salary: ($stateParams && $stateParams.salary !== "" ? $stateParams.salary : null)
                });
        };

        if ($stateParams) {
            if ($stateParams.year) $scope.year = parseInt($stateParams.year);
            else $scope.year = "";
            if ($stateParams.price) $scope.price = parseInt($stateParams.price);
            else $scope.price = "";
            if ($stateParams.proposals) $scope.proposals = parseInt($stateParams.proposals);
            else $scope.proposals = "";
            if ($stateParams.deals) $scope.deals = parseInt($stateParams.deals);
            else $scope.deals = "";
            if ($stateParams.salary) $scope.salary = parseInt($stateParams.salary);
            else $scope.salary = "";
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
        $scope.showSettings = function() {
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

        if ($scope.year !== "" && $scope.year !== 0 ||
            ($scope.year !== "" &&
                $scope.year !== 0 &&
                ($scope.price !== "" || $scope.deals !== "" || $scope.salary !== ""))
        ) {
            $scope.showParams();
        }

        getMainPage();


        $scope.reset = function() {
            $location.search("year", null);
            $location.search("price", null);
            $location.search("deals", null);
            $location.search("proposals", null);
            $location.search("salary", null);
            if ($stateParams) {
                $stateParams.year = null;
                $stateParams.price = null;
                $stateParams.deals = null;
                $stateParams.proposals = null;
                $stateParams.salary = null;
            }
            $scope.year = "";
            $scope.price = "";
            $scope.deals = "";
            $scope.proposals = "";
            $scope.salary = "";

            getMainPage();
            $scope.showParams();
        };
        $scope.calculate = function() {
            $location.search("year", ($scope.year !== 0 && $scope.year !== "") ? $scope.year : null);
            $location.search("price", ($scope.price !== "") ? $scope.price : null);
            $location.search("proposals", ($scope.proposals !== "") ? $scope.proposals : null);
            $location.search("deals", ($scope.deals !== "") ? $scope.deals : null);
            $location.search("salary", ($scope.salary !== "") ? $scope.salary : null);

            if ($stateParams) {
                $stateParams.year = ($scope.year !== 0 && $scope.year !== "") ? $scope.year : null;
                $stateParams.price = ($scope.price !== "") ? $scope.price : null;
                $stateParams.proposals = ($scope.proposals !== "") ? $scope.proposals : null;
                $stateParams.deals = ($scope.deals !== "") ? $scope.deals : null;
                $stateParams.salary = ($scope.salary !== "") ? $scope.salary : null;
            }
            getMainPage();
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           МОДАЛЬНАЯ ДОБАВЛЕНИЯ / РЕДАКТИРОВАНИЯ
        ///////////////////////////////////////////////////////////////////////
        //Модальная добавления/редактирования Указываем какие поля будем видеть
        $scope.modal = {
            showWhen: true,
            showSalary: true,
            showProposals: true,
            showDeals: true,
            showPrice: true
        };
        //Прячу модальную Добавить/Редактировать
        //Готовлю данные для добавления новой записи и отображаю модальную
        $scope.addModalOpen = function () {
            $scope.modal.title = $scope.translation.TitleAdd;

            $scope.modal.addButton = true;

            $("#SalaryModalWindow").modal("show");
        };
        $scope.$on("$destroy",
            function () {
                $scope.addModalHide();
                $("body").removeClass("modal-open");
                $(".modal-backdrop").remove();
            });
        $scope.addModalHide = function () {
            $("#SalaryModalWindow").modal("hide");
        };

        //в случае успеха закроем модальное и перезапросим данные, с первой страницы
        function afterAdd() {
            $scope.addModalHide();
            getMainPage();
        };

        //Готовлю данные для отправки и вызову глобальную AddData
        $scope.modal.addButtonClick = function () {
            $rootScope.GetPage(constants.Pages.Add,
                $http,
                afterAdd,
                {
                    pageId: $rootScope.pageId,
                    name: $scope.currentTab === 1 ? 'salary' : $scope.currentTab === 2 ? 'price' : 'deals',
                    datetime: $scope.currentTab === 1 ? $scope.modal.monthYear : $scope.currentTab === 2 ? $scope.modal.date : $scope.modal.monthYearDeals,
                    other: $scope.currentTab === 1 ? $scope.modal.salary : $scope.currentTab === 2 ? ($scope.modal.proposals + '-' + $scope.modal.price) : $scope.modal.deals
                });
        };
        ////////

        //Основные данные
        function fillScope(page) {

            $scope.data = page.Data;
            $scope.dataSalary = page.DataSalary;
            $scope.dataDeals = page.DataDeals;

            $scope.LastUpdatedPrice = page.LastUpdatedPrice;
            $scope.LastUpdatedSalary = page.LastUpdatedSalary;
            $scope.LastUpdatedDeals = page.LastUpdatedDeals;

            createChart();
        };

        $scope.viewModeChange = function() {
            createChart();
        };

        $scope.showSalary = false;
        $scope.showDeals = false;
        $scope.showProposals = false;

        function createChart() {

            var chartData = [];
            if ($scope.data.length > 0) {
                for (var i = 0; i < $scope.data.length; i++) {
                    chartData.push(
                    {
                        "date": new Date($scope.data[i].DateText.split("/")[2],
                            parseInt($scope.data[i].DateText.split("/")[1] - 1),
                            $scope.data[i].DateText.split("/")[0]),
                        "price": $scope.data[i].Price,
                        "count": $scope.data[i].Count,
                        "salary": $scope.dataSalary[i].Amount,
                        "deals": $scope.dataDeals[i].Amount
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
                    backgroundColor: "#000000",
                    backgroundAlpha: 0.15
                };

                // listen for "dataUpdated" event (fired when chart is inited) and call zoomChart method when it happens
                //chart.addListener("dataUpdated", zoomChart);
                chart.zoomOutText = $scope.translation.ShowAll || "Show all";

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
                priceAxis.unit = " " + ($scope.translation.USDM2 || "USD/m2");
                chart.addValueAxis(priceAxis);

                var salaryAxis = new AmCharts.ValueAxis();
                salaryAxis.axisAlpha = 0.5; // no axis line
                salaryAxis.position = "left";
                salaryAxis.inside = true;
                salaryAxis.unit = " " + ($scope.translation.USD || "USD");
                if ($scope.showSalary) chart.addValueAxis(salaryAxis);

                var countAxis = new AmCharts.ValueAxis();
                countAxis.axisAlpha = 0; // no axis line
                countAxis.gridAlpha = 0; // no grid
                countAxis.position = "right";
                if ($scope.showProposals) chart.addValueAxis(countAxis);

                var dealsAxis = new AmCharts.ValueAxis();
                dealsAxis.axisAlpha = 0.5; // no axis line
                dealsAxis.position = "left";
                dealsAxis.inside = true;
                dealsAxis.unit = "";
                if ($scope.showDeals) chart.addValueAxis(dealsAxis);
                
                /*
                 * price graph
                 */
                var priceGraph = new AmCharts.AmGraph();
                priceGraph.type = "smoothedLine"; // this line makes the graph smoothed line.
                priceGraph.valueAxis = priceAxis; // indicate which axis should be used
                priceGraph.lineColor = "#ff0000";
                priceGraph
                    .negativeLineColor = "#637BB6"; // this line makes the graph to change color when it drops below 0
                priceGraph.bullet = "round";
                priceGraph.bulletBorderColor = "#FFFFFF";
                priceGraph.bulletBorderThickness = 2;
                priceGraph.lineThickness = 1;
                priceGraph.valueField = "price";
                priceGraph.balloonText = "[[value]] " + ($scope.translation.USDM2 || "USD/m2");
                priceGraph.hideBulletsCount = 55;
// this makes the chart to hide bullets when there are more than 55 series in selection
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
                salaryGraph
                    .negativeLineColor = "#637BB6"; // this line makes the graph to change color when it drops below 0
                salaryGraph.bullet = "round";
                salaryGraph.bulletBorderColor = "#FFFFFF";
                salaryGraph.bulletBorderThickness = 2;
                salaryGraph.lineThickness = 1;
                salaryGraph.valueField = "salary";
                salaryGraph.balloonText = "[[value]] " + ($scope.translation.USD || "USD");
                salaryGraph.hideBulletsCount = 55;
// this makes the chart to hide bullets when there are more than 55 series in selection
                if ($scope.showSalary) chart.addGraph(salaryGraph);

                /*
                 * count graph
                 */
                var countGraph = new AmCharts.AmGraph();
                countGraph.type = "column";
                countGraph.valueAxis = countAxis; // indicate which axis should be used
                countGraph.lineColor = "#000000";
                countGraph.valueField = "count";
                countGraph.balloonText = "[[value]] " + ($scope.translation.Proposals || "Proposals");
                countGraph.title = ($scope.translation.Proposals || "Proposals");
                countGraph.fillAlphas = 0.1;
                countGraph.lineAlpha = 0;
                if ($scope.showProposals) chart.addGraph(countGraph);
                /*
                * deals graph
                */
                var dealsGraph = new AmCharts.AmGraph();
                dealsGraph.type = "smoothedLine"; // this line makes the graph smoothed line.

                if ($scope.showSalary) {
                    /*
                    if ($scope.priceToSalary) {
                        salaryGraph.valueAxis = salaryAxis; // indicate which axis should be used
                    } else {
                        salaryGraph.valueAxis = priceAxis; // indicate which axis should be used
                    }
                    */
                }
                dealsGraph.lineColor = "#0000ff";
                dealsGraph.negativeLineColor = "#637BB6"; // this line makes the graph to change color when it drops below 0
                dealsGraph.bullet = "round";
                dealsGraph.bulletBorderColor = "#FFFFFF";
                dealsGraph.bulletBorderThickness = 2;
                dealsGraph.lineThickness = 1;
                dealsGraph.valueField = "deals";
                dealsGraph.balloonText = "[[value]] " + ($scope.translation.Deals || "Deals");;
                dealsGraph.hideBulletsCount = 55;
                // this makes the chart to hide bullets when there are more than 55 series in selection
                if ($scope.showDeals) chart.addGraph(dealsGraph);

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
                legend.data.push({ title: $scope.translation.Price || "Price", color: "#ff0000" });
                if ($scope.showSalary)
                    legend.data.push({ title: $scope.translation.Salary || "Salary", color: "#00ff00" });
                if ($scope.showDeals)
                    legend.data.push({ title: $scope.translation.Deals || "Deals", color: "#0000ff" });
                if ($scope.showProposals)
                    legend.data.push({ title: $scope.translation.Proposals || "Proposals", color: "#666666" });

                chart.addLegend(legend);
                // WRITE
                chart.write("chartdiv");
            };
        };
    }
]);