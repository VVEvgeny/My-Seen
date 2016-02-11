'use strict';

/* App Module */
var routerApp = angular.module('MySeenApp', ['ui.router', 'ui.bootstrap'
    , 'FilmsController', 'SerialsController', 'BooksController', 'EventsController', 'HomeController', 'AboutController', 'ImprovementsController'
    , 'UsersController', 'LogsController', 'ErrorsController', 'SettingsController', 'RoadsController'
]);

routerApp.config(function($stateProvider, $urlRouterProvider) {

    $stateProvider
        .state('/', {
            url: '/',
            templateUrl: "Content/Angular/templates/home.html",
            controller: 'HomeController',
            reloadOnSearch: false
        })
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
        .state('books', {
            url: '/books/?:page&search',
            templateUrl: "Content/Angular/templates/books.html",
            controller: 'BooksController',
            reloadOnSearch: false
        })
        .state('events', {
            url: '/events/?:page&search&ended',
            templateUrl: "Content/Angular/templates/events.html",
            controller: 'EventsController',
            reloadOnSearch: false
        })
        .state('settings', {
            url: '/settings/',
            templateUrl: "Content/Angular/templates/settings.html",
            controller: 'FilmsController',
            reloadOnSearch: false
        })
        .state('improvements', {
            url: '/improvements/',
            templateUrl: "Content/Angular/templates/improvements.html",
            controller: 'ImprovementsController',
            reloadOnSearch: false
        })
        .state('users', {
            url: '/users/?:page&search',
            templateUrl: "Content/Angular/templates/users.html",
            controller: 'UsersController',
            reloadOnSearch: false
        })
        .state('logs', {
            url: '/logs/?:page&search',
            templateUrl: "Content/Angular/templates/logs.html",
            controller: 'LogsController',
            reloadOnSearch: false
        })
        .state('errors', {
            url: '/errors/?:page&search',
            templateUrl: "Content/Angular/templates/errors.html",
            controller: 'ErrorsController',
            reloadOnSearch: false
        })
        .state('roads', {
            url: '/roads/',
            templateUrl: "Content/Angular/templates/roads.html",
            controller: 'RoadsController',
            reloadOnSearch: false
        })
        .state('about', {
            url: '/about/',
            templateUrl: "Content/Angular/templates/about.html",
            controller: 'AboutController',
            reloadOnSearch: false
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

