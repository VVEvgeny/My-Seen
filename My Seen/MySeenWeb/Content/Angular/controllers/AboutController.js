'use strict';

/* Controllers */

var MySeenApp = angular.module('AboutController', []);

MySeenApp.controller('AboutController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.closeModals();

  }]);
