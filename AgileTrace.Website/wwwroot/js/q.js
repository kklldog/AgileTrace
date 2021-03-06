﻿app.controller('queryCtrl', function ($scope, $http,$filter) {
    var dateFilter = $filter('date');

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

    var initQueryDate = function (){
        $scope.startDate = dateFilter(new Date(),'yyyy-MM-dd');
        $scope.endDate = $scope.startDate;
    }

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
            +'&startDate='+$scope.startDate
            +'&endDate='+$scope.endDate
            + '&_=' + (new Date).getTime())
            .then(function (rep) {
                $scope.rows = rep.data.result;
                $scope.pageInfo.totalPages = Math.ceil(rep.data.totalCount / pageSize);
            });
    }

    var getApps = function () {
        $http.get('/app/apps?_=' + (new Date).getTime())
            .then(function (rep) {
                $scope.apps = rep.data.result;
                $scope.apps.unshift({ id: '',name:'all' });
                $scope.getPageTrace(1);
            });
    }

    $scope.findApp = function(appId) {
        for (var i = 0; i < $scope.apps.length; i++) {
            var item = $scope.apps[i];
            if (item.id === appId)
                return item;
        }

        return {
            id: '',
            name: ''
        };
    }

    $scope.selectedAppIdChanged = function () {
        $scope.getPageTrace(1);
    }

    initQueryDate();
    getApps();
});