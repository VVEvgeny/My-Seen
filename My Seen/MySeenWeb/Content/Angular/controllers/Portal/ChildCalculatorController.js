App.config(function($stateProvider) {

    $stateProvider
        .state("portal/childCalculator",
        {
            url: "/portal/ChildSexCalculator/",
            templateUrl: "Content/Angular/templates/Portal/childcalculator.html",
            controller: "ChildCalculatorController",
            reloadOnSearch: false
        });
});

var TestWebGLRun = false;

App.controller("ChildCalculatorController",
[
    "$scope", "$rootScope", "$state", "$stateParams", "$http", "$location", "Constants", "$log", "$anchorScroll",
    function($scope, $rootScope, $state, $stateParams, $http, $location, constants, $log, $anchorScroll) {

        $anchorScroll();
        $rootScope.pageId = constants.PageIds.ChildSexCalculator;

        //Основные данные
        function fillScope(page) {
            $scope.data = page.Data;

            var girl = $scope.translation.Girl;
            var boy = $scope.translation.Boy;

            if ($scope.data.length > 0) {
                for (var i = 0; i < $scope.data.length; i++) {
                    var dataProvider = [];
                    for (var j = 0; j < $scope.data[i].Data.length; j++) {
                        dataProvider.push({
                            "Day": $scope.data[i].Data[j].Day,
                            "Boy": $scope.data[i].Data[j].Boy,
                            "Girl": $scope.data[i].Data[j].Girl
                        });
                    }

                    AmCharts.makeChart("chartdiv-" + (i + 1),
                    {
                        "type": "serial",
                        "theme": "light",
                        "legend": {
                            "horizontalGap": 100,
                            "maxColumns": 1,
                            "position": "right",
                            "useGraphSettings": true,
                            "markerSize": 10
                        },
                        "dataProvider": dataProvider,
                        "valueAxes": [{ "stackType": "regular", "axisAlpha": 0.3, "gridAlpha": 0 }],
                        "graphs": [
                            {
                                "balloonText":
                                    "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>",
                                "fillAlphas": 0.8,
                                "labelText": "[[value]]",
                                "lineAlpha": 0.3,
                                "title": boy,
                                "type": "column",
                                "color": "#000000",
                                "valueField": "Boy"
                            },
                            {
                                "balloonText":
                                    "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>",
                                "fillAlphas": 0.8,
                                "labelText": "[[value]]",
                                "lineAlpha": 0.3,
                                "title": girl,
                                "type": "column",
                                "color": "#000000",
                                "valueField": "Girl"
                            }
                        ],
                        "categoryField": "Day",
                        "categoryAxis": {
                            "gridPosition": "start",
                            "axisAlpha": 0,
                            "gridAlpha": 0,
                            "position": "left"
                        },
                        "export": {
                            "enabled": true
                        }
                    });
                }
            }
        };

        //Перевод таблицы и модальной
        function fillTranslation(page) {
            $scope.translation = page;
            $scope.translation.loaded = true;
        }

        //Перевод всех данных на тек. странице
        $scope.translation = {};
        $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });


        $scope.requestButtonClick = function() {

            $rootScope.GetPage(constants.Pages.Main,
                $http,
                fillScope,
                {
                    pageId: $rootScope.pageId,
                    year: $scope.dateYear,
                    dateMan: $scope.dateMan,
                    dateWoman: $scope.dateWoman
                });
        };

    }
]);