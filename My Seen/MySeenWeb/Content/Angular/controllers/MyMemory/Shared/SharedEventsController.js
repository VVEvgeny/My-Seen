App.config(function($stateProvider) {

    $stateProvider
        .state('mymemory/sharedEvents', {
            url: '/mymemory/events/shared/:key?page&search&ended',
            templateUrl: "Content/Angular/templates/MyMemory/Shared/events.html",
            controller: 'SharedEventsController',
            reloadOnSearch: false
        });
});

App.controller('SharedEventsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants', '$anchorScroll',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants, $anchorScroll) {

      if (!$stateParams.key) {
          $state.go('mymemory/events');
      }
      $anchorScroll();
      $rootScope.loading = true;
      //������ ��������, ��� �������� � �������
      $rootScope.pageId = constants.PageIds.Events;
      //�������� �� ���� ������
      $scope.pageCanSearch = true;
      //������� ���� ������ �� ���. ��������
      $scope.translation = {};
      //�������� �������� �� ��������� � �������
      $scope.prepared = {};
      
      //������� ������� � ���������
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
          if (!$scope.data) $rootScope.loading = true;
      }
      //�������� ������
      var eventsInterval = '';
      $scope.$on("$destroy", function () {
          clearInterval(eventsInterval);
      });
      //��� ��������� ������� ������
      function fillPrepared(page) {
          $scope.prepared = page;
          $scope.prepared.loaded = true;
          if (!$scope.data || !$scope.translation.loaded) $rootScope.loading = true;
      }
      function fillScope(page) {
          $scope.data = page.Data;
          $scope.isMyData = page.IsMyData;
          $scope.pages = page.Pages;

          clearInterval(eventsInterval);
          eventsInterval = setInterval(recalcEstimated, 1000);
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope,
              {
                  pageId: $rootScope.pageId,
                  shareKey: $stateParams.key,
                  page: $stateParams.page,
                  search: $stateParams.search,
                  ended: $stateParams.ended
              });
      };

      //console.log($stateParams);
      //console.log($stateParams.key);

      //����� 3 ������� �� ������, ����� ����� ������ ������� �� ����� ������ � �� ����������/���������
      $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: $rootScope.pageId });
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
      getMainPage();

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ��� �������
      ///////////////////////////////////////////////////////////////////////
      $scope.eventSelect = $stateParams ? $stateParams.ended ? $stateParams.ended : '0' : '0';
      $scope.selectedChange = function () {
          $location.search('page', null);
          $location.search('ended', $scope.eventSelect === '0' ? null : $scope.eventSelect);
          if ($stateParams) {
              $stateParams.page = null;
              $stateParams.ended = $scope.eventSelect === '0' ? null : $scope.eventSelect;
          }
          getMainPage();
      };
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
      ///////////////////////////////////////////////////////////////////////           ���������
      ///////////////////////////////////////////////////////////////////////
      //�� ��������� �������� �� ����������, ��� ������������� ����������, � ��� � ���� � ���������� ��� ���������� ����� reloadOnSearch: false      
      $scope.pagination = {};
      $scope.pagination.goToPage = function(page) {
          $location.search('page', page > 1 ? page : null);
          if ($stateParams) $stateParams.page = page > 1 ? page : null;
          getMainPage();
          $anchorScroll();
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ��������
      ///////////////////////////////////////////////////////////////////////
      $scope.deleteShareButtonClick = function (id) {
          $rootScope.GetPage(constants.Pages.DeleteShare, $http, getMainPage, { pageId: $rootScope.pageId, recordId: $scope.data[id].Id });
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ������
      ///////////////////////////////////////////////////////////////////////
      function recalcEstimated() {
          $rootScope.safeApply(function () {
              // every changes goes here
              if ($scope.data.length > 0) {
                  for (var i = 0; i < $scope.data.length; i++) {
                      if ($scope.data[i].EstimatedTo !== $scope.translation.Ready) {
                          $scope.data[i].EstimatedTo = getTime($scope.data[i].EstimatedTo);
                      }
                      if ($scope.data[i].HaveHistory) {
                          $scope.data[i].EstimatedLast = getTime($scope.data[i].EstimatedLast);
                      }
                  }
              }
          });
      };
  }]);
