App.config(function ($stateProvider) {

    $stateProvider
        .state('events', {
            url: '/events/?:page&search&ended',
            templateUrl: "Content/Angular/templates/Main/events.html",
            controller: 'EventsController',
            reloadOnSearch: false
        });
});

App.controller('EventsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //������ ��������, ��� �������� � �������
      var pageId = 4;
      //�������� �� ������ ��������
      $scope.pageCanAdd = true;
      //�������� �� ���� ������
      $scope.pageCanSearch = true;

      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.clearControllers();

      //������� ���� ������ �� ���. ��������
      $scope.translation = {};
      //�������� �������� �� ��������� � �������
      $scope.prepared = {};
      //��������� ����������/�������������� ��������� ����� ���� ����� ������
      $scope.modal = {
          showName: true,
          showWhen: true,
          showEventTypes: true
      };
      //��������� �������
      $scope.modalShare = {};

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
      $rootScope.eventsInterval = '';
      function fillScope(page) {
          $scope.data = page.Data;
          $scope.pages = page.Pages;

          clearInterval($rootScope.eventsInterval);
          $rootScope.eventsInterval = setInterval(recalcEstimated, 1000);
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope
              , {
                  pageId: pageId, page: ($stateParams ? $stateParams.page : null),
                  search: ($stateParams ? $stateParams.search : null),
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
      $scope.eventSelect = $stateParams ? $stateParams.ended ? $stateParams.ended : '0' : '0';
      $scope.selectedChange = function() {
          $location.search('page', null);
          $location.search('ended', $scope.eventSelect === '0' ? null : $scope.eventSelect);
          if ($stateParams) {
              $stateParams.page = null;
              $stateParams.ended = $scope.eventSelect === '0' ? null : $scope.eventSelect;
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
      $scope.pagination.goToPage = function(page) {
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
          $scope.modal.name = '';
          $scope.modal.datetimeNow = $scope.prepared.DateTimeNow;
          if ($scope.modal.eventType !== $scope.prepared.TypeList[0].Value) $scope.modal.eventType = $scope.prepared.TypeList[0].Value;

          $scope.modal.addButton = true;
          $scope.modal.shareButton = false;
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
              name: $scope.modal.name,
              type: $scope.modal.eventType,
              datetime: $scope.modal.datetimeNow
          });
      };
      //������� ��������� ��� ��������������
      $scope.modal.editButtonClick = function (id) {
          $scope.editedIndex = id;
          $scope.modal.title = $scope.translation.TitleEdit;
          $scope.modal.name = $scope.data[id].Name;
          $scope.modal.datetimeNow = $scope.data[id].DateText;
          if (parseInt($scope.modal.eventType) !== parseInt($scope.data[id].RepeatType)) $scope.modal.eventType = $scope.data[id].RepeatType;

          $scope.modal.shareButton = true;
          $scope.modal.deleteButton = true;
          $scope.modal.saveButton = true;
          $scope.modal.addButton = false;

          $("#AddModalWindow").modal("show");
      };
      //��������� ����� ��������� ������
      $scope.modal.saveButtonClick = function () {
          $rootScope.GetPage(constants.Pages.Update, $http, afterSave, {
              pageId: pageId,
              id: $scope.data[$scope.editedIndex].Id,
              name: $scope.modal.name,
              type: $scope.modal.eventType,
              datetime: $scope.modal.datetimeNow
          });
      };
      //��������� ����� ������� ������
      $scope.modal.deleteButtonClick = function () {
          $rootScope.GetPage(constants.Pages.Delete, $http, afterSave, { pageId: pageId, recordId: $scope.data[$scope.editedIndex].Id });
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ��������� �������
      ///////////////////////////////////////////////////////////////////////
      //��������� ����� �������� � ��������� �������
      function getShareCallBack(link) {
          $scope.modalShare.loading = false;
          if (link === '-') { //��� ������, �������� ������ - ��������
              $scope.modalShare.addButton = true;
              $scope.modalShare.link = '';
          } else { //���� ������, ��������, �������� ������ - ����������� + �������
              $scope.modalShare.tryButton = true;
              $scope.modalShare.deleteButton = true;
              $scope.modalShare.link = link;
          }
          getMainPage();
      };
      //����� ���������, �� ����� ���� �� ��������� ��������������, ����� �������� $scope.editedIndex (������ ������ �������� �������� �� 0, ��� if(0) ��� false)
      $scope.modalShare.shareButtonClick = function (id) {
          if (!id && id !== 0) id = $scope.editedIndex;

          $scope.editedIndex = id;
          $scope.modalShare.loading = true;

          $scope.modalShare.addButton = false;
          $scope.modalShare.tryButton = false;
          $scope.modalShare.deleteButton = false;

          $rootScope.GetPage(constants.Pages.GetShare, $http, getShareCallBack, { pageId: pageId, recordId: $scope.data[$scope.editedIndex].Id });

          $("#ShareModalWindow").modal("show");
      };
      //������ ������ ����������� � ��������� �������, ������� �� �������� ������� ������
      $scope.modalShare.tryButtonClick = function () {
          window.open($scope.modalShare.link, '_blank');
      };
      //������� ������ � ������� ������ �� ��������� �������
      $scope.modalShare.deleteButtonClick = function () {
          $scope.modalShare.loading = true;

          $scope.modalShare.addButton = false;
          $scope.modalShare.tryButton = false;
          $scope.modalShare.deleteButton = false;

          $rootScope.GetPage(constants.Pages.DeleteShare, $http, getShareCallBack, { pageId: pageId, recordId: $scope.data[$scope.editedIndex].Id });
      };
      //��������� ������ �� ��������� �������
      $scope.modalShare.addButtonClick = function () {
          $scope.modalShare.loading = true;

          $scope.modalShare.addButton = false;
          $scope.modalShare.tryButton = false;
          $scope.modalShare.deleteButton = false;

          $rootScope.GetPage(constants.Pages.GenerateShare, $http, getShareCallBack, { pageId: pageId, recordId: $scope.data[$scope.editedIndex].Id });
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ������
      ///////////////////////////////////////////////////////////////////////
      function recalcEstimated() {
          $scope.$apply(function() {
              // every changes goes here
              if ($scope.data.length > 0) {
                  for (var i = 0; i < $scope.data.length; i++) {
                      if ($scope.data[i].EstimatedTo !== $scope.translation.Ready) {
                          $scope.data[i].EstimatedTo = getTime($scope.data[i].EstimatedTo);
                      }
                      if ($scope.data[i].HaveHistory) {
                          $scope.data[i].EstimatedLast = getTime($scope.data[i].EstimatedLast);
                      }
                  }
              }
          });
      };
  }]);
