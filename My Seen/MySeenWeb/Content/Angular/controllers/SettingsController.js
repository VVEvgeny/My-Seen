App.config(function($stateProvider) {

    $stateProvider
        .state("settings",
        {
            url: "/settings/",
            templateUrl: "Content/Angular/templates/settings.html",
            controller: "SettingsController",
            reloadOnSearch: false
        });
});

App.controller("SettingsController",
[
    "$scope", "$rootScope", "$state", "$stateParams", "$http", "$location", "Constants", "$anchorScroll",
    function($scope, $rootScope, $state, $stateParams, $http, $location, constants, $anchorScroll) {

        $anchorScroll();
        $rootScope.loading = true;
        //Индекс страницы, для запросов к серверу
        $rootScope.pageId = constants.PageIds.Settings;

        //Перевод
        function fillTranslation(page) {
            $scope.translation = page;
            $scope.translation.loaded = true;
            if (!$scope.data) $rootScope.loading = true;
        }

        //Основные данные
        function fillScope(page) {
            $scope.data = page;
        };

        function getMainPage() {
            $rootScope.GetPage(constants.Pages.Main, $http, fillScope, { pageId: $rootScope.pageId });
        };

        $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
        getMainPage();


        function afterLanguageChange() {
            //window.location.href = window.location.href; //не юзает пост
            window.location.reload();
        };

        $scope.languageChange = function() {
            $scope.translation.loaded = false;
            $rootScope.GetPage(constants.PagesSettings.SetLanguage,
                $http,
                afterLanguageChange,
                { val: $scope.data.Lang });
        };
        $scope.themeChange = function() {
            $rootScope.GetPage(constants.PagesSettings
                .SetTheme,
                $http,
                afterLanguageChange,
                { val: $scope.data.Theme });
        };
        $scope.rppChange = function() {
            $rootScope.GetPage(constants.PagesSettings.SetRpp, $http, null, { val: $scope.data.Rpp });
        };
        $scope.markersChange = function() {
            $rootScope.GetPage(constants.PagesSettings.SetMor, $http, null, { val: $scope.data.Markers });
        };

        $scope.modalSetPassword = {};
        $scope.setPassword = function() {
            $scope.modalSetPassword.password = "";
            $scope.modalSetPassword.newPassword = "";
            $scope.modalSetPassword.passwordConfirm = "";

            $("#PasswordModalWindow").modal("show");
        };

        $scope.$on("$destroy",
            function() {
                $("#PasswordModalWindow").modal("hide");
                $("body").removeClass("modal-open");
                $(".modal-backdrop").remove();
            });

        function afterSetPassword() {
            $("#PasswordModalWindow").modal("hide");
            getMainPage();
        };

        $scope.modalSetPassword.addButtonClick = function() {
            $rootScope.GetPage(constants.PagesSettings.SetPassword,
                $http,
                afterSetPassword,
                {
                    password: $scope.modalSetPassword.password,
                    newPassword: $scope.modalSetPassword.newPassword
                }
            );
        };

        function afterGetLogins(page) {
            $scope.userLogins = page.UserLogins;
            $scope.otherLogins = page.OtherLogins;

            $("#LoginsModalWindow").modal("show");
        };

        function getLogins() {
            $rootScope.GetPage(constants.PagesSettings.GetLogins, $http, afterGetLogins, {});
        };

        $scope.manageExternals = function() {
            getLogins();
        };

        $scope.modalLogins = {};
        $scope.modalLogins.removeButtonClick = function(id) {
            $rootScope.GetPage(constants.PagesSettings.RemoveLogin,
                $http,
                getLogins,
                { loginProvider: $scope.userLogins[id].LoginProvider, providerKey: $scope.userLogins[id].ProviderKey });
        };
    }
]);