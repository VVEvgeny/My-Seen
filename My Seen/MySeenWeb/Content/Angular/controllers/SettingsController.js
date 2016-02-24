App.config(function ($stateProvider) {

    $stateProvider
        .state('settings', {
            url: '/settings/',
            templateUrl: "Content/Angular/templates/settings.html",
            controller: 'SettingsController',
            reloadOnSearch: false
        });
});

App.controller('SettingsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.clearControllers();

      //������ ��������, ��� �������� � �������
      $rootScope.pageId = constants.PageIds.Settings;

      //�������
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      //�������� ������
      function fillScope(page) {
          $scope.data = page;
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope, { pageId: $rootScope.pageId });
      };

      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
      getMainPage();


      function afterLanguageChange() {
          //window.location.href = window.location.href; //�� ����� ����
          window.location.reload();
      };
      $scope.languageChange = function () {
          $scope.translation.loaded = false;
          $rootScope.GetPage(constants.PagesSettings.SetLanguage, $http, afterLanguageChange, { val: $scope.data.Lang });
      };
      $scope.rppChange = function () {
          $rootScope.GetPage(constants.PagesSettings.SetRpp, $http, null, { val: $scope.data.Rpp });
      };
      $scope.markersChange = function () {
          $rootScope.GetPage(constants.PagesSettings.SetMor, $http, null, { val: $scope.data.Markers });
      };
      $scope.googleServicesChange = function () {
          $rootScope.GetPage(constants.PagesSettings.SetGoogleService, $http, null, { val: $scope.data.GoogleServiceEnabledInt });
      };
      $scope.vkServicesChange = function () {
          $rootScope.GetPage(constants.PagesSettings.SetVkService, $http, null, { val: $scope.data.VkServiceEnabledInt });
      };
      $scope.facebookServicesChange = function() {
          $rootScope.GetPage(constants.PagesSettings.SetFacebookService, $http, null, { val: $scope.data.FacebookServiceEnabledInt });
      };

      $scope.modalSetPassword = {};
      $scope.setPassword = function () {
          $scope.modalSetPassword.password = '';
          $scope.modalSetPassword.newPassword = '';
          $scope.modalSetPassword.passwordConfirm = '';

          $("#PasswordModalWindow").modal("show");
      };
      
      function afterSetPassword() {
          $("#PasswordModalWindow").modal("hide");
          $rootScope.clearControllers();

          getMainPage();
      };
      $scope.modalSetPassword.addButtonClick = function () {
          $rootScope.GetPage(constants.PagesSettings.SetPassword, $http, afterSetPassword,
              {
                  password: $scope.modalSetPassword.password,
                  newPassword: $scope.modalSetPassword.newPassword
              }
          );
      };

      function afterGetLogins(page) {
          $scope.userLogins = page.UserLogins;
          $scope.otherLogins = page.OtherLogins;

          $("#LoginsModalWindow").modal("show");
      };
      function getLogins() {
          $rootScope.GetPage(constants.PagesSettings.GetLogins, $http, afterGetLogins, { });
      };
      $scope.manageExternals = function () {
          getLogins();
      };

      $scope.modalLogins = {};
      $scope.modalLogins.removeButtonClick = function (id) {
          $rootScope.GetPage(constants.PagesSettings.RemoveLogin, $http, getLogins, { loginProvider: $scope.userLogins[id].LoginProvider, providerKey: $scope.userLogins[id].ProviderKey });
      };
  }]);
