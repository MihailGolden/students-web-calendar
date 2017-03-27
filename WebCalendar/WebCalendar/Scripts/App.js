var calendar = {
    init: function () {
        var date = new Date();
        var startDate = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
        var monthNumber = date.getMonth();

        function getMonthName(number) {
            var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            return months[number];
        }
        $('.month').text(getMonthName(monthNumber));
        $('div.col-md-1.table-bordered .row .col-md-1.table-bordered:contains("' + date.getDate() + '")').addClass('current-day');
    }
};

$(document).ready(function () {
    calendar.init();

    $('#modal-new-event').on('hidden.bs.modal', function () {
        $("input").val('');
        $('.selectable .ui-selected').removeClass('ui-selected');
    });

    $('.selectable').selectable({
        filter: ".selectable-cell",
        stop: function () {
            var selectedElems = $(".ui-selected");
            var $scope = getScope('calendarCtrl');

            var startHour = selectedElems[0].getAttribute("data-hour");
            var endHour = selectedElems[selectedElems.length - 1].getAttribute("data-hour");

            $scope.$apply(function () {
                $scope.calendar.dayTimePeriod.startHour = startHour;
                $scope.calendar.dayTimePeriod.endHour = endHour;
            });

            $('#modal-new-event').modal({
                show: true
            });
        }
    });

});

function getScope(ctrlName) {
    var sel = 'div[ng-controller="' + ctrlName + '"]';
    return angular.element(sel).scope();
}

angular.module('calendarApp', []).controller('calendarCtrl', function ($scope, $http) {
    $scope.hours = ["12am", "1am", "2am", "3am", "4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am",
        "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm"
    ];

    $scope.fileName = 'gridForDay.html';
    $scope.filePath = function () {
        return 'Calendar/GridHtml?fileName=' + $scope.fileName;
    };
    $scope.nav = function (fileName) {
        $scope.fileName = fileName;
    };

    $scope.currentDate = new Date();

    $scope.calendar = {

        dayTimePeriod: {
            startHour: "",
            endHour: ""
        },

        getDayName: function (date) {
            var date = date == null ? $scope.currentDate : date;
            var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
            return days[date.getDay()];
        },

        addDays: function (date, days) {
            var result = date == null ? new Date($scope.currentDate) : new Date(date);
            var days = days == null ? 1 : days;
            result.setDate(result.getDate() + days);
            $scope.currentDate = result;
            return result;
        },

        subtractDays: function (date, days) {
            var result = date == null ? new Date($scope.currentDate) : new Date(date);
            var days = days == null ? 1 : days;
            result.setDate(result.getDate() - days);
            $scope.currentDate = result;
            return result;
        },

        getDate: function (date) {
            var date = date == null ? $scope.currentDate : date;
            return date.getDate();
        },

        getMonth: function (date) {
            var date = date == null ? $scope.currentDate : date;
            return date.getMonth() + 1;
        },

        getMonthName: function (date) {
            var date = date == null ? $scope.currentDate : date;
            var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            return months[date.getMonth()];
        }

    };

    $scope.sendEventInfo = function () {
        var event = document.getElementById("event-title").value;
        $http({
            url: "/",
            method: "POST",
            data: {
                'event': event,
                'startHour': $scope.calendar.dayTimePeriod.startHour,
                'endHour': $scope.calendar.dayTimePeriod.endHour,
                'year': $scope.currentDate.getFullYear(),
                'month': $scope.calendar.getMonth(),
                'day': $scope.calendar.getDate()
            }

        }).then(function mySucces(response) {
            //alert('success');
        }, function myError(response) {
            //alert('error');
        });
    };

});