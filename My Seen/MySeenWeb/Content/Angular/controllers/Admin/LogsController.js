App.config(function ($stateProvider) {

    $stateProvider
        .state('logs', {
            url: '/admin/logs/?:page&search',
            templateUrl: "Content/Angular/templates/Admin/logs.html",
            controller: 'LogsController',
            reloadOnSearch: false
        });
});

App.controller('LogsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants', '$anchorScroll',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants, $anchorScroll) {

      //������ ��������, ��� �������� � �������
      $rootScope.pageId = constants.PageIds.Logs;
      //�������� �� ���� ������
      $scope.pageCanSearch = true;
      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.clearControllers();
      //������� ���� ������ �� ���. ��������
      $scope.translation = {};

      //������� ������� � ���������
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      //�������� ������
      function fillScope(page) {
          $scope.data = page.Data;
          $scope.pages = page.Pages;
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope, { pageId: $rootScope.pageId, page: ($stateParams ? $stateParams.page : null), search: ($stateParams ? $stateParams.search : null) });
      };

      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
      getMainPage();

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           �����
      ///////////////////////////////////////////////////////////////////////
      $scope.quickSearch = {};
      $scope.quickSearch.text = $stateParams ? $stateParams.search : null;
      $scope.searchButtonClick = function () {
          $location.search('search', $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null);
          $location.search('page', null);//� ������ �������� ����� �����
          if ($stateParams) $stateParams.page = null;
          if ($stateParams) $stateParams.search = $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null;
          getMainPage();
      };

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ���������
      ///////////////////////////////////////////////////////////////////////
      //�� ��������� �������� �� ����������, ��� ������������� ����������, � ��� � ���� � ���������� ��� ���������� ����� reloadOnSearch: false      
      $scope.pagination = {};
      $scope.pagination.goToPage = function (page) {
          $location.search('page', page > 1 ? page : null);
          if ($stateParams) $stateParams.page = page > 1 ? page : null;
          getMainPage();
          $anchorScroll();
      }
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           COLLAPSE
      ///////////////////////////////////////////////////////////////////////
      $scope.showPost = function(id) {
          if (id === 'all') {
              if ($("#postPlus_" + id).hasClass("hidden")) {

                  $("#postMinus_" + id).addClass("hidden");
                  $("#postPlus_" + id).removeClass("hidden");

                  if ($scope.data.length > 0) {
                      for (var i = 0; i < $scope.data.length; i++) {
                          $("#collapseme_" + i).addClass("out");
                          $("#collapseme_" + i).removeClass("in");

                          $("#postMinus_" + i).addClass("hidden");
                          $("#postPlus_" + i).removeClass("hidden");
                      }
                  }

              } else {
                  $("#postPlus_" + id).addClass("hidden");
                  $("#postMinus_" + id).removeClass("hidden");

                  if ($scope.data.length > 0) {
                      for (var i = 0; i < $scope.data.length; i++) {
                          $("#collapseme_" + i).addClass("in");
                          $("#collapseme_" + i).removeClass("out");

                          $("#postPlus_" + i).addClass("hidden");
                          $("#postMinus_" + i).removeClass("hidden");
                      }
                  }
              }
          } else {
              if ($("#postPlus_" + id).hasClass("hidden")) {

                  $("#collapseme_" + id).addClass("out");
                  $("#collapseme_" + id).removeClass("in");

                  $("#postMinus_" + id).addClass("hidden");
                  $("#postPlus_" + id).removeClass("hidden");
              } else {
                  $("#collapseme_" + id).addClass("in");
                  $("#collapseme_" + id).removeClass("out");

                  $("#postPlus_" + id).addClass("hidden");
                  $("#postMinus_" + id).removeClass("hidden");
              }
          }
      };
  }]);
