App.config(function($stateProvider) {

    $stateProvider
        .state('mymemory/sharedSerials',
        {
            url: '/mymemory/serials/shared/:key?page&search',
            templateUrl: "Content/Angular/templates/MyMemory/Shared/serials.html",
            controller: 'SharedSerialsController',
            reloadOnSearch: false
        });
});

App.controller('SharedSerialsController',
[
    '$scope', '$rootScope', '$state', '$stateParams', '$http', '$location', 'Constants', '$anchorScroll',
    function($scope, $rootScope, $state, $stateParams, $http, $location, constants, $anchorScroll) {

        if (!$stateParams.key) {
            $state.go('mymemory/serials');
        }
        $anchorScroll();
        $rootScope.loading = true;
        //������ ��������, ��� �������� � �������
        $rootScope.pageId = constants.PageIds.Serials;
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
            if (!$scope.data) $rootScope.loading = true;
        }

        //�������� ������
        function fillScope(page) {
            $scope.data = page.Data;
            $scope.isMyData = page.IsMyData;
            $scope.pages = page.Pages;
        };

        function getMainPage() {
            $rootScope.GetPage(constants.Pages.Main,
                $http,
                fillScope,
                {
                    pageId: $rootScope.pageId,
                    shareKey: $stateParams.key,
                    page: $stateParams.page,
                    search: $stateParams.search
                });
        };

        //console.log($stateParams);
        //console.log($stateParams.key);

        //����� 3 ������� �� ������, ����� ����� ������ ������� �� ����� ������ � �� ����������/���������
        $rootScope.GetPage(constants.Pages.Translation, $http, fillTranslation, { pageId: $rootScope.pageId });
        getMainPage();

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           �����
        ///////////////////////////////////////////////////////////////////////
        $scope.quickSearch = {};
        $scope.quickSearch.text = $stateParams ? $stateParams.search : null;
        $scope.searchButtonClick = function() {
            $location.search('search', $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null);
            $location.search('page', null); //� ������ �������� ����� �����
            if ($stateParams) $stateParams.page = null;
            if ($stateParams) $stateParams.search = $scope.quickSearch.text !== '' ? $scope.quickSearch.text : null;
            getMainPage();
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           ���������
        ///////////////////////////////////////////////////////////////////////
        //�� ��������� �������� �� ����������, ��� ������������� ����������, � ��� � ���� � ���������� ��� ���������� ����� reloadOnSearch: false      
        $scope.pagination = {};
        $scope.pagination.goToPage = function(page) {
            $location.search('page', page > 1 ? page : null);
            if ($stateParams) $stateParams.page = page > 1 ? page : null;
            getMainPage();
            $anchorScroll();
        };
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////           ��������
        ///////////////////////////////////////////////////////////////////////
        $scope.deleteShareButtonClick = function(id) {
            $rootScope.GetPage(constants.Pages.DeleteShare,
                $http,
                getMainPage,
                { pageId: $rootScope.pageId, recordId: $scope.data[id].Id });
        };
    }
]);
