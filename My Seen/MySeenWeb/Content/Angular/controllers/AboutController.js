App.config(function($stateProvider) {

    $stateProvider
        .state('about', {
            url: '/about/',
            templateUrl: "Content/Angular/templates/about.html",
            controller: 'AboutController',
            reloadOnSearch: false
        });
});

App.controller('AboutController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      $rootScope.pageId = constants.PageIds.About;
  }]);
