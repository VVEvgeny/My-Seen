'use strict';

/* Controllers */

var MySeenApp = angular.module('ImprovementsController', []);

MySeenApp.controller('ImprovementsController', ['$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants',
  function ($scope, $rootScope, $state, $stateParams, $http, $location, constants) {

      //�� ������ ������ ������, ����� ������� �� ��������, ��� ������ ������� ���������
      $rootScope.closeModals();

  }]);
