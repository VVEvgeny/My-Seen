App.config(function ($stateProvider) {

    $stateProvider
        .state('settings', {
            url: '/settings/',
            templateUrl: "Content/Angular/templates/main_pages/settings.html",
            controller: 'FilmsController',
            reloadOnSearch: false
        });
});

App.controller('SettingsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.closeModals();

  }]);
