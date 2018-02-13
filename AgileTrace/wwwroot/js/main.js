var app = angular.module('app', ['ui.router', 'agile.bootstrap-pagebar', 'ngCookies','ngAnimate']);
app.config(function ($stateProvider, $urlRouterProvider) {
        var dashState = {
            name: 'dash',
            url: '/dash',
            templateUrl: '/home/getview?viewName=dash',
            controller: 'dashCtrl'
        };

        var queryState = {
            name: 'query',
            url: '/query',
            templateUrl: '/home/getview?viewName=query',
            controller: 'queryCtrl'
        };

        var settingState = {
            name: 'setting',
            url: '/setting',
            templateUrl: '/home/getview?viewName=setting',
            controller: 'settingCtrl'
        };

        $stateProvider.state(dashState);
        $stateProvider.state(queryState);
        $stateProvider.state(settingState);

        $urlRouterProvider.otherwise("/dash");
    }
);
