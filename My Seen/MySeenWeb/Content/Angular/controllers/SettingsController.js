App.config(function ($stateProvider) {

    $stateProvider
        .state('settings', {
            url: '/settings/',
            templateUrl: "Content/Angular/templates/main_pages/settings.html",
            controller: 'SettingsController',
            reloadOnSearch: false
        });
});

App.controller('SettingsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //На всякий случай закрою, может переход со страницы, где забыли закрыть модальную
      $rootScope.clearControllers();

      //Индекс страницы, для запросов к серверу
      var pageId = 106;

      //Перевод
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      //Основные данные
      function fillScope(page) {
          $scope.data = page;
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope, { pageId: pageId });
      };

      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: pageId });
      getMainPage();


      function afterLanguageChange() {
          //window.location.href = window.location.href; //не юзает пост
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
      $scope.facebookServicesChange = function () {
          $rootScope.GetPage(constants.PagesSettings.SetFacebookService, $http, null, { val: $scope.data.FacebookServiceEnabledInt });
      };
  }]);
