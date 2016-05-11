App.config(function($stateProvider) {

    $stateProvider
        .state("mymemory/roadsEditor",
        {
            url: "/mymemory/roads/editor/:id",
            templateUrl: "Content/Angular/templates/MyMemory/roadsEditor.html",
            controller: "RoadsEditorController",
            reloadOnSearch: false
        });
});

App.controller("RoadsEditorController",
[
    "$scope", "$rootScope", "$state", "$stateParams", "$http", "$location", "Constants", "$anchorScroll",
    function($scope, $rootScope, $state, $stateParams, $http, $location, constants, $anchorScroll) {

        $anchorScroll();
        $rootScope.loading = true;
        //Индекс страницы, для запросов к серверу
        $rootScope.pageId = constants.PageIds.Roads;
        //Показать ли кнопку ДОБАВИТЬ
        $scope.pageCanAdd = true;

        //Перевод всех данных на тек. странице
        $scope.translation = {};
        //Загрузка значений по умолчанию и списков
        $scope.prepared = {};
        //Модальная добавления/редактирования Указываем какие поля будем видеть
        $scope.modal = {
            showName: true,
            showWhen: true,
            showRoadTypes: true,
            showCoordinates: true
        };

        //Для модальной готовим данные
        function fillPrepared(page) {
            $scope.prepared = page;
            $scope.prepared.loaded = true;
            if ((!$scope.data && $stateParams.id) || !$scope.translation.loaded) $rootScope.loading = true;
        }

        //Перевод таблицы и модальной
        function fillTranslation(page) {
            $scope.translation = page;
            $scope.translation.loaded = true;
            if ((!$scope.data && $stateParams.id) || !$scope.prepared.loaded) $rootScope.loading = true;
        }

        //Основные данные
        function fillScope(page) {
            $scope.data = page;
            window.coordinates = 0;
            console.log($scope.data);
            if ($scope.data && $scope.data.DataFoot.length > 0) {
                $scope.data.DataFoot[0].Coordinates.split(";")
                    .forEach(function(item) {
                        if (item) {
                            window.current = {};
                            current.latLng = new window.google.maps.LatLng(item.split(",")[0], item.split(",")[1]);
                            addMarkerInEditor(true);
                        }
                    });
                calculateRoute();
                autoFit();
            } else {
                $state.go("mymemory/roads");
            }
        };

        //Сразу 3 запроса на сервер, далее будет только запросы по новым данным и на добавление/изменение
        $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: $rootScope.pageId });
        $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
        if ($stateParams.id) {
            $rootScope.GetPage(constants.Pages.Main,
                $http,
                fillScope,
                { pageId: $rootScope.pageId, road: $stateParams.id });
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           МОДАЛЬНАЯ ДОБАВЛЕНИЯ / РЕДАКТИРОВАНИЯ
        ///////////////////////////////////////////////////////////////////////
        //Прячу модальную Добавить/Редактировать
        //Готовлю данные для добавления новой записи и отображаю модальную
        $scope.addModalOpen = function() {

            if ($stateParams.id) { //как редактор
                $scope.modal.title = $scope.translation.TitleEdit;
                $scope.modal.name = $scope.data.DataFoot[0].Name;
                $scope.modal.datetimeNow = $scope.data.DataFoot[0].DateFullText;
                if ($scope.modal
                    .roadType !==
                    $scope.data.DataFoot[0].Type) $scope.modal.roadType = $scope.data.DataFoot[0].Type;

                $scope.modal.saveButton = true;
                $scope.modal.addButton = false;
            } else { //ка кдобавление
                $scope.modal.title = $scope.translation.TitleAdd;
                $scope.modal.name = "";
                $scope.modal.datetimeNow = $scope.prepared.DateTimeNow;
                if ($scope.modal
                    .roadType !==
                    $scope.prepared.TypeList[0].Value) $scope.modal.roadType = $scope.prepared.TypeList[0].Value;

                $scope.modal.saveButton = false;
                $scope.modal.addButton = true;
            }

            var $panel = $("#panelCoordinates");
            var $rows = $panel.find("div");
            $scope.modal.coordinates = "";
            $rows.each(function(index, element) {
                var $row = $(element);
                var id = $row.attr("id");
                //console.log("id=", id);
                id = id.replace("(", "").replace(")", "");
                $scope.modal.coordinates += id + ";";
            });

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

        //в случае успеха закроем модальное и перезапросим данные, с первой страницы
        function afterAdd() {
            $scope.addModalHide();
            $state.go("mymemory/roads");
        };

        //Готовлю данные для отправки и вызову глобальную AddData
        $scope.modal.addButtonClick = function() {
            $rootScope.GetPage(constants.Pages.Add,
                $http,
                afterAdd,
                {
                    pageId: $rootScope.pageId,
                    name: $scope.modal.name,
                    type: $scope.modal.roadType,
                    datetime: $scope.modal.datetimeNow,
                    coordinates: $scope.modal.coordinates,
                    distance: CalcDistanceFromTxt($scope.modal.coordinates)
                });
        };
        //Модальная хочет сохранить данные
        $scope.modal.saveButtonClick = function() {
            $rootScope.GetPage(constants.Pages.Update,
                $http,
                afterAdd,
                {
                    pageId: $rootScope.pageId,
                    id: $stateParams.id,
                    name: $scope.modal.name,
                    type: $scope.modal.roadType,
                    datetime: $scope.modal.datetimeNow,
                    coordinates: $scope.modal.coordinates,
                    distance: CalcDistanceFromTxt($scope.modal.coordinates)
                });
        };
    }
]);