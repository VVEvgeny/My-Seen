App.config(function($stateProvider) {

    $stateProvider
        .state('mymemory/sharedFilms', {
            url: '/mymemory/films/shared/:key?page&search',
            templateUrl: "Content/Angular/templates/MyMemory/Shared/films.html",
            controller: 'SharedFilmsController',
            reloadOnSearch: false
        });
});

App.controller('SharedFilmsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      if (!$stateParams.key) {
          $state.go('mymemory/films');
      }

      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.clearControllers();

      //������ ��������, ��� �������� � �������
      var pageId = 0;
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
      function fillScope(page) {
          $scope.data = page.Data;
          $scope.isMyData = page.IsMyData;
          $scope.pages = page.Pages;
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope,
              {
                  pageId: pageId,
                  shareKey: $stateParams.key,
                  page: $stateParams.page,
                  search: $stateParams.search
              });
      };

      //console.log($stateParams);
      //console.log($stateParams.key);

      //����� 3 ������� �� ������, ����� ����� ������ ������� �� ����� ������ � �� ����������/���������
      $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: pageId });
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: pageId });
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
      ///////////////////////////////////////////////////////////////////////           ���������
      ///////////////////////////////////////////////////////////////////////
      //�� ��������� �������� �� ����������, ��� ������������� ����������, � ��� � ���� � ���������� ��� ���������� ����� reloadOnSearch: false      
      $scope.pagination = {};
      $scope.pagination.goToPage = function(page) {
          $location.search('page', page > 1 ? page : null);
          if ($stateParams) $stateParams.page = page > 1 ? page : null;
          getMainPage();
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ��������
      ///////////////////////////////////////////////////////////////////////
      $scope.deleteShareButtonClick = function (id) {
          $rootScope.GetPage(constants.Pages.DeleteShare, $http, getMainPage, { pageId: pageId, recordId: $scope.data[id].Id });
      };
  }]);
