App.config(function ($stateProvider) {

    $stateProvider
        .state('resume', {
            url: '/tests/resume/',
            templateUrl: "Content/Angular/templates/tests/resume.html",
            controller: 'ResumeController',
            reloadOnSearch: false
        });
});

var TestWebGLRun = false;

App.controller('ResumeController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {
      
      $rootScope.pageId = constants.PageIds.Resume;

  }]);
