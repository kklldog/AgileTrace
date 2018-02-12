app.controller('settingCtrl', function ($scope, $http) {
    $scope.rows = [];
    $scope.selectApp = {};

    var getApps = function () {
        $http.get('/home/apps?_=' + (new Date).getTime())
            .then(function (rep) {
                $scope.rows = rep.data.result;
            });
    }

    $scope.showAddApp = function () {
        $scope.selectApp = {
            name: '',
            securityKey: ''
        };
        $('#popEditApp').modal('show');
    }
    $scope.showUpdateApp = function (app) {
        $scope.selectApp = angular.copy(app, $scope.selectApp);
        $('#popEditApp').modal('show');
    }
    var updateApp = function (app) {
        $http.post('/home/updateApp', app)
            .then(function (rep) {
                if (rep.data) {
                    $('#popEditApp').modal('hide');
                    alert('update successful 。');
                    getApps();
                } else {
                    alert('update fail 。');
                }
            });
    }
    var addApp = function (app) {
        $http.post('/home/addApp', app)
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
        var result = confirm('确定删除？');
        if (result) {
            $http.post('/home/DeleteApp?id=' + app.id)
                .then(function (rep) {
                    if (rep.data) {
                        getApps();
                    } else {
                        alert('delete fail 。');
                    }
                });
        }
    }

    $scope.doSave = function() {
        if ($scope.selectApp.id) {
            //update
            updateApp($scope.selectApp);
        } else {
            //add
            addApp($scope.selectApp);
        }
    }

    getApps();
});