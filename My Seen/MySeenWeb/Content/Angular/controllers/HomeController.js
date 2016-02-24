App.config(function ($stateProvider) {

    $stateProvider
        .state('/', {
            url: '/',
            templateUrl: "Content/Angular/templates/home.html",
            controller: 'HomeController',
            reloadOnSearch: false
        });
});

App.controller('HomeController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //Ќа вс€кий случай закрою, может переход со страницы, где забыли закрыть модальную
      $rootScope.clearControllers();

      $rootScope.pageId = constants.PageIds.Main;

      //ѕеревод всех данных на тек. странице
      $scope.translation = {};
      //ѕеревод таблицы и модальной
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });

  }]);
