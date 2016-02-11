'use strict';

/* Controllers */

var MySeenApp = angular.module('RoadsController', []);

MySeenApp.controller('RoadsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //Индекс страницы, для запросов к серверу
      var pageId = 3;
      //Показать ли кнопку ДОБАВИТЬ
      $scope.pageCanAdd = true;

      //На всякий случай закрою, может переход со страницы, где забыли закрыть модальную
      $rootScope.closeModals();

      //Перевод всех данных на тек. странице
      $scope.translation = {};
      //Загрузка значений по умолчанию и списков
      $scope.prepared = {};
      //Модальная добавления/редактирования Указываем какие поля будем видеть
      $scope.modal = {};
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
      function fillScope(page) {
          $scope.data = page;
      };
      function getMainPage() {
          $rootScope.GetPage(constants.Pages.Main, $http, fillScope, { pageId: pageId, page: ($stateParams ? $stateParams.page : null), search: ($stateParams ? $stateParams.search : null) });
      };

      //Сразу 3 запроса на сервер, далее будет только запросы по новым данным и на добавление/изменение
      $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: pageId });
      $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: pageId });
      getMainPage();

  }]);
