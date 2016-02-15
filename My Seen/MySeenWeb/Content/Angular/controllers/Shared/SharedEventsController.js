App.config(function($stateProvider) {

    $stateProvider
        .state('sharedEvents', {
            url: '/events/shared/:key?page&search',
            templateUrl: "Content/Angular/templates/shared_pages/events.html",
            controller: 'SharedEventsController',
            reloadOnSearch: false
        });
});

App.controller('SharedEventsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      if (!$stateParams.key) {
          $state.go('events');
      }

      //Индекс страницы, для запросов к серверу
      var pageId = 4;
      
      //Перевод всех данных на тек. странице
      $scope.translation = {};
      //Загрузка значений по умолчанию и списков
      $scope.prepared = {};
      
      //Для модальной готовим данные
      function fillPrepared(page) {
          $scope.prepared = page;
          $scope.prepared.loaded = true;
      }
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
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope,
              {
                  pageId: pageId,
                  shareKey: $stateParams.key,
                  page: $stateParams.page,
                  search: $stateParams.search
              });
      };

      console.log($stateParams);
      console.log($stateParams.key);

      //Сразу 3 запроса на сервер, далее будет только запросы по новым данным и на добавление/изменение
      $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: pageId });
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: pageId });
      getMainPage();

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ПОИСК
      ///////////////////////////////////////////////////////////////////////
      $scope.quickSearch = {};
      $scope.quickSearch.text = $stateParams ? $stateParams.search : null;
      $scope.searchButtonClick = function() {
          $location.search('search', $scope.quickSearch.text !== ''? $scope.quickSearch.text: null);
          $location.search('page', null);//с первой страницы новый поиск
          if ($stateParams) $stateParams.page = null;
          if ($stateParams) $stateParams.search = $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null;
          getMainPage();
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ПАГИНАЦИЯ
      ///////////////////////////////////////////////////////////////////////
      //Не использую перехода по состояниям, они перезагружают контроллер, а так у меня в настройках для контролера стоит reloadOnSearch: false      
      $scope.pagination = {};
      $scope.pagination.goToPage = function (page) {
          $location.search('page', page > 1 ? page : null);
          if ($stateParams) $stateParams.page = page > 1 ? page : null;
          getMainPage();
      }
  }]);
