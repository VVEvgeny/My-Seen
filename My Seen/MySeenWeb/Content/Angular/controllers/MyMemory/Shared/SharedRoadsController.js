App.config(function($stateProvider) {

    $stateProvider
        .state('mymemory/sharedRoads', {
            url: '/mymemory/roads/shared/:key?search',
            templateUrl: "Content/Angular/templates/MyMemory/Shared/roads.html",
            controller: 'SharedRoadsController',
            reloadOnSearch: false
        });
});

App.controller('SharedRoadsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      if (!$stateParams.key) {
          $state.go('mymemory/roads');
      }
      //������ ��������, ��� �������� � �������
      $rootScope.pageId = constants.PageIds.Roads;
      //�������� �� ���� ������
      $scope.pageCanSearch = true;
      //������� ���� ������ �� ���. ��������
      $scope.translation = {};
      //�������� �������� �� ��������� � �������
      $scope.prepared = {};
      
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
      var calcTab = true;
      function fillScope(page) {
          $scope.data = page;
          if (calcTab) {
              if ($scope.data.DataFoot.length > 0) $scope.currentTab = 1;
              else if ($scope.data.DataBike.length > 0) $scope.currentTab = 3;
              else if ($scope.data.DataCar.length > 0) $scope.currentTab = 2;
          }
          calcTab = true;
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope,
              {
                  pageId: $rootScope.pageId,
                  shareKey: $stateParams.key,
                  year: ($stateParams ? $stateParams.year : null),
                  search: ($stateParams ? $stateParams.search : null)
              });
      };

      //����� 3 ������� �� ������, ����� ����� ������ ������� �� ����� ������ � �� ����������/���������
      $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: $rootScope.pageId });
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
      getMainPage();

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           �����
      ///////////////////////////////////////////////////////////////////////
      $scope.quickSearch = {};
      $scope.quickSearch.text = $stateParams ? $stateParams.search : null;
      $scope.searchButtonClick = function() {
          $location.search('search', $scope.quickSearch.text !== ''? $scope.quickSearch.text: null);
          $location.search('page', null);//� ������ �������� ����� �����
          if ($stateParams) $stateParams.page = null;
          if ($stateParams) $stateParams.search = $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null;
          getMainPage();
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ����� ����
      ///////////////////////////////////////////////////////////////////////
      $scope.year = $stateParams ? $stateParams.year ? $stateParams.year.toString() : '0' : '0';
      $scope.selectedChange = function () {
          $location.search('year', $scope.year === '0' ? null : $scope.year);
          if ($stateParams) {
              $stateParams.year = $scope.year === '0' ? null : $scope.year;
          }
          getMainPage();
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ������� �������
      ///////////////////////////////////////////////////////////////////////
      $scope.currentTab = 1;
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           �����
      ///////////////////////////////////////////////////////////////////////
      $scope.showRoad = function (index) {
          //console.log("index=" + index + " type=" + type);
          clearMap();

          var array;
          if (index === 0) {
              if ($scope.currentTab === 1) {
                  array = $scope.data.DataFoot;
              } else if ($scope.currentTab === 2) {
                  array = $scope.data.DataCar;
              } else {
                  array = $scope.data.DataBike;
              }
          } else {
              if ($scope.currentTab === 1) {
                  array = $scope.data.DataFoot[index];
              } else if ($scope.currentTab === 2) {
                  array = $scope.data.DataCar[index];
              } else {
                  array = $scope.data.DataBike[index];
              }
          }
          if (!array.length) { //���� ��� �� ������ ������ 1 �������
              showRoad(array);
          } else {
              array.forEach(function (item) {
                  showRoad(item);
              });
          }

          autoFit();
      };
      //������� ������� ��������
      function afterSave() {
          $scope.addModalHide();
          calcTab = false;
          getMainPage();
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           Id by index
      ///////////////////////////////////////////////////////////////////////
      function getId(index) {
          if ($scope.currentTab === 1) {
              return $scope.data.DataFoot[index].Id;
          }
          else if ($scope.currentTab === 2) {
              return $scope.data.DataCar[index].Id;
          }
          return $scope.data.DataBike[index].Id;
      };
      //��������� ����� ������� ������
      $scope.modal = {};
      $scope.modal.deleteButtonClick = function (id) {
          $rootScope.GetPage(constants.Pages.Delete, $http, afterSave, { pageId: $rootScope.pageId, recordId: getId(id) });
      };
  }]);
