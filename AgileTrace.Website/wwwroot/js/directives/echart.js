app.directive('echart', function () {
    var renderChart = function (ele, option) {
        var chart = echarts.init(ele);
        chart.setOption(option);
    }
    return {
        restrict: 'A',
        replace: false,
        scope: {
            chartOption: "="
        },
        template: '',
        link: function (scope, element, attrs, ngModel) {
            renderChart(element[0], scope.chartOption);
        }
    }
})