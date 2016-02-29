App.config(function ($stateProvider) {

    $stateProvider
        .state('users', {
            url: '/admin/users/?:page&search',
            templateUrl: "Content/Angular/templates/Admin/users.html",
            controller: 'UsersController',
            reloadOnSearch: false
        });
});

App.controller('UsersController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants', '$anchorScroll',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants, $anchorScroll) {

      //Индекс страницы, для запросов к серверу
      $rootScope.pageId = constants.PageIds.Users;
      //Показать ли поле ПОИСКа
      $scope.pageCanSearch = true;
      //Перевод всех данных на тек. странице
      $scope.translation = {};

      //Перевод таблицы и модальной
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      //Основные данные
      function fillScope(page) {
          $scope.data = page.Data;
          $scope.defaultData = angular.copy($scope.data);
          $scope.pages = page.Pages;
          $scope.userRoles = page.UserRoles;
          $scope.canControl = page.CanControl;
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope, { pageId: $rootScope.pageId, page: ($stateParams ? $stateParams.page : null), search: ($stateParams ? $stateParams.search : null) });
      };

      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
      getMainPage();

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           SEARCH
      ///////////////////////////////////////////////////////////////////////
      $scope.quickSearch = {};
      $scope.quickSearch.text = $stateParams ? $stateParams.search : null;
      $scope.searchButtonClick = function () {
          $location.search('search', $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null);
          $location.search('page', null);//с первой страницы новый поиск
          if ($stateParams) $stateParams.page = null;
          if ($stateParams) $stateParams.search = $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null;
          getMainPage();
      };

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           PAGINATION
      ///////////////////////////////////////////////////////////////////////
      $scope.pagination = {};
      $scope.pagination.goToPage = function (page) {
          $location.search('page', page > 1 ? page : null);
          if ($stateParams) $stateParams.page = page > 1 ? page : null;
          getMainPage();
          $anchorScroll();
      }
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           MODAL
      ///////////////////////////////////////////////////////////////////////
      $scope.toggleSelection = function toggleSelection(index, role) {

          var idx = $scope.data[index].Roles.indexOf(role);

          // is currently selected
          if (idx > -1) {
              $scope.data[index].Roles.splice(idx, 1);
          }
          // is newly selected
          else {
              $scope.data[index].Roles.push(role);
          }
      };

      $scope.modal = {};
      $scope.modal.updateButtonClick = function (index) {
          console.log(index);
          console.log($scope.data[index].Roles);

          $rootScope.GetPage(constants.PagesAdmin.UpdateUser, $http, getMainPage, { name: $scope.data[index].Name, roles: $scope.data[index].Roles });
          //UpdateUser
      };
  }]);
