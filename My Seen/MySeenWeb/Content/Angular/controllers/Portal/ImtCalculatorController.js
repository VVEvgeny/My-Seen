App.config(function ($stateProvider) {

    $stateProvider
        .state('portal/imtCalculator', {
            url: '/portal/ImtCalculator/',
            templateUrl: "Content/Angular/templates/Portal/imtcalculator.html",
            controller: 'IMTCalculatorController',
            reloadOnSearch: false
        });
});

var TestWebGLRun = false;

App.controller('IMTCalculatorController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants', '$log',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants, $log) {
      
      $rootScope.pageId = constants.PageIds.IMTCalculator;

      //Перевод таблицы и модальной
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      //Перевод всех данных на тек. странице
      $scope.translation = {};
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });

      function textBmi(value)
      {
          if (value <= 16.5) return $scope.translation.DeadlineUnderweight;
          if (value <= 18.4) return $scope.translation.LackOfWeight;
          if (value <= 24.9) return $scope.translation.NormalWeight;
          if (value <= 30) return $scope.translation.Overweight;
          if (value <= 34.9) return $scope.translation.ObesityClassI;
          if (value <= 40) return $scope.translation.ObesityClassII;
          return $scope.translation.ObesityClassIII;
      };

      $scope.calculate = function () {

          $scope.bmi = parseInt($scope.weight / (($scope.growth / 100) * ($scope.growth / 100)));
          $scope.level = textBmi($scope.bmi);

          var start;
          var end;
          var color;
          var bmi;
          var dataProvider = [];

          start = 0;
          end = parseInt(16.5 * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#FF0F00";
          bmi = $scope.translation.BMI + " <= 16.5";
          dataProvider.push({ "name": $scope.translation.DeadlineUnderweight, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = parseInt(18.4 * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#F8FF01";
          bmi = $scope.translation.BMI + " > 16.5 & <= 18.4";
          dataProvider.push({ "name": $scope.translation.LackOfWeight, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = parseInt(24.9 * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#04D215";
          bmi = $scope.translation.BMI + " > 18.4 & <= 24.9";
          dataProvider.push({ "name": $scope.translation.NormalWeight, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = parseInt(30 * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#F8FF01";
          bmi = $scope.translation.BMI + " > 24.9 & <= 30";
          dataProvider.push({ "name": $scope.translation.Overweight, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = parseInt(34.9 * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#FF9E01";
          bmi = $scope.translation.BMI + " > 30 & <= 34.9";
          dataProvider.push({ "name": $scope.translation.ObesityClassI, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = parseInt(40 * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#FF0F00";
          bmi = $scope.translation.BMI + " > 34.9 & <= 40";
          dataProvider.push({ "name": $scope.translation.ObesityClassII, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = 180;
          color = "#FF0F00";
          bmi = $scope.translation.BMI + " > 40";
          dataProvider.push({ "name": $scope.translation.ObesityClassIII, "start": start, "end": end, "color": color, "bmi": bmi });

          AmCharts.makeChart("chartdiv", {
              "theme": "light",
              "type": "serial",
              "dataProvider": dataProvider,
              "valueAxes": [{
                  "axisAlpha": 0,
                  "gridAlpha": 0.1
              }],
              "startDuration": 1,
              "graphs": [{
                  "balloonText": "<b>[[category]]</b><br>[[start]] - [[end]]<br>[[bmi]]",
                  "colorField": "color",
                  "fillAlphas": 0.8,
                  "lineAlpha": 0,
                  "openField": "start",
                  "type": "column",
                  "valueField": "end"
              }],
              "rotate": true,
              "columnWidth": 1,
              "categoryField": "name",
              "categoryAxis": {
                  "gridPosition": "start",
                  "axisAlpha": 0,
                  "gridAlpha": 0.1,
                  "position": "left"
              },
              "export": {
                  "enabled": true
              }
          });

          $scope.result = true;
      };

  }]);
