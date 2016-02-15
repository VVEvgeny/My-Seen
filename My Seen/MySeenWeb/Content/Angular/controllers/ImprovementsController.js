App.config(function ($stateProvider) {

    $stateProvider
        .state('improvements', {
            url: '/improvements/',
            templateUrl: "Content/Angular/templates/main_pages/improvements.html",
            controller: 'ImprovementsController',
            reloadOnSearch: false
        });
});

App.controller('ImprovementsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //На всякий случай закрою, может переход со страницы, где забыли закрыть модальную
      $rootScope.closeModals();

  }]);
