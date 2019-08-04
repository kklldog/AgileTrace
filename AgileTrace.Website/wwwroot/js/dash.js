app.factory('websocket', function ($http) {

    var service = {};
    service.opened = false;

    service.open = function (wsurl) {
        if (service.opened) {
            return service.ws;
        }
        var ws = new WebSocket(wsurl);
        ws.onopen = function (evt) {
            service.opened = true;
            service.ws = ws;
            console.log('Connection open ...');
            ws.send('hi');
        }
        ws.onclose = function (evt) {
            console.log('Connection closed.');
            service.ws = undefined;
            service.opened = false;
        };
        return ws;
    };

    return service;

});

app.controller('dashCtrl', function ($scope, $http, websocket) {
    $scope.traces = [];
    $http.get('/home/gethost?_=' + (new Date).getTime())
        .then(function (rep) {
            var wsurl = 'ws://' + rep.data + '/ws';
            var ws = websocket.open(wsurl);
            ws.onmessage = function (evt) {
                console.log('Received Message: ' + evt.data);
                $scope.$apply(function () {
                    if (evt.data !== 'hi') {
                        $scope.traces.pop();
                        var t = JSON.parse(evt.data);
                        $scope.traces.push(t);
                    }
                });
            };
        });

    var newOption = function (category,title, data) {
        var option = {
            title: {
                text: title,
                top: 20,
                x:'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            series: [
                {
                    name: 'LogLevel',
                    type: 'pie',
                    radius: '70%',
                    center: ['50%', '50%'],
                    data: data,
                    itemStyle: {
                        emphasis: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ]
        };

        return option;
    }

    $scope.chartsOptions = [];
    var logLevels = ['Debug', 'Trace', 'Info', 'Warn', 'Error', 'Fatal'];
    $http.post('/home/DashChartData', logLevels)
        .then(function (rep) {
            angular.forEach(rep.data, function (d) {
                var title = !d.appName ? 'all' : d.appName;
                var op = newOption(logLevels, title, d.data);
                $scope.chartsOptions.push(op);
            });
        });
});