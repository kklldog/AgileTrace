app.factory('websocket', function () {
    var ws = new WebSocket('ws://localhost:57524/ws');
    ws.onopen = function (evt) {
        console.log('Connection open ...');
        ws.send('hi');
    }

    ws.onclose = function (evt) {
        console.log('Connection closed.');
    };

    return ws;

});

app.controller('dashCtrl', function ($scope, websocket) {
    $scope.traces = [];
    websocket.onmessage = function (evt) {
        console.log('Received Message: ' + evt.data);
        $scope.$apply(function() {
            if (evt.data !== 'hi') {
                $scope.traces.pop();
                var t = JSON.parse(evt.data);
                $scope.traces.push(t);
            }
        });


    };
});