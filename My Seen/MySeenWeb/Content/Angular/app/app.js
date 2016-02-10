'use strict';

/* App Module */
var routerApp = angular.module('MySeenApp', ['ui.router', 'ui.bootstrap', 'FilmsController', 'SerialsController']);

routerApp.config(function($stateProvider, $urlRouterProvider) {

    $stateProvider
        // �������, ��������� � ��� ========================================
        .state('films', {
            url: '/films/?:page&search',
            templateUrl: "Content/Angular/templates/films.html",
            controller: 'FilmsController',
            reloadOnSearch: false
        })
        .state('serials', {
            url: '/serials/?:page&search',
            templateUrl: "Content/Angular/templates/serials.html",
            controller: 'SerialsController',
            reloadOnSearch: false
        })
        // � ��� =================================
        .state('about', {
            // we'll get to this in a bit
        
        });
    $urlRouterProvider.otherwise('/');
}).run(function($rootScope) {
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////           ������ � ��������
    ///////////////////////////////////////////////////////////////////////
    $rootScope.GetPage = function(pageName, $http, callback, parameters) {
        $rootScope.loading = true;
        $http.post(pageName, parameters).success(function(jsonData) {
            $rootScope.loading = false;
            if (jsonData.error) {
                alert(jsonData.error);
            } else {
                callback(jsonData); //������� �� ����������
            }
        });
    };
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////           ������� ���������
    ///////////////////////////////////////////////////////////////////////
    $rootScope.closeModals = function() {
        //��������� ��� ����� �������� ��������, ������ �������...
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
    };
}).constant('Constants', {
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////           ��������� / ������������
    ///////////////////////////////////////////////////////////////////////
    Pages: {
        Main: '/Json/GetPage/',
        Add: '/Json/AddData/',
        Update: '/Json/UpdateData/',
        Prepared: '/Json/GetPrepared/',
        Translation: '/Json/GetTranslation/',
        Delete: '/Json/DeleteData/',
        GetShare: '/Json/GetShare/',
        DeleteShare: '/Json/DeleteShare/',
        GenerateShare: '/Json/GenerateShare/'
    }
});

