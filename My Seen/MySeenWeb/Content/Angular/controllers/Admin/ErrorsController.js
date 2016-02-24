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

      //Индекс страницы, для запросов к серверу
      $rootScope.pageId = constants.PageIds.Errors;
      //Показать ли поле ПОИСКа
      $scope.pageCanSearch = true;
      //На всякий случай закрою, может переход со страницы, где забыли закрыть модальную
      $rootScope.clearControllers();
      //Перевод всех данных на тек. странице
      $scope.translation = {};

      //Перевод таблицы и модальной
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      //Основные данные
      function fillScope(page) {
          $scope.data = page.Data;
          $scope.pages = page.Pages;
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope, { pageId: $rootScope.pageId, page: ($stateParams ? $stateParams.page : null), search: ($stateParams ? $stateParams.search : null) });
      };

      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
      getMainPage();

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           SEARCH
      ///////////////////////////////////////////////////////////////////////
      $scope.quickSearch = {};
      $scope.quickSearch.text = $stateParams ? $stateParams.search : null;
      $scope.searchButtonClick = function () {
          $location.search('search', $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null);
          $location.search('page', null);//с первой страницы новый поиск
          if ($stateParams) $stateParams.page = null;
          if ($stateParams) $stateParams.search = $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null;
          getMainPage();
      };

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           PAGINATION
      ///////////////////////////////////////////////////////////////////////
      //Не использую перехода по состояниям, они перезагружают контроллер, а так у меня в настройках для контролера стоит reloadOnSearch: false      
      $scope.pagination = {};
      $scope.pagination.goToPage = function (page) {
          $location.search('page', page > 1 ? page : null);
          if ($stateParams) $stateParams.page = page > 1 ? page : null;
          getMainPage();
      }
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ACTIONS
      ///////////////////////////////////////////////////////////////////////
      $scope.removeAll = function () {
          $rootScope.GetPage(constants.Pages.RemoveAllError, $http, getMainPage, {});
      };
  }]);
