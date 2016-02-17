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

      //На всякий случай закрою, может переход со страницы, где забыли закрыть модальную
      $rootScope.clearControllers();

      //Индекс страницы, для запросов к серверу
      var pageId = 103;
      //Показать ли кнопку ДОБАВИТЬ
      $scope.pageCanAdd = true;
      //Показать ли поле ПОИСКа
      $scope.pageCanSearch = true;

      //Перевод всех данных на тек. странице
      $scope.translation = {};
      //Загрузка значений по умолчанию и списков
      $scope.prepared = {};
      //Модальная добавления/редактирования Указываем какие поля будем видеть
      $scope.modal = {
          showDescription: true,
          showComplexTypes: true
      };

      //Для модальной готовим данные
      function fillPrepared(page) {
          $scope.prepared = page;
          $scope.prepared.loaded = true;
      }
      //Перевод таблицы и модальной
      function fillTranslation(page) {
          $scope.translation = page;
          $scope.translation.loaded = true;
      }
      //Основные данные
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

      //Сразу 3 запроса на сервер, далее будет только запросы по новым данным и на добавление/изменение
      $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: pageId });
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: pageId });
      getMainPage();

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ТИП СОБЫИТЯ
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
      ///////////////////////////////////////////////////////////////////////           КОМПЛЕКС
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
      ///////////////////////////////////////////////////////////////////////           ПОИСК
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
      ///////////////////////////////////////////////////////////////////////           ПАГИНАЦИЯ
      ///////////////////////////////////////////////////////////////////////
      //Не использую перехода по состояниям, они перезагружают контроллер, а так у меня в настройках для контролера стоит reloadOnSearch: false      
      $scope.pagination = {};
      $scope.pagination.goToPage = function (page) {
          $location.search('page', page > 1 ? page : null);
          if ($stateParams) $stateParams.page = page > 1 ? page : null;
          getMainPage();
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           МОДАЛЬНАЯ ДОБАВЛЕНИЯ / РЕДАКТИРОВАНИЯ
      ///////////////////////////////////////////////////////////////////////
      //Прячу модальную Добавить/Редактировать
      //Готовлю данные для добавления новой записи и отображаю модальную
      $scope.addModalOpen = function () {
          $scope.modal.title = $scope.translation.TitleAdd;
          $scope.modal.description = '';

          if ($scope.modal.complexType !== $scope.complexSelect) {//Выбирать можно все, добавлять только по типам
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
      //в случае успеха закроем модальное и перезапросим данные, с первой страницы
      function afterAdd() {
          $scope.addModalHide();
          $location.search('page', null);//с первой страницы новый поиск
          $location.search('search', null);
          if ($stateParams) {
              $stateParams.page = null;
              $stateParams.search = null;
          }
          $scope.quickSearch.text = null;
          getMainPage();
      };
      //Обновим текущую страницу
      function afterSave() {
          $scope.addModalHide();
          getMainPage();
      };
      //Готовлю данные для отправки и вызову глобальную AddData
      $scope.modal.addButtonClick = function () {
          $rootScope.GetPage(constants.Pages.Add, $http, afterAdd, {
              pageId: pageId,
              name: $scope.modal.description,
              type: $scope.modal.complexType
          });
      };
      //Готовлю модальную для редактирования
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
      //Модальная хочет сохранить данные
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
      //Модальная хочет удалить данные
      $scope.modal.deleteButtonClick = function () {
          $rootScope.GetPage(constants.Pages.Delete, $http, afterSave, { pageId: pageId, recordId: $scope.data[$scope.editedIndex].Id });
      };
  }]);
