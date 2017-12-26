App.config(function($stateProvider) {

    $stateProvider
        .state("logs",
        {
            url: "/admin/logs/?bots:&period&page&search",
            templateUrl: "Content/Angular/templates/Admin/logs.html",
            controller: "LogsController",
            reloadOnSearch: false
        });
});

App.controller("LogsController",
[
    "$scope", "$rootScope", "$state", "$stateParams", "$http", "$location", "Constants", "$anchorScroll",
    function($scope, $rootScope, $state, $stateParams, $http, $location, constants, $anchorScroll) {

        $anchorScroll();
        $rootScope.loading = true;
        //������ ��������, ��� �������� � �������
        $rootScope.pageId = constants.PageIds.Logs;
        //�������� �� ������ ��������
        $scope.pageCanAdd = true;
        //�������� �� ���� ������
        $scope.pageCanSearch = true;
        //������� ���� ������ �� ���. ��������
        $scope.translation = {};
        //�������� �������� �� ��������� � �������
        $scope.prepared = {};

        //������� ������� � ���������
        function fillTranslation(page) {
            $scope.translation = page;
            $scope.translation.loaded = true;
        }

        //��� ��������� ������� ������
        function fillPrepared(page) {
            $scope.prepared = page;
            $scope.prepared.loaded = true;
            if (!$scope.data || !$scope.translation.loaded) $rootScope.loading = true;
        }

        //�������� ������
        function fillScope(page) {
            $scope.data = page.Data;
            $scope.pages = page.Pages;
            if (!$scope.data) $rootScope.loading = true;
        };

        function getMainPage() {
            $rootScope.GetPage(constants.Pages.Main,
                $http,
                fillScope,
                {
                    pageId: $rootScope.pageId,
                    page: $stateParams.page,
                    search: $stateParams.search,
                    bots: $stateParams.bots,
                    period: $stateParams.period
                });
        };

        $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
        $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: $rootScope.pageId });
        getMainPage();

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           Period
        ///////////////////////////////////////////////////////////////////////
        //console.log("period=" + $stateParams.period);
        var skipPeriod = false;
        if ($stateParams.period) {
            $scope.period = $stateParams.period;
            skipPeriod = true;
        }
        $scope.$watch(function() { return $scope.period; },
            function() {
                if ($scope.period && !skipPeriod) {
                    //console.log("new period=" + $scope.period);
                    $location.search("period", $scope.period !== "0" ? $scope.period : null);
                    if ($stateParams) $stateParams.period = $scope.period;
                    getMainPage();
                }
                skipPeriod = false;
            });

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           Search
        ///////////////////////////////////////////////////////////////////////
        $scope.quickSearch = {};
        $scope.quickSearch.text = $stateParams ? $stateParams.search : null;
        $scope.searchButtonClick = function() {
            $location.search("search", $scope.quickSearch.text !== "" ? $scope.quickSearch.text : null);
            $location.search("page", null); //� ������ �������� ����� �����
            if ($stateParams) $stateParams.page = null;
            if ($stateParams) $stateParams.search = $scope.quickSearch.text !== "" ? $scope.quickSearch.text : null;
            getMainPage();
        };

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           ���������
        ///////////////////////////////////////////////////////////////////////
        //�� ��������� �������� �� ����������, ��� ������������� ����������, � ��� � ���� � ���������� ��� ���������� ����� reloadOnSearch: false      
        $scope.pagination = {};
        $scope.pagination.goToPage = function(page) {
            $location.search("page", page > 1 ? page : null);
            if ($stateParams) $stateParams.page = page > 1 ? page : null;
            getMainPage();
            $anchorScroll();
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           COLLAPSE
        ///////////////////////////////////////////////////////////////////////
        $scope.showPost = function(id) {
            if (id === "all") {
                if ($("#postPlus_" + id).hasClass("hidden")) {

                    $("#postMinus_" + id).addClass("hidden");
                    $("#postPlus_" + id).removeClass("hidden");

                    if ($scope.data.length > 0) {
                        for (var i = 0; i < $scope.data.length; i++) {
                            $("#collapseme_" + i).addClass("out");
                            $("#collapseme_" + i).removeClass("in");

                            $("#postMinus_" + i).addClass("hidden");
                            $("#postPlus_" + i).removeClass("hidden");
                        }
                    }

                } else {
                    $("#postPlus_" + id).addClass("hidden");
                    $("#postMinus_" + id).removeClass("hidden");

                    if ($scope.data.length > 0) {
                        for (var i = 0; i < $scope.data.length; i++) {
                            $("#collapseme_" + i).addClass("in");
                            $("#collapseme_" + i).removeClass("out");

                            $("#postPlus_" + i).addClass("hidden");
                            $("#postMinus_" + i).removeClass("hidden");
                        }
                    }
                }
            } else {
                if ($("#postPlus_" + id).hasClass("hidden")) {

                    $("#collapseme_" + id).addClass("out");
                    $("#collapseme_" + id).removeClass("in");

                    $("#postMinus_" + id).addClass("hidden");
                    $("#postPlus_" + id).removeClass("hidden");
                } else {
                    $("#collapseme_" + id).addClass("in");
                    $("#collapseme_" + id).removeClass("out");

                    $("#postPlus_" + id).addClass("hidden");
                    $("#postMinus_" + id).removeClass("hidden");
                }
            }
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           ��������� ���������� / ��������������
        ///////////////////////////////////////////////////////////////////////
        //��������� ����������/�������������� ��������� ����� ���� ����� ������
        $scope.modal = {
            showName: true,
            showBotString: true,
            showLanguageType: true
        };
        $scope.modal.title = $scope.translation.TitleAdd;
        //����� ��������� ��������/�������������
        //������� ������ ��� ���������� ����� ������ � ��������� ���������
        $scope.addModalOpen = function () {
            $scope.modal.title = $scope.translation.TitleAdd;
            $scope.modal.name = "";
            $scope.modal.botString = "";
            if ($scope.modal
                .languageType !==
                $scope.prepared.LanguagesList[0].Value) $scope.modal.languageType = $scope.prepared.LanguagesList[0].Value;

            $scope.modal.addButton = true;

            $("#AddModalWindow").modal("show");
        };
        $scope.$on("$destroy",
            function () {
                $scope.addModalHide();
                $("body").removeClass("modal-open");
                $(".modal-backdrop").remove();
            });
        $scope.addModalHide = function () {
            $("#AddModalWindow").modal("hide");
        };

        //� ������ ������ ������� ��������� � ������������ ������, � ������ ��������
        function afterAdd() {
            $scope.addModalHide();
            $location.search("page", null); //� ������ �������� ����� �����
            $location.search("search", null);
            if ($stateParams) {
                $stateParams.page = null;
                $stateParams.search = null;
            }
            $scope.quickSearch.text = null;
            getMainPage();
        };

        //������� ������ ��� �������� � ������ ���������� AddData
        $scope.modal.addButtonClick = function () {
            $rootScope.GetPage(constants.Pages.Add,
                $http,
                afterAdd,
                {
                    pageId: $rootScope.pageId,
                    name: $scope.modal.name,
                    botString: $scope.modal.botString,
                    languageType: $scope.modal.languageType
                });
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           Settings
        ///////////////////////////////////////////////////////////////////////
        $scope.isFilters = false;
        $scope.showFilters = function() {
            if ($("#filters").hasClass("out")) {
                $("#filters").addClass("in");
                $("#filters").removeClass("out");
                $scope.isFilters = true;
            } else {
                $("#filters").addClass("out");
                $("#filters").removeClass("in");
                $scope.isFilters = false;
            }
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           Bots
        ///////////////////////////////////////////////////////////////////////
        $scope.bots = $stateParams.bots === "true";
        $scope.botsChange = function() {
            $location.search("page", null); //� ������ �������� ����� �����
            $location.search("bots", !$scope.bots ? null : $scope.bots);
            if ($stateParams) $stateParams.page = null;
            if ($stateParams) $stateParams.bots = !$scope.bots ? null : $scope.bots;
            getMainPage();
        };
        if ($scope.bots) $scope.showFilters();
    }
]);