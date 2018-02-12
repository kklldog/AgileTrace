app.controller('queryCtrl', function ($scope, $http) {
    $scope.pageInfo = {
        pageIndex: 1,
        showPages: 5,
        totalPages: 10
    }
    var pageSize = 15;

    $scope.rows = [];
    $scope.selectRow = {};
    $scope.showTraceDetail = function(row) {
        $scope.selectRow = row;
        $('#popTraceDetail').modal('show');
    }

    var getPageTrace = function (pageIndex) {
        $scope.pageInfo.pageIndex = pageIndex;
        $http.get('/home/pagetrace?pageIndex=' + pageIndex + '&pageSize=' + pageSize + '&_=' + (new Date).getTime())
            .then(function (rep) {
                $scope.rows = rep.data.result;
                $scope.pageInfo.totalPages = Math.ceil(rep.data.totalCount / pageSize);
            });
    }

    getPageTrace(1);
});