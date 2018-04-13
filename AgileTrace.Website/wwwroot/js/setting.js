app.controller('settingCtrl', function ($scope, $http) {
    $scope.rows = [];
    $scope.selectApp = {};

    var getApps = function () {
        $http.get('/app/apps?_=' + (new Date).getTime())
            .then(function (rep) {
                $scope.rows = rep.data.result;
            });
    }

    $scope.showAddApp = function () {
        $scope.selectApp = {
            name: '',
            securityKey: '',
            isAdd:true
        };
        $('#popEditApp').modal('show');
    }
    $scope.showUpdateApp = function (app) {
        $scope.selectApp = angular.copy(app, $scope.selectApp);
        $('#popEditApp').modal('show');
    }
    var updateApp = function (app) {
        $http.post('/app/updateApp', app)
            .then(function (rep) {
                if (rep.data) {
                    $('#popEditApp').modal('hide');
                    alert('edit successful 。');
                    getApps();
                } else {
                    alert('edit fail 。');
                }
            });
    }
    var addApp = function (app) {
        $http.post('/app/addApp', app)
            .then(function (rep) {
                if (rep.data) {
                    $('#popEditApp').modal('hide');
                    alert('add successful 。');
                    getApps();
                } else {
                    alert('add fail 。');
                }
            });
    }
    $scope.deleteApp = function (app) {
        var result = confirm('delete ？');
        if (result) {
            $http.post('/app/DeleteApp?id=' + app.id)
                .then(function (rep) {
                    if (rep.data) {
                        getApps();
                    } else {
                        alert('delete fail 。');
                    }
                });
        }
    }

    $scope.doSave = function () {
        if (!$scope.selectApp.name) {
            alert('App name can not empty !');
            return;
        }
        if (!$scope.selectApp.securityKey) {
            alert('App securityKey can not empty !');
            return;
        }
        if (!$scope.selectApp.isAdd) {
            //update
            updateApp($scope.selectApp);
        } else {
            //add
            addApp($scope.selectApp);
        }
    }

    getApps();
});