'use strict';

/* App Module */
var App = angular.module('MySeenApp', ['ui.router', 'ui.bootstrap']);

App.config(function ($stateProvider, $urlRouterProvider, $locationProvider) {

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
    $locationProvider.hashPrefix('!');

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
                if (callback) callback(jsonData); //Отдадим на выполнение
            }
        });
    };
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////           Закрыть модальные
    ///////////////////////////////////////////////////////////////////////
    $rootScope.clearControllers = function() {
        //Модальная при смене страницы остается, чистим вручную...
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        //Если был интервал, почистим его
        if ($rootScope.eventsInterval) clearInterval($rootScope.eventsInterval);
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
        GenerateShare: '/Json/GenerateShare/',
        EndImprovement: '/Json/EndImprovement/'
    },
    PagesSettings: {
        SetLanguage: '/Settings/SetLanguage/',
        SetRpp: '/Settings/SetRpp/',
        SetMor: '/Settings/SetMor/',
        SetVkService: '/Settings/SetVkService/',
        SetGoogleService: '/Settings/SetGoogleService/',
        SetFacebookService: '/Settings/SetFacebookService/',
        SetPassword: '/Settings/SetPassword/',
        GetLogins: '/Settings/GetLogins/',
        RemoveLogin: '/Settings/RemoveLogin/'
    }
});

