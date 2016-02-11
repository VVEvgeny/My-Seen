'use strict';

/* Controllers */

var MySeenApp = angular.module('AboutController', []);

MySeenApp.controller('AboutController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //На всякий случай закрою, может переход со страницы, где забыли закрыть модальную
      $rootScope.closeModals();

  }]);
