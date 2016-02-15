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

      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.closeModals();

  }]);
