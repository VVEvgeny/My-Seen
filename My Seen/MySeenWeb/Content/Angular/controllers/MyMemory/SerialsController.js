App.config(function($stateProvider) {

    $stateProvider
        .state('mymemory/serials',
        {
            url: '/mymemory/serials/?:page&search',
            templateUrl: "Content/Angular/templates/MyMemory/serials.html",
            controller: 'SerialsController',
            reloadOnSearch: false
        });
});

App.controller('SerialsController',
[
    '$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants', '$anchorScroll',
    function($scope, $rootScope, $state, $stateParams, $http, $location, constants, $anchorScroll) {

        $anchorScroll();
        $rootScope.loading = true;
        //Индекс страницы, для запросов к серверу
        $rootScope.pageId = constants.PageIds.Serials;
        //Показать ли кнопку ДОБАВИТЬ
        $scope.pageCanAdd = true;
        //Показать ли поле ПОИСКа
        $scope.pageCanSearch = true;

        //Перевод всех данных на тек. странице
        $scope.translation = {};
        //Загрузка значений по умолчанию и списков
        $scope.prepared = {};
        //Модальная добавления/редактирования Указываем какие поля будем видеть
        $scope.modal = {
            showName: true,
            showYear: true,
            showSeason: true,
            showSeries: true,
            showWhen: true,
            showGenre: true,
            showRating: true
        };
        //Модальная доступа
        $scope.modalShare = {};

        //Для модальной готовим данные
        function fillPrepared(page) {
            $scope.prepared = page;
            $scope.prepared.loaded = true;
            if (!$scope.data || !$scope.translation.loaded) $rootScope.loading = true;
        }

        //Перевод таблицы и модальной
        function fillTranslation(page) {
            $scope.translation = page;
            $scope.translation.loaded = true;
            if (!$scope.data || !$scope.prepared.loaded) $rootScope.loading = true;
        }

        //Основные данные
        function fillScope(page) {
            $scope.data = page.Data;
            $scope.pages = page.Pages;
        };

        function getMainPage() {
            $rootScope.GetPage(constants.Pages.Main,
                $http,
                fillScope,
                {
                    pageId: $rootScope.pageId,
                    page: ($stateParams ? $stateParams.page : null),
                    search: ($stateParams ? $stateParams.search : null)
                });
        };

        //Сразу 3 запроса на сервер, далее будет только запросы по новым данным и на добавление/изменение
        $rootScope.GetPage(constants.Pages.Prepared, $http, fillPrepared, { pageId: $rootScope.pageId });
        $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
        getMainPage();

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           ПОИСК
        ///////////////////////////////////////////////////////////////////////
        $scope.quickSearch = {};
        $scope.quickSearch.text = $stateParams ? $stateParams.search : null;
        $scope.searchButtonClick = function() {
            $location.search('search', $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null);
            $location.search('page', null); //с первой страницы новый поиск
            if ($stateParams) $stateParams.page = null;
            if ($stateParams) $stateParams.search = $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null;
            getMainPage();
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           ПАГИНАЦИЯ
        ///////////////////////////////////////////////////////////////////////
        //Не использую перехода по состояниям, они перезагружают контроллер, а так у меня в настройках для контролера стоит reloadOnSearch: false      
        $scope.pagination = {};
        $scope.pagination.goToPage = function(page) {
            $location.search('page', page > 1 ? page : null);
            if ($stateParams) $stateParams.page = page > 1 ? page : null;
            getMainPage();
            $anchorScroll();
        }
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           МОДАЛЬНАЯ ДОБАВЛЕНИЯ / РЕДАКТИРОВАНИЯ
        ///////////////////////////////////////////////////////////////////////
        //Прячу модальную Добавить/Редактировать
        //Готовлю данные для добавления новой записи и отображаю модальную
        $scope.addModalOpen = function() {
            $scope.modal.title = $scope.translation.TitleAdd;
            $scope.modal.name = '';
            $scope.modal.year = $scope.prepared.Year;
            $scope.modal.season = '';
            $scope.modal.series = '';
            $scope.modal.datetimeNow = $scope.prepared.DateTimeNow;
            if ($scope.modal
                .genre !==
                $scope.prepared.GenreList[0].Value) $scope.modal.genre = $scope.prepared.GenreList[0].Value;
            if ($scope.modal
                .rating !==
                $scope.prepared.RatingList[0].Value) $scope.modal.rating = $scope.prepared.RatingList[0].Value;

            $scope.modal.addButton = true;
            $scope.modal.shareButton = false;
            $scope.modal.deleteButton = false;
            $scope.modal.saveButton = false;

            $("#AddModalWindow").modal("show");
        };
        $scope.$on("$destroy",
            function() {
                $scope.addModalHide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            });
        $scope.addModalHide = function() {
            $("#AddModalWindow").modal("hide");
        };

        //в случае успеха закроем модальное и перезапросим данные, с первой страницы
        function afterAdd() {
            $scope.addModalHide();
            $location.search('page', null); //с первой страницы новый поиск
            $location.search('search', null);
            if ($stateParams) {
                $stateParams.page = null;
                $stateParams.search = null;
            }
            $scope.quickSearch.text = null;
            getMainPage();
        };

        //Обновим текущую страницу
        function afterSave() {
            $scope.addModalHide();
            getMainPage();
        };

        //Готовлю данные для отправки и вызову глобальную AddData
        $scope.modal.addButtonClick = function() {
            $rootScope.GetPage(constants.Pages.Add,
                $http,
                afterAdd,
                {
                    pageId: $rootScope.pageId,
                    name: $scope.modal.name,
                    year: $scope.modal.year,
                    season: $scope.modal.season,
                    series: $scope.modal.series,
                    datetime: $scope.modal.datetimeNow,
                    genre: $scope.modal.genre,
                    rating: $scope.modal.rating
                });
        };
        //Готовлю модальную для редактирования
        $scope.modal.editButtonClick = function(id) {
            $scope.editedIndex = id;
            $scope.modal.title = $scope.translation.TitleEdit;
            $scope.modal.name = $scope.data[id].Name;
            $scope.modal.year = $scope.data[id].Year === 0 ? null : $scope.data[id].Year;
            $scope.modal.season = $scope.data[id].LastSeason;
            $scope.modal.series = $scope.data[id].LastSeries;
            $scope.modal.datetimeNow = $scope.data[id].DateBeginText;
            if ($scope.modal.genre !== $scope.data[id].GenreVal) $scope.modal.genre = $scope.data[id].GenreVal;
            if ($scope.modal.rating !== $scope.data[id].RatingVal) $scope.modal.rating = $scope.data[id].RatingVal;

            $scope.modal.shareButton = true;
            $scope.modal.deleteButton = true;
            $scope.modal.saveButton = true;
            $scope.modal.addButton = false;

            $("#AddModalWindow").modal("show");
        };
        //Модальная хочет сохранить данные
        $scope.modal.saveButtonClick = function() {
            $rootScope.GetPage(constants.Pages.Update,
                $http,
                afterSave,
                {
                    pageId: $rootScope.pageId,
                    id: $scope.data[$scope.editedIndex].Id,
                    name: $scope.modal.name,
                    year: $scope.modal.year,
                    season: $scope.modal.season,
                    series: $scope.modal.series,
                    datetime: $scope.modal.datetimeNow,
                    genre: $scope.modal.genre,
                    rating: $scope.modal.rating
                });
        };
        //Модальная хочет удалить данные
        $scope.modal.deleteButtonClick = function() {
            $rootScope.GetPage(constants.Pages.Delete,
                $http,
                afterSave,
                { pageId: $rootScope.pageId, recordId: $scope.data[$scope.editedIndex].Id });
        };

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           МОДАЛЬНАЯ ДОСТУПА
        ///////////////////////////////////////////////////////////////////////
        //Изменения после запросов с модальной доступа
        function getShareCallBack(link) {
            $scope.modalShare.loading = false;
            if (link === '-') { //нет ссылки, показать кнопку - Получить
                $scope.modalShare.addButton = true;
                $scope.modalShare.link = '';
            } else { //есть ссылка, заполним, показать кнопку - Попробовать + Удалить
                $scope.modalShare.tryButton = true;
                $scope.modalShare.deleteButton = true;
                $scope.modalShare.link = link;
            }
            getMainPage();
        };

        //Вызов модальной, он может быть из модальнйо редактирования, тогда заполнен $scope.editedIndex (откуда вызвал проверяю отдельно на 0, ибо if(0) это false)
        $scope.modalShare.shareButtonClick = function(id) {
            if (!id && id !== 0) id = $scope.editedIndex;

            $scope.editedIndex = id;
            $scope.modalShare.loading = true;

            $scope.modalShare.addButton = false;
            $scope.modalShare.tryButton = false;
            $scope.modalShare.deleteButton = false;

            $rootScope.GetPage(constants.Pages.GetShare,
                $http,
                getShareCallBack,
                { pageId: $rootScope.pageId, recordId: $scope.data[$scope.editedIndex].Id });

            $("#ShareModalWindow").modal("show");
        };
        //Нажата кнопка попробовать в модальной доступа, откроем на соседней вкладке ссылку
        $scope.modalShare.tryButtonClick = function() {
            //window.open($scope.modalShare.link, '_blank');
            $state.go('mymemory/sharedSerials',
            { "key": $scope.modalShare.link.split('/')[$scope.modalShare.link.split('/').length - 1] });
        };
        //Удаляем доступ к текущей записи из модальной доступа
        $scope.modalShare.deleteButtonClick = function() {
            $scope.modalShare.loading = true;

            $scope.modalShare.addButton = false;
            $scope.modalShare.tryButton = false;
            $scope.modalShare.deleteButton = false;

            $rootScope.GetPage(constants.Pages.DeleteShare,
                $http,
                getShareCallBack,
                { pageId: $rootScope.pageId, recordId: $scope.data[$scope.editedIndex].Id });
        };
        //Добавляем доступ из модальной доступа
        $scope.modalShare.addButtonClick = function() {
            $scope.modalShare.loading = true;

            $scope.modalShare.addButton = false;
            $scope.modalShare.tryButton = false;
            $scope.modalShare.deleteButton = false;

            $rootScope.GetPage(constants.Pages.GenerateShare,
                $http,
                getShareCallBack,
                { pageId: $rootScope.pageId, recordId: $scope.data[$scope.editedIndex].Id });
        };
    }
]);
