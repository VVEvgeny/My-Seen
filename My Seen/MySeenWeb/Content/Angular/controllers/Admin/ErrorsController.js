App.config(function ($stateProvider) {

    $stateProvider
        .state('errors', {
            url: '/admin/errors/?:page&search',
            templateUrl: "Content/Angular/templates/Admin/errors.html",
            controller: 'ErrorsController',
            reloadOnSearch: false
        });
});

App.controller('ErrorsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //������ ��������, ��� �������� � �������
      var pageId = 104;
      //�������� �� ���� ������
      $scope.pageCanSearch = true;
      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.clearControllers();
      //������� ���� ������ �� ���. ��������
      $scope.translation = {};

      //������� ������� � ���������
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      //�������� ������
      function fillScope(page) {
          $scope.data = page.Data;
          $scope.pages = page.Pages;
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope, { pageId: pageId, page: ($stateParams ? $stateParams.page : null), search: ($stateParams ? $stateParams.search : null) });
      };

      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: pageId });
      getMainPage();

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           �����
      ///////////////////////////////////////////////////////////////////////
      $scope.quickSearch = {};
      $scope.quickSearch.text = $stateParams ? $stateParams.search : null;
      $scope.searchButtonClick = function () {
          $location.search('search', $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null);
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
      $scope.pagination.goToPage = function (page) {
          $location.search('page', page > 1 ? page : null);
          if ($stateParams) $stateParams.page = page > 1 ? page : null;
          getMainPage();
      }

  }]);
