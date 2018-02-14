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
});