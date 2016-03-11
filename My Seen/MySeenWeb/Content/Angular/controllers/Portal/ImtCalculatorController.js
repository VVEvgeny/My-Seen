App.config(function ($stateProvider) {

    $stateProvider
        .state('portal/imtCalculator', {
            url: '/portal/BmiCalculator/?a&s&g&w',
            templateUrl: "Content/Angular/templates/Portal/imtcalculator.html",
            controller: 'BmiCalculatorController',
            reloadOnSearch: false
        });
});

App.controller('BmiCalculatorController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants', '$log',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants, $log) {
      
      $rootScope.pageId = constants.PageIds.IMTCalculator;

      $scope.translation = {};
      var calculateAfterLoad = false;
      var sexs = { Man: 1, Woman: 2 };
      var ages = { to24: 1, to34: 2, to44: 3, to54: 4, to64: 5, more64: 6 };
      $scope.sexs = [{ id: sexs.Man, name: $scope.translation.Man || 'Man' }, { id: sexs.Woman, name: $scope.translation.Woman || 'Woman' }];

      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;

          if (calculateAfterLoad) $scope.calculate();
          $scope.sexs = [{ id: sexs.Man, name: $scope.translation.Man || 'Man' }, { id: sexs.Woman, name: $scope.translation.Woman || 'Woman' }];
      }
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });

      $scope.ages = [
          { id: ages.to24, name: '19-24' },
          { id: ages.to34, name: '25-34' },
          { id: ages.to44, name: '35-44' },
          { id: ages.to54, name: '45-54' },
          { id: ages.to64, name: '55-64' },
          { id: ages.more64, name: '65+' }
      ];
      var bmis = {
          DeadlineUnderweight: 1,
          LackOfWeight: 2,
          NormalWeight: 3,
          Overweight: 4,
          ObesityClassI: 5,
          ObesityClassII: 6,
          ObesityClassIII: 7
      };

      if ($stateParams) {

          if ($stateParams.a &&
          (parseInt($stateParams.a) === ages.to24 ||
                  parseInt($stateParams.a) === ages.to34 ||
                  parseInt($stateParams.a) === ages.to44 ||
                  parseInt($stateParams.a) === ages.to54 ||
                  parseInt($stateParams.a) === ages.to64 ||
                  parseInt($stateParams.a) === ages.more64
          )) $scope.age = parseInt($stateParams.a);
          else $scope.age = 1;
          if ($stateParams.s && (parseInt($stateParams.s) === sexs.Man || parseInt($stateParams.s) === sexs.Woman)) $scope.sex = parseInt($stateParams.s);
          else $scope.sex = 1;
          if ($stateParams.g) $scope.growth = parseInt($stateParams.g);
          if ($stateParams.w) $scope.weight = parseInt($stateParams.w);

          if ($stateParams.g && $stateParams.w) calculateAfterLoad = true;
      }

      function getCorrection(age) {
          if (age === ages.to24) return 0;
          if (age === ages.to34) return 1;
          if (age === ages.to44) return 2;
          if (age === ages.to54) return 3;
          if (age === ages.to64) return 4;
          if (age === ages.more64) return 5;
          return 0;
      };
      function getBmi(sex, age, type) {
          if (sex === sexs.Man) {
              if (type === bmis.DeadlineUnderweight) {
                  return 16.5 + getCorrection(age);
              }
              else if (type === bmis.LackOfWeight) {
                  return 20 + getCorrection(age);
              }
              else if (type === bmis.NormalWeight) {
                  return 25 + getCorrection(age);
              }
              else if (type === bmis.Overweight) {
                  return 30 + getCorrection(age);
              }
              else if (type === bmis.ObesityClassI) {
                  return 35 + getCorrection(age);
              }
              else if (type === bmis.ObesityClassII) {
                  return 40 + getCorrection(age);
              }
              else{
                  return 100;
              }
          } else {
              if (type === bmis.DeadlineUnderweight) {
                  return 15.5 + getCorrection(age);
              }
              else if (type === bmis.LackOfWeight) {
                  return 19 + getCorrection(age);
              }
              else if (type === bmis.NormalWeight) {
                  return 24 + getCorrection(age);
              }
              else if (type === bmis.Overweight) {
                  return 30 + getCorrection(age);
              }
              else if (type === bmis.ObesityClassI) {
                  return 35 + getCorrection(age);
              }
              else if (type === bmis.ObesityClassII) {
                  return 40 + getCorrection(age);
              }
              else {
                  return 100;
              }
          }
      };
      function textBmi(value,sex,age)
      {
          if (value <= getBmi(sex, age, bmis.DeadlineUnderweight)) return $scope.translation.DeadlineUnderweight;
          if (value <= getBmi(sex, age, bmis.LackOfWeight)) return $scope.translation.LackOfWeight;
          if (value <= getBmi(sex, age, bmis.NormalWeight)) return $scope.translation.NormalWeight;
          if (value <= getBmi(sex, age, bmis.Overweight)) return $scope.translation.Overweight;
          if (value <= getBmi(sex, age, bmis.ObesityClassI)) return $scope.translation.ObesityClassI;
          if (value <= getBmi(sex, age, bmis.ObesityClassII)) return $scope.translation.ObesityClassII;
          return $scope.translation.ObesityClassIII;
      };

      $scope.calculate = function () {

          if ($stateParams) {
              $location.search('a', $scope.age !== 1 ? $scope.age : null);
              $location.search('s', $scope.sex !== 1 ? $scope.sex : null);
              $location.search('g', $scope.growth !== 0 ? $scope.growth : null);
              $location.search('w', $scope.weight !== 0 ? $scope.weight : null);
          }

          $scope.bmi = parseInt($scope.weight / (($scope.growth / 100) * ($scope.growth / 100)));
          $scope.level = textBmi($scope.bmi, $scope.sex, $scope.age);

          var start;
          var end;
          var color;
          var bmi;
          var dataProvider = [];

          start = 0;
          end = parseInt(getBmi($scope.sex, $scope.age, bmis.DeadlineUnderweight) * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#FF0F00";
          bmi = $scope.translation.BMI + " <= " + getBmi($scope.sex, $scope.age, bmis.DeadlineUnderweight);
          dataProvider.push({ "name": $scope.translation.DeadlineUnderweight, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = parseInt(getBmi($scope.sex, $scope.age, bmis.LackOfWeight) * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#F8FF01";
          bmi = $scope.translation.BMI + " > " + getBmi($scope.sex, $scope.age, bmis.DeadlineUnderweight) + " & <= " + getBmi($scope.sex, $scope.age, bmis.LackOfWeight);
          dataProvider.push({ "name": $scope.translation.LackOfWeight, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = parseInt(getBmi($scope.sex, $scope.age, bmis.NormalWeight) * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#04D215";
          bmi = $scope.translation.BMI + " > " + getBmi($scope.sex, $scope.age, bmis.LackOfWeight) + " & <= " + getBmi($scope.sex, $scope.age, bmis.NormalWeight);
          dataProvider.push({ "name": $scope.translation.NormalWeight, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = parseInt(getBmi($scope.sex, $scope.age, bmis.Overweight) * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#F8FF01";
          bmi = $scope.translation.BMI + " > " + getBmi($scope.sex, $scope.age, bmis.NormalWeight) + " & <= " + getBmi($scope.sex, $scope.age, bmis.Overweight);
          dataProvider.push({ "name": $scope.translation.Overweight, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = parseInt(getBmi($scope.sex, $scope.age, bmis.ObesityClassI) * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#FF9E01";
          bmi = $scope.translation.BMI + " > " + getBmi($scope.sex, $scope.age, bmis.Overweight) + " & <= " + getBmi($scope.sex, $scope.age, bmis.ObesityClassI);
          dataProvider.push({ "name": $scope.translation.ObesityClassI, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = parseInt(getBmi($scope.sex, $scope.age, bmis.ObesityClassII) * (($scope.growth / 100) * ($scope.growth / 100)));
          color = "#FF0F00";
          bmi = $scope.translation.BMI + " > " + getBmi($scope.sex, $scope.age, bmis.ObesityClassI) + " & <= " + getBmi($scope.sex, $scope.age, bmis.ObesityClassII);
          dataProvider.push({ "name": $scope.translation.ObesityClassII, "start": start, "end": end, "color": color, "bmi": bmi });

          start = end;
          end = 180;
          color = "#FF0F00";
          bmi = $scope.translation.BMI + " > 40";
          bmi = $scope.translation.BMI + " > " + getBmi($scope.sex, $scope.age, bmis.ObesityClassII);
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
