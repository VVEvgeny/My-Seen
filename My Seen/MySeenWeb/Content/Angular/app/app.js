'use strict';

/* App Module */
var App = angular.module('MySeenApp', ['ui.router', 'ui.bootstrap']);

App.config(function($stateProvider, $urlRouterProvider, $locationProvider) {

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
    $locationProvider.hashPrefix('!');

    $urlRouterProvider.otherwise('/');

}).run(function ($rootScope, $cacheFactory, Constants) {
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////         VARIABLES
    ///////////////////////////////////////////////////////////////////////
    $rootScope.$watch( function() { return window.GAuthorized; }, function() { $rootScope.authorized = window.GAuthorized; });
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////         CACHE
    ///////////////////////////////////////////////////////////////////////
    var cacheEnabled = true;
    $rootScope.keysTranslates = [];
    $rootScope.cacheTranslates = $cacheFactory('translates');
    $rootScope.putTranslates = function (key, value) {
        if (angular.isUndefined($rootScope.cacheTranslates.get(key))) {
            $rootScope.keysTranslates.push(key);
        }
        $rootScope.cacheTranslates.put(key, angular.isUndefined(value) ? null : value);
    };
    $rootScope.getTranslates = function (key) {
        if (angular.isUndefined($rootScope.cacheTranslates.get(key))) return null;
        return $rootScope.cacheTranslates.get(key);
    };
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////           Change observed
    ///////////////////////////////////////////////////////////////////////
    $rootScope.safeApply = function(fn) {
        var phase = this.$root.$$phase;
        if (phase == '$apply' || phase == '$digest') {
            if (fn && (typeof (fn) === 'function')) {
                fn();
            }
        } else {
            this.$apply(fn);
        }
    };
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////           REQUESTS
    ///////////////////////////////////////////////////////////////////////
    $rootScope.GetPage = function(pageName, $http, callback, parameters, silentMode) {
        if (!silentMode) $rootScope.loading = true;

        if (cacheEnabled) {
            if (pageName === Constants.PagesSettings.SetLanguage) {
                $rootScope.cacheTranslates.removeAll();
            }
            if (pageName === Constants.Pages.Translation) {
                if ($rootScope.getTranslates(parameters.pageId) != null) {
                    if (!silentMode) $rootScope.loading = false;
                    callback($rootScope.getTranslates(parameters.pageId));
                    return;
                }
            }
        }

        $http.post(pageName, parameters).success(function(jsonData) {

            if (!silentMode) $rootScope.loading = false;
            if (jsonData.error) {
                if (!silentMode) alert(jsonData.error);
            } else {
                if (pageName === Constants.Pages.Translation) {
                    $rootScope.putTranslates(parameters.pageId, jsonData);
                }
                if (callback) callback(jsonData); //������� �� ����������
            }
        });
    };
}).constant('Constants', {
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////           Constants
    ///////////////////////////////////////////////////////////////////////
    PageIds:
    {
        Films: 0,
        Serials: 1,
        Books: 2,
        Roads: 3,
        Events: 4,
        Improvements: 103,
        About: 105,
        Settings: 106,
        Errors: 104,
        Logs: 102,
        Users: 101,
        Main: 200,
        Memes: 201,
        ChildSexCalculator: 202,
        TestWebGL: 900
    },
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
        EndImprovement: '/Json/EndImprovement/',
        RemoveAllError: '/Json/RemoveAllError/'
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
        RemoveLogin: '/Settings/RemoveLogin/',
    },
    PagesPortal:
    {
        RateMem: '/Portal/RateMem/'
    },
    PagesAdmin:
    {
        UpdateUser: '/Json/UpdateUser/'
    }
}).service('$log', function () {
    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////           Angular NLog
    ///////////////////////////////////////////////////////////////////////
    this.log = function(msg) {
        JL('Angular').trace(msg);
    }
    this.debug = function(msg) {
        JL('Angular').debug(msg);
    }
    this.info = function(msg) {
        JL('Angular').info(msg);
    }
    this.warn = function(msg) {
        JL('Angular').warn(msg);
    }
    this.error = function(msg) {
        JL('Angular').error(msg);
    }
}).factory('$exceptionHandler', function() {
    return function(exception, cause) {
        JL('Angular').fatalException(cause, exception);
        throw exception;
    };
});

