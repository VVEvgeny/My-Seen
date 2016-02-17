App.config(function ($stateProvider) {

    $stateProvider
        .state('improvements', {
            url: '/improvements/?:page&search&complex&ended',
            templateUrl: "Content/Angular/templates/Main/improvements.html",
            controller: 'ImprovementsController',
            reloadOnSearch: false
        });
});

App.controller('ImprovementsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.clearControllers();

      //������ ��������, ��� �������� � �������
      var pageId = 103;
      //�������� �� ������ ��������
      $scope.pageCanAdd = true;
      //�������� �� ���� ������
      $scope.pageCanSearch = true;

      //������� ���� ������ �� ���. ��������
      $scope.translation = {};
      //�������� �������� �� ��������� � �������
      $scope.prepared = {};
      //��������� ����������/�������������� ��������� ����� ���� ����� ������
      $scope.modal = {
          showDescription: true,
          showComplexTypes: true
      };

      //��� ��������� ������� ������
      function fillPrepared(page) {
          $scope.prepared = page;
          $scope.prepared.loaded = true;
      }
      //������� ������� � ���������
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      //�������� ������
      function fillScope(page) {
          $scope.data = page.Data;
          $scope.pages = page.Pages;
          $scope.canControl = page.CanControl;
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope
              , {
                  pageId: pageId,
                  page: ($stateParams ? $stateParams.page : null),
                  search: ($stateParams ? $stateParams.search : null),
                  complex: ($stateParams ? $stateParams.complex : null),
                  ended: ($stateParams ? $stateParams.ended : null)
              });
      };

      //����� 3 ������� �� ������, ����� ����� ������ ������� �� ����� ������ � �� ����������/���������
      $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: pageId });
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: pageId });
      getMainPage();

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ��� �������
      ///////////////////////////////////////////////////////////////////////
      $scope.typeSelect = $stateParams ? $stateParams.ended ? $stateParams.ended : '0' : '0';
      $scope.selectedTypeChange = function () {
          $location.search('page', null);
          $location.search('ended', $scope.typeSelect === '0' ? null : $scope.typeSelect);
          if ($stateParams) {
              $stateParams.page = null;
              $stateParams.ended = $scope.typeSelect === '0' ? null : $scope.typeSelect;
          }
          getMainPage();
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ��������
      ///////////////////////////////////////////////////////////////////////
      $scope.complexSelect = $stateParams ? $stateParams.complex ? $stateParams.complex : '0' : '0';
      $scope.selectedChange = function () {
          $location.search('page', null);
          $location.search('complex', $scope.complexSelect === '0' ? null : $scope.complexSelect);
          if ($stateParams) {
              $stateParams.page = null;
              $stateParams.complex = $scope.complexSelect === '0' ? null : $scope.complexSelect;
          }
          getMainPage();
      };
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
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ��������� ���������� / ��������������
      ///////////////////////////////////////////////////////////////////////
      //����� ��������� ��������/�������������
      //������� ������ ��� ���������� ����� ������ � ��������� ���������
      $scope.addModalOpen = function () {
          $scope.modal.title = $scope.translation.TitleAdd;
          $scope.modal.description = '';

          if ($scope.modal.complexType !== $scope.complexSelect) {//�������� ����� ���, ��������� ������ �� �����
              if ($scope.complexSelect !== '0') {
                  $scope.modal.complexType = $scope.complexSelect;
              } else $scope.modal.complexType = '1';
          }

          $scope.modal.showComplexTypes = true;
          $scope.modal.showVersion = false;
          $scope.modal.addButton = true;
          $scope.modal.deleteButton = false;
          $scope.modal.saveButton = false;

          $("#AddModalWindow").modal("show");
      };
      $scope.addModalHide = function () {
          $("#AddModalWindow").modal("hide");
          $rootScope.clearControllers();
      };
      //� ������ ������ ������� ��������� � ������������ ������, � ������ ��������
      function afterAdd() {
          $scope.addModalHide();
          $location.search('page', null);//� ������ �������� ����� �����
          $location.search('search', null);
          if ($stateParams) {
              $stateParams.page = null;
              $stateParams.search = null;
          }
          $scope.quickSearch.text = null;
          getMainPage();
      };
      //������� ������� ��������
      function afterSave() {
          $scope.addModalHide();
          getMainPage();
      };
      //������� ������ ��� �������� � ������ ���������� AddData
      $scope.modal.addButtonClick = function () {
          $rootScope.GetPage(constants.Pages.Add, $http, afterAdd, {
              pageId: pageId,
              name: $scope.modal.description,
              type: $scope.modal.complexType
          });
      };
      //������� ��������� ��� ��������������
      $scope.modal.editButtonClick = function (id,forEnd) {
          $scope.editedIndex = id;
          $scope.forEnd = forEnd;

          $scope.modal.title = $scope.translation.TitleEdit;
          if (forEnd) {
              $scope.modal.showComplexTypes = false;
              $scope.modal.showVersion = true;

              $scope.modal.description = '';
              $scope.modal.version = '';
          } else {
              $scope.modal.showComplexTypes = true;
              $scope.modal.showVersion = false;

              $scope.modal.description = $scope.data[id].Text;
              if (parseInt($scope.modal.complexType) !== parseInt($scope.data[id].Complex)) $scope.modal.complexType = $scope.data[id].Complex.toString();
          }

          $scope.modal.deleteButton = true;
          $scope.modal.saveButton = true;
          $scope.modal.addButton = false;

          $("#AddModalWindow").modal("show");
      };
      //��������� ����� ��������� ������
      $scope.modal.saveButtonClick = function () {
          if ($scope.forEnd) {
              $rootScope.GetPage(constants.Pages.EndImprovement, $http, afterSave, {
                  pageId: pageId,
                  id: $scope.data[$scope.editedIndex].Id,
                  name: $scope.modal.description,
                  version: $scope.modal.version
              });
          } else {
              $rootScope.GetPage(constants.Pages.Update, $http, afterSave, {
                  pageId: pageId,
                  id: $scope.data[$scope.editedIndex].Id,
                  name: $scope.modal.description,
                  type: $scope.modal.complexType
              });
          }
      };
      //��������� ����� ������� ������
      $scope.modal.deleteButtonClick = function () {
          $rootScope.GetPage(constants.Pages.Delete, $http, afterSave, { pageId: pageId, recordId: $scope.data[$scope.editedIndex].Id });
      };
  }]);
