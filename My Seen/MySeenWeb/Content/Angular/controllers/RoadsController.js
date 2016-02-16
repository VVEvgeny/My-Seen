App.config(function ($stateProvider) {

    $stateProvider
        .state('roads', {
            url: '/roads/?:year&search',
            templateUrl: "Content/Angular/templates/main_pages/roads.html",
            controller: 'RoadsController',
            reloadOnSearch: false
        });
});

App.controller('RoadsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //Индекс страницы, для запросов к серверу
      var pageId = 3;
      //Показать ли кнопку ДОБАВИТЬ
      $scope.pageCanAdd = true;
      //Показать ли поле ПОИСКа
      $scope.pageCanSearch = true;
      //На всякий случай закрою, может переход со страницы, где забыли закрыть модальную
      $rootScope.clearControllers();

      //Перевод всех данных на тек. странице
      $scope.translation = {};
      //Загрузка значений по умолчанию и списков
      $scope.prepared = {};
      //Модальная добавления/редактирования Указываем какие поля будем видеть
      $scope.modal = {
          showName: true,
          showWhen: true,
          showRoadTypes: true,
          showCoordinates: true
      };
      //Модальная доступа
      $scope.modalShare = {};

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
      var calcTab = true;
      function fillScope(page) {
          $scope.data = page;
          if (calcTab) {
              if ($scope.data.DataFoot.length > 0) $scope.currentTab = 1;
              else if ($scope.data.DataBike.length > 0) $scope.currentTab = 3;
              else if ($scope.data.DataCar.length > 0) $scope.currentTab = 2;
          }
          calcTab = true;
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope,
              {
                  pageId: pageId,
                  year: ($stateParams ? $stateParams.year : null),
                  search: ($stateParams ? $stateParams.search : null)
              });
      };

      //Сразу 3 запроса на сервер, далее будет только запросы по новым данным и на добавление/изменение
      $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: pageId });
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: pageId });
      getMainPage();

      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           ПОИСК
      ///////////////////////////////////////////////////////////////////////
      $scope.quickSearch = {};
      $scope.quickSearch.text = $stateParams ? $stateParams.search : null;
      $scope.searchButtonClick = function () {
          $location.search('search', $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null);
          if ($stateParams) $stateParams.search = $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null;
          getMainPage();
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           Выбор года
      ///////////////////////////////////////////////////////////////////////
      $scope.year = $stateParams ? $stateParams.year ? $stateParams.year.toString() : '0' : '0';
      $scope.selectedChange = function () {
          $location.search('year', $scope.year === '0' ? null : $scope.year);
          if ($stateParams) {
              $stateParams.year = $scope.year === '0' ? null : $scope.year;
          }
          getMainPage();
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           Текущая вкладка
      ///////////////////////////////////////////////////////////////////////
      $scope.currentTab = 1;
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           КАРТА
      ///////////////////////////////////////////////////////////////////////
      $scope.showRoad = function(index) {
          //console.log("index=" + index + " type=" + type);
          clearMap();

          var array;
          if (index === 0) {
              if ($scope.currentTab === 1) {
                  array = $scope.data.DataFoot;
              } else if ($scope.currentTab === 2) {
                  array = $scope.data.DataCar;
              } else {
                  array = $scope.data.DataBike;
              }
          } else {
              if ($scope.currentTab === 1) {
                  array = $scope.data.DataFoot[index];
              } else if ($scope.currentTab === 2) {
                  array = $scope.data.DataCar[index];
              } else {
                  array = $scope.data.DataBike[index];
              }
          }
          if (!array.length) { //Если это не массив значит 1 элемент
              showRoad(array);
          } else {
              array.forEach(function(item) {
                  showRoad(item);
              });
          }

          autoFit();
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           Id by index
      ///////////////////////////////////////////////////////////////////////
      function getId(index) {
          if ($scope.currentTab === 1) {
              return $scope.data.DataFoot[index].Id;
          }
          else if ($scope.currentTab === 2) {
              return $scope.data.DataCar[index].Id;
          }
          return $scope.data.DataBike[index].Id;
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           МОДАЛЬНАЯ ДОБАВЛЕНИЯ / РЕДАКТИРОВАНИЯ
      ///////////////////////////////////////////////////////////////////////
      //Прячу модальную Добавить/Редактировать
      //Готовлю данные для добавления новой записи и отображаю модальную
      $scope.addModalOpen = function () {
          $scope.modal.title = $scope.translation.TitleAdd;
          $scope.modal.name = '';
          $scope.modal.datetimeNow = $scope.prepared.DateTimeNow;
          if ($scope.modal.roadType !== $scope.prepared.TypeList[0].Value) $scope.modal.roadType = $scope.prepared.TypeList[0].Value;
          $scope.modal.coordinates = '';

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
      //в случае успеха закроем модальное и перезапросим данные, с первой страницы
      function afterAdd() {
          $scope.addModalHide();
          calcTab = false;
          getMainPage();
      };
      //Обновим текущую страницу
      function afterSave() {
          $scope.addModalHide();
          calcTab = false;
          getMainPage();
      };
      //Готовлю данные для отправки и вызову глобальную AddData
      $scope.modal.addButtonClick = function () {
          $rootScope.GetPage(constants.Pages.Add, $http, afterAdd, {
              pageId: pageId,
              name: $scope.modal.name,
              type: $scope.modal.roadType,
              datetime: $scope.modal.datetimeNow,
              coordinates: $scope.modal.coordinates,
              distance: CalcDistanceFromTxt($scope.modal.coordinates)
          });
      };
      //Готовлю модальную для редактирования
      $scope.modal.editButtonClick = function (id) {
          $scope.editedIndex = id;
          $scope.modal.title = $scope.translation.TitleEdit;

          if ($scope.currentTab === 1) {
              $scope.modal.name = $scope.data.DataFoot[id].Name;
              $scope.modal.datetimeNow = $scope.data.DataFoot[id].DateFullText;
              $scope.modal.coordinates = $scope.data.DataFoot[id].Coordinates;
          } else if ($scope.currentTab === 2) {
              $scope.modal.name = $scope.data.DataCar[id].Name;
              $scope.modal.datetimeNow = $scope.data.DataCar[id].DateFullText;
              $scope.modal.coordinates = $scope.data.DataCar[id].Coordinates;
          } else {
              $scope.modal.name = $scope.data.DataBike[id].Name;
              $scope.modal.datetimeNow = $scope.data.DataBike[id].DateFullText;
              $scope.modal.coordinates = $scope.data.DataBike[id].Coordinates;
          }
          //if (parseInt($scope.modal.roadType) !== parseInt(type)) $scope.modal.roadType = $scope.prepared.TypeList[parseInt(type)].Value;;
          if (parseInt($scope.modal.roadType) !== parseInt($scope.currentTab)) $scope.modal.roadType = $scope.currentTab;

          $scope.modal.shareButton = true;
          $scope.modal.deleteButton = true;
          $scope.modal.saveButton = true;
          $scope.modal.addButton = false;

          $("#AddModalWindow").modal("show");
      };
      //Модальная хочет сохранить данные
      $scope.modal.saveButtonClick = function () {
          $rootScope.GetPage(constants.Pages.Update, $http, afterSave, {
              pageId: pageId,
              id: getId($scope.editedIndex),
              name: $scope.modal.name,
              type: $scope.modal.roadType,
              datetime: $scope.modal.datetimeNow,
              coordinates: $scope.modal.coordinates,
              distance: CalcDistanceFromTxt($scope.modal.coordinates)
          });
      };
      //Модальная хочет удалить данные
      $scope.modal.deleteButtonClick = function () {
          $rootScope.GetPage(constants.Pages.Delete, $http, afterSave, { pageId: pageId, recordId: getId($scope.editedIndex) });
      };
      ///////////////////////////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////           МОДАЛЬНАЯ ДОСТУПА
      ///////////////////////////////////////////////////////////////////////
      //Изменения после запросов с модальной доступа
      function getShareCallBack(link) {
          $scope.modalShare.loading = false;
          if (link === '-') { //нет ссылки, показать кнопку - Получить
              $scope.modalShare.addButton = true;
              $scope.modalShare.link = '';
          } else { //есть ссылка, заполним, показать кнопку - Попробовать + Удалить
              $scope.modalShare.tryButton = true;
              $scope.modalShare.deleteButton = true;
              $scope.modalShare.link = link;
          }
          calcTab = false;
          getMainPage();
      };
      //Вызов модальной, он может быть из модальнйо редактирования, тогда заполнен $scope.editedIndex (откуда вызвал проверяю отдельно на 0, ибо if(0) это false)
      $scope.modalShare.shareButtonClick = function (id) {
          if (!id && id !== 0) id = $scope.editedIndex;

          $scope.editedIndex = id;
          $scope.modalShare.loading = true;

          $scope.modalShare.addButton = false;
          $scope.modalShare.tryButton = false;
          $scope.modalShare.deleteButton = false;

          $rootScope.GetPage(constants.Pages.GetShare, $http, getShareCallBack, { pageId: pageId, recordId: getId($scope.editedIndex) });

          $("#ShareModalWindow").modal("show");
      };
      //Нажата кнопка попробовать в модальной доступа, откроем на соседней вкладке ссылку
      $scope.modalShare.tryButtonClick = function () {
          window.open($scope.modalShare.link, '_blank');
      };
      //Удаляем доступ к текущей записи из модальной доступа
      $scope.modalShare.deleteButtonClick = function () {
          $scope.modalShare.loading = true;

          $scope.modalShare.addButton = false;
          $scope.modalShare.tryButton = false;
          $scope.modalShare.deleteButton = false;

          $rootScope.GetPage(constants.Pages.DeleteShare, $http, getShareCallBack, { pageId: pageId, recordId: getId($scope.editedIndex) });
      };
      //Добавляем доступ из модальной доступа
      $scope.modalShare.addButtonClick = function () {
          $scope.modalShare.loading = true;

          $scope.modalShare.addButton = false;
          $scope.modalShare.tryButton = false;
          $scope.modalShare.deleteButton = false;

          $rootScope.GetPage(constants.Pages.GenerateShare, $http, getShareCallBack, { pageId: pageId, recordId: getId($scope.editedIndex) });
      };
  }]);
