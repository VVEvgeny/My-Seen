App.config(function($stateProvider) {

    $stateProvider
        .state("portal/memes",
        {
            url: "/portal/memes/:id?:page&search",
            templateUrl: "Content/Angular/templates/portal/memes.html",
            controller: "MemesController",
            reloadOnSearch: false
        });
});

App.controller("MemesController",
[
    "$scope", "$rootScope", "$state", "$stateParams", "$http", "$location", "Constants", "$anchorScroll",
    function($scope, $rootScope, $state, $stateParams, $http, $location, constants, $anchorScroll) {

        $anchorScroll();
        $rootScope.pageId = constants.PageIds.Memes;
        $scope.isOne = $stateParams.id;

        //Показать ли кнопку ДОБАВИТЬ
        if (!$scope.isOne) $scope.pageCanAdd = $rootScope.authorized;
        $scope.$watch(function() { return $scope.pageCanAdd; },
            function() { $scope.pageCanAdd = $rootScope.authorized; });

        //Показать ли поле ПОИСКа
        if (!$scope.isOne) $scope.pageCanSearch = true;
        //Перевод всех данных на тек. странице
        $scope.translation = {};

        //Перевод таблицы и модальной
        function fillTranslation(page) {
            $scope.translation = page;
            $scope.translation.loaded = true;
        }

        $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });

        //Основные данные
        function fillScope(page) {
            $scope.data = page.Data;
            $scope.pages = page.Pages;
            //fillExternalServices();
        };

        function getMainPage() {
            $rootScope.GetPage(constants.Pages.Main,
                $http,
                fillScope,
                {
                    Id: ($stateParams ? $stateParams.id : null),
                    pageId: $rootScope.pageId,
                    page: ($stateParams ? $stateParams.page : null),
                    search: ($stateParams ? $stateParams.search : null)
                });
        };

        getMainPage();

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           ПАГИНАЦИЯ
        ///////////////////////////////////////////////////////////////////////
        //Не использую перехода по состояниям, они перезагружают контроллер, а так у меня в настройках для контролера стоит reloadOnSearch: false      
        $scope.pagination = {};
        $scope.pagination.goToPage = function(page) {
            $location.search("page", page > 1 ? page : null);
            if ($stateParams) $stateParams.page = page > 1 ? page : null;
            getMainPage();
            $anchorScroll();
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           External Services
        ///////////////////////////////////////////////////////////////////////
        window.VK.init({ apiId: 5091515, onlyWidgets: true });
        if ($scope.isOne) {
            //http://vk.com/dev/widget_comments
            window.VK.Widgets.Comments("vk_comments", { limit: 10, attach: "*", autoPublish: 0 }, $stateParams.id);
        }

        function fillExternalServices() {
            if ($scope.data.length > 0) {
                for (var i = 0; i < $scope.data.length; i++) {
                    window.VK.Widgets.Like("vk_like_" + $scope.data[i].Id,
                    {
                        type: "button",
                        pageUrl: "http://myseen.by/portal/memes/" + $scope.data[i].Id,
                        pageTitle: $scope.data[i].Name,
                        pageImage: $scope.data[i].Image
                    });
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           ПОИСК
        ///////////////////////////////////////////////////////////////////////
        $scope.quickSearch = {};
        $scope.quickSearch.text = $stateParams ? $stateParams.search : null;
        $scope.searchButtonClick = function() {
            $location.search("search", $scope.quickSearch.text !== "" ? $scope.quickSearch.text : null);
            $location.search("page", null); //с первой страницы новый поиск
            if ($stateParams) $stateParams.page = null;
            if ($stateParams) $stateParams.search = $scope.quickSearch.text !== "" ? $scope.quickSearch.text : null;
            getMainPage();
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           MODAL ADD
        ///////////////////////////////////////////////////////////////////////
        $scope.modal = {};
        $scope.addModalOpen = function() {
            $scope.modal.name = "";
            $scope.modal.link = "";
            $("#AddModalWindow").modal("show");
        };
        $scope.$on("$destroy",
            function() {
                $scope.addModalHide();
                $("body").removeClass("modal-open");
                $(".modal-backdrop").remove();
            });
        $scope.addModalHide = function() {
            $("#AddModalWindow").modal("hide");
        };

        function afterAdd() {
            $scope.addModalHide();
            $location.search("page", null); //с первой страницы новый поиск
            $location.search("search", null);
            if ($stateParams) {
                $stateParams.page = null;
                $stateParams.search = null;
            }
            $scope.quickSearch.text = null;
            getMainPage();
        };

        $scope.modal.addButtonClick = function() {
            $rootScope.GetPage(constants.Pages.Add,
                $http,
                afterAdd,
                {
                    pageId: $rootScope.pageId,
                    name: $scope.modal.name,
                    link: $scope.modal.link
                });
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           FUNCTIONS
        ///////////////////////////////////////////////////////////////////////
        $scope.showPost = function(id) {
            if ($("#collapseme_" + id).hasClass("out")) {

                $("#collapseme_" + id).addClass("in");
                $("#collapseme_" + id).removeClass("out");

                $("#postPlus_" + id).addClass("hidden");
                $("#postMinus_" + id).removeClass("hidden");

            } else {
                $("#collapseme_" + id).addClass("out");
                $("#collapseme_" + id).removeClass("in");

                $("#postMinus_" + id).addClass("hidden");
                $("#postPlus_" + id).removeClass("hidden");
            }
        };
        $scope.plusPost = function(index) {
            $rootScope.safeApply(function() {
                // every changes goes here
                if ($scope.data.length > 0) {
                    if ($scope.data[index].Stats.Plus)
                        $scope.data[index].Stats.Plus = $scope.data[index].Stats.Plus + 1;
                    else $scope.data[index].Stats.Plus = 1;
                    $scope.data[index].Stats.Select = true;

                    $rootScope.GetPage(constants.PagesPortal.RateMem,
                        $http,
                        null,
                        { recordId: $scope.data[index].Id, plus: true },
                        true);
                }
            });
        };
        $scope.minusPost = function(index) {
            $rootScope.safeApply(function() {
                // every changes goes here
                if ($scope.data.length > 0) {
                    if ($scope.data[index].Stats.Minus)
                        $scope.data[index].Stats.Minus = $scope.data[index].Stats.Minus + 1;
                    else $scope.data[index].Stats.Minus = 1;
                    $scope.data[index].Stats.Select = true;

                    $rootScope.GetPage(constants.PagesPortal.RateMem,
                        $http,
                        null,
                        { recordId: $scope.data[index].Id, plus: false },
                        true);
                }
            });
        };
        $scope.removePost = function(index) {
            $rootScope.safeApply(function() {
                if ($scope.data.length > 0) {
                    $rootScope.GetPage(constants.Pages.Delete,
                        $http,
                        null,
                        { pageId: $rootScope.pageId, recordId: $scope.data[index].Id },
                        true);
                    $scope.data.splice(index, 1);
                }
            });
        };
    }
]);