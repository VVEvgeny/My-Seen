'use strict';

/* Controllers */

var MySeenApp = angular.module('SettingsController', []);

MySeenApp.controller('SettingsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.closeModals();

  }]);
