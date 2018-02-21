app.controller('queryCtrl', function ($scope, $http) {
    $scope.pageInfo = {
        pageIndex: 1,
        showPages: 10,
        totalPages: 0
    }
    $scope.selectedAppId = '';
    $scope.logLevel = '';
    var pageSize = 15;
    $scope.rows = [];
    $scope.selectRow = {};

    $scope.showTraceDetail = function (row) {
        $scope.selectRow = row;
        $('#popTraceDetail').modal('show');
    }

    $scope.getPageTrace = function (pageIndex) {
        $scope.pageInfo.pageIndex = pageIndex;
        $http.get('/home/pagetrace?' +
            'appId=' + $scope.selectedAppId +
            '&pageIndex=' + pageIndex
            + '&pageSize=' + pageSize
            +'&logLevel='+$scope.logLevel
            + '&_=' + (new Date).getTime())
            .then(function (rep) {
                $scope.rows = rep.data.result;
                $scope.pageInfo.totalPages = Math.ceil(rep.data.totalCount / pageSize);
            });
    }

    var getApps = function () {
        $http.get('/home/apps?_=' + (new Date).getTime())
            .then(function (rep) {
                $scope.apps = rep.data.result;
                $scope.apps.unshift({ id: '',name:'all' });
                $scope.getPageTrace(1);
            });
    }

    $scope.selectedAppIdChanged = function () {
        $scope.getPageTrace(1);
    }

    getApps();
});