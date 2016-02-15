'use strict';

/* App Module */
var App = angular.module('MySeenApp', ['ui.router', 'ui.bootstrap']);

App.config(function ($stateProvider, $urlRouterProvider) {

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
    $rootScope.closeModals = function () {
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

