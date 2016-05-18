App.config(function($stateProvider) {

    $stateProvider
        .state("mymemory/sharedRoads",
        {
            url: "/mymemory/roads/shared/:key?search&year",
            templateUrl: "Content/Angular/templates/MyMemory/Shared/roads.html",
            controller: "SharedRoadsController",
            reloadOnSearch: false
        });
});

App.controller("SharedRoadsController",
[
    "$scope", "$rootScope", "$state", "$stateParams", "$http", "$location", "Constants", "$anchorScroll",
    function($scope, $rootScope, $state, $stateParams, $http, $location, constants, $anchorScroll) {

        if (!$stateParams.key) {
            $state.go("mymemory/roads");
        }
        $anchorScroll();
        $rootScope.loading = true;
        //Индекс страницы, для запросов к серверу
        $rootScope.pageId = constants.PageIds.Roads;
        //Показать ли поле ПОИСКа
        $scope.pageCanSearch = true;
        //Перевод всех данных на тек. странице
        $scope.translation = {};
        //Загрузка значений по умолчанию и списков
        $scope.prepared = {};

        //Перевод таблицы и модальной
        function fillTranslation(page) {
            $scope.translation = page;
            $scope.translation.loaded = true;
            if (!$scope.data) $rootScope.loading = true;
        }

        //Основные данные
        var calcTab = true;

        function fillScope(page) {
            $scope.data = page;
            if (calcTab) {
                if ($scope.data.DataFoot.length > 0) $scope.currentTab = 1;
                else if ($scope.data.DataBike.length > 0) $scope.currentTab = 3;
                else if ($scope.data.DataCar.length > 0) $scope.currentTab = 2;
            }
            calcTab = true;
            if ($scope.data.DataFoot.length > 0 ||
                $scope.data.DataBike.length > 0 ||
                $scope.data.DataCar.length > 0) $scope.showRoad(0);
        };

        function getMainPage() {
            //console.log($stateParams);
            $rootScope.GetPage(constants.Pages.Main,
                $http,
                fillScope,
                {
                    pageId: $rootScope.pageId,
                    shareKey: $stateParams.key,
                    year: $stateParams.year,
                    search: $stateParams.search
                });
        };

        //Сразу 3 запроса на сервер, далее будет только запросы по новым данным и на добавление/изменение
        $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
        getMainPage();

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
        ///////////////////////////////////////////////////////////////////////           Выбор года
        ///////////////////////////////////////////////////////////////////////
        $scope.year = $stateParams ? $stateParams.year ? $stateParams.year.toString() : "0" : "0";
        $scope.selectedChange = function() {
            $location.search("year", $scope.year === "0" ? null : $scope.year);
            if ($stateParams) {
                $stateParams.year = $scope.year === "0" ? null : $scope.year;
            }
            getMainPage();
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           Текущая вкладка
        ///////////////////////////////////////////////////////////////////////
        $scope.currentTab = 1;
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           КАРТА
        ///////////////////////////////////////////////////////////////////////
        $scope.showRoad = function(index) {
            //console.log("index=" + index + " type=" + type);
            clearMap();

            var array;
            if (index === 0) {
                if ($scope.currentTab === 1) {
                    array = $scope.data.DataFoot;
                } else if ($scope.currentTab === 2) {
                    array = $scope.data.DataCar;
                } else {
                    array = $scope.data.DataBike;
                }
            } else {
                if ($scope.currentTab === 1) {
                    array = $scope.data.DataFoot[index];
                } else if ($scope.currentTab === 2) {
                    array = $scope.data.DataCar[index];
                } else {
                    array = $scope.data.DataBike[index];
                }
            }
            if (!array.length) { //Если это не массив значит 1 элемент
                showRoad(array);
            } else {
                array.forEach(function(item) {
                    showRoad(item);
                });
            }

            autoFit();
        };

        //Обновим текущую страницу
        function afterSave() {
            $scope.addModalHide();
            calcTab = false;
            getMainPage();
        };

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           Id by index
        ///////////////////////////////////////////////////////////////////////
        function getId(index) {
            if ($scope.currentTab === 1) {
                return $scope.data.DataFoot[index].Id;
            } else if ($scope.currentTab === 2) {
                return $scope.data.DataCar[index].Id;
            }
            return $scope.data.DataBike[index].Id;
        };

        //Модальная хочет удалить данные
        $scope.modal = {};
        $scope.modal.deleteButtonClick = function(id) {
            $rootScope.GetPage(constants.Pages.Delete,
                $http,
                afterSave,
                { pageId: $rootScope.pageId, recordId: getId(id) });
        };
    }
]);