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

      $rootScope.pageId = constants.PageIds.Main;

      //Перевод всех данных на тек. странице
      $scope.translation = {};
      //Перевод таблицы и модальной
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });

  }]);
