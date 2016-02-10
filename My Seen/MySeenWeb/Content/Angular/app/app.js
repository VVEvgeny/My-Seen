'use strict';

/* App Module */
var routerApp = angular.module('MySeenApp', ['ui.router', 'ui.bootstrap', 'FilmsController', 'SerialsController']);

routerApp.config(function($stateProvider, $urlRouterProvider) {

    $stateProvider
        // главная, состояние и вид ========================================
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
        // о нас =================================
        .state('about', {
            // we'll get to this in a bit
        
        });
    $urlRouterProvider.otherwise('/');
}).run(function($rootScope) {
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////           Работа с сервером
    ///////////////////////////////////////////////////////////////////////
    $rootScope.GetPage = function(pageName, $http, callback, parameters) {
        $rootScope.loading = true;
        $http.post(pageName, parameters).success(function(jsonData) {
            $rootScope.loading = false;
            if (jsonData.error) {
                alert(jsonData.error);
            } else {
                callback(jsonData); //Отдадим на выполнение
            }
        });
    };
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////           Закрыть модальные
    ///////////////////////////////////////////////////////////////////////
    $rootScope.closeModals = function() {
        //Модальная при смене страницы остается, чистим вручную...
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
    };
}).constant('Constants', {
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////           Константы / Перечесления
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

