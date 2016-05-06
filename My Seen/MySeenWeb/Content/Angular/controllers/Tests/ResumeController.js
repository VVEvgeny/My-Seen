App.config(function($stateProvider) {

    $stateProvider
        .state('resume',
        {
            url: '/tests/resume/',
            templateUrl: "Content/Angular/templates/tests/resume.html",
            controller: 'ResumeController',
            reloadOnSearch: false
        });
});

App.controller('ResumeController',
[
    '$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants', '$anchorScroll',
    function($scope, $rootScope, $state, $stateParams, $http, $location, constants, $anchorScroll) {

        $anchorScroll();
        $rootScope.pageId = constants.PageIds.Resume;

    }
]);
