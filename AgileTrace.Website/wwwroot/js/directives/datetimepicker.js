app.directive('datetimepicker', function () {
    return {
        restrict: 'A',
        replace: false,
        scope: {
            dateValue: "=",
            viewMode: "@",
            startDate: '@',
            endDate:'@'
        },
        template: '',
        link: function (scope, element, attrs, ngModel) {
            var maxView = "year";
            var minView = "hour";
            var dateFomatter = "yyyy-mm-dd hh:ii";

            if (scope.viewMode == 'month') {
                maxView = "year";
                minView = "month";
                dateFomatter = "yyyy-mm-dd";
            }

            element.datetimepicker({
                language: 'zh-CN',
                format: dateFomatter,
                minView: minView,
                autoclose: true,
                minuteStep: 5,
                startDate: scope.startDate,
                endDate:scope.endDate
            }).on('changeDate', function () {
                scope.$apply(date);
            });

            function date() {
                console.log('setting date', element.val(), attrs.title);
                if (scope.dateValue!=undefined) {
                    scope.dateValue = element.val();
                }
            }

            date(); // initialize
        }
    }
});