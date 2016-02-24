App.config(function ($stateProvider) {

    $stateProvider
        .state('mymemory/roadsEditor', {
            url: '/mymemory/roads/editor/:id',
            templateUrl: "Content/Angular/templates/MyMemory/roadsEditor.html",
            controller: 'RoadsEditorController',
            reloadOnSearch: false
        });
});

App.controller('RoadsEditorController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //������ ��������, ��� �������� � �������
      $rootScope.pageId = constants.PageIds.Roads;
      //�������� �� ������ ��������
      $scope.pageCanAdd = true;
      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.clearControllers();

      //������� ���� ������ �� ���. ��������
      $scope.translation = {};
      //�������� �������� �� ��������� � �������
      $scope.prepared = {};
      //��������� ����������/�������������� ��������� ����� ���� ����� ������
      $scope.modal = {
          showName: true,
          showWhen: true,
          showRoadTypes: true,
          showCoordinates: true
      };

      //��� ��������� ������� ������
      function fillPrepared(page) {
          $scope.prepared = page;
          $scope.prepared.loaded = true;
      }
      //������� ������� � ���������
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      //�������� ������
      function fillScope(page) {
          $scope.data = page;
          coordinates = 0;
          page.DataFoot[0].Coordinates.split(';').forEach(function (item) {
              if (item) {
                  current = {};
                  current.latLng = new window.google.maps.LatLng(item.split(",")[0], item.split(",")[1]);
                  addMarkerInEditor(true);
              }
          });
          calculateRoute();
          autoFit();
      };
      //����� 3 ������� �� ������, ����� ����� ������ ������� �� ����� ������ � �� ����������/���������
      $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: $rootScope.pageId });
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
      if ($stateParams.id) {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope, { pageId: $rootScope.pageId, road: $stateParams.id });
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ��������� ���������� / ��������������
      ///////////////////////////////////////////////////////////////////////
      //����� ��������� ��������/�������������
      //������� ������ ��� ���������� ����� ������ � ��������� ���������
      $scope.addModalOpen = function () {

          if ($stateParams.id) { //��� ��������
              $scope.modal.title = $scope.translation.TitleEdit;
              $scope.modal.name = $scope.data.DataFoot[0].Name;
              $scope.modal.datetimeNow = $scope.data.DataFoot[0].DateFullText;
              if ($scope.modal.roadType !== $scope.data.DataFoot[0].Type) $scope.modal.roadType = $scope.data.DataFoot[0].Type;

              $scope.modal.saveButton = true;
              $scope.modal.addButton = false;
          } else { //�� �����������
              $scope.modal.title = $scope.translation.TitleAdd;
              $scope.modal.name = '';
              $scope.modal.datetimeNow = $scope.prepared.DateTimeNow;
              if ($scope.modal.roadType !== $scope.prepared.TypeList[0].Value) $scope.modal.roadType = $scope.prepared.TypeList[0].Value;

              $scope.modal.saveButton = false;
              $scope.modal.addButton = true;
          }

          var $panel = $("#panelCoordinates");
          var $rows = $panel.find("div");
          $scope.modal.coordinates = '';
          $rows.each(function (index, element) {
              var $row = $(element);
              var id = $row.attr("id");
              //console.log("id=", id);
              id = id.replace('(', '').replace(')', '');
              $scope.modal.coordinates +=id+";";
          });
          
          $("#AddModalWindow").modal("show");
      };
      $scope.addModalHide = function () {
          $("#AddModalWindow").modal("hide");
          $rootScope.clearControllers();
      };
      //� ������ ������ ������� ��������� � ������������ ������, � ������ ��������
      function afterAdd() {
          $scope.addModalHide();
          $state.go('mymemory/roads');
      };
      //������� ������ ��� �������� � ������ ���������� AddData
      $scope.modal.addButtonClick = function () {
          $rootScope.GetPage(constants.Pages.Add, $http, afterAdd, {
              pageId: $rootScope.pageId,
              name: $scope.modal.name,
              type: $scope.modal.roadType,
              datetime: $scope.modal.datetimeNow,
              coordinates: $scope.modal.coordinates,
              distance: CalcDistanceFromTxt($scope.modal.coordinates)
          });
      };
      //��������� ����� ��������� ������
      $scope.modal.saveButtonClick = function () {
          $rootScope.GetPage(constants.Pages.Update, $http, afterAdd, {
              pageId: $rootScope.pageId,
              id: $stateParams.id,
              name: $scope.modal.name,
              type: $scope.modal.roadType,
              datetime: $scope.modal.datetimeNow,
              coordinates: $scope.modal.coordinates,
              distance: CalcDistanceFromTxt($scope.modal.coordinates)
          });
      };
  }]);
