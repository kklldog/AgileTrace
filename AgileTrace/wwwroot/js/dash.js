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

    var renderChart = function (category, data) {
        var chart0 = echarts.init(document.getElementById('chart0'));
        var option = {
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
           
            series: [
                {
                    name: 'LogLevel',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '40%'],
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
        chart0.setOption(option);
    }

    var logLevels = ['Debug', 'Trace', 'Info', 'Warn', 'Error', 'Fatal'];
    $http.post('/home/getchartdata', logLevels)
        .then(function (rep) {
            renderChart(logLevels, rep.data);
        });
});