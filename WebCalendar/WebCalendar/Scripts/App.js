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

    var app = angular.module('calendarApp', []);
    app.controller('calendarCtrl', function ($scope, $http) {
        $scope.hours = ["12am", "1am", "2am", "3am", "4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am",
            "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm"
        ];

        $scope.fileName = 'GridForDay';
        $scope.filePath = function () {
            return '/Calendar/GridHtml?fileName=' + $scope.fileName;
        };
        $scope.nav = function (fileName, grid) {
            $scope.fileName = fileName;
            $scope.currentGrid = grid;
        };

        $scope.currentDate = new Date();
        $scope.currentGrid = 'day';


        $scope.calendar = {

            dayTimePeriod: {
                startHour: "",
                endHour: ""
            },

            getDayName: function (date) {
                var date = date == null ? $scope.currentDate : new Date(date);
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

            addMonths: function (date, months) {
                var result = date == null ? new Date($scope.currentDate) : new Date(date);
                var months = months == null ? 1 : months;
                result.setMonth(result.getMonth() + months);
                $scope.currentDate = result;
                return result;
            },

            subtractMonths: function (date, months) {
                var result = date == null ? new Date($scope.currentDate) : new Date(date);
                var months = months == null ? 1 : months;
                result.setMonth(result.getMonth() - months);
                $scope.currentDate = result;

                return result;
            },

            addWeeks: function (date, weeks) {
                var result = date == null ? new Date($scope.currentDate) : new Date(date);
                var days = weeks == null ? 7 : weeks * 7;

                return this.addDays(result, days);
            },

            subtractWeeks: function (date, weeks) {
                var result = date == null ? new Date($scope.currentDate) : new Date(date);
                var days = weeks == null ? 7 : weeks * 7;

                return this.subtractDays(result, days);
            },

            addDate: function () {
                switch ($scope.currentGrid) {
                    case 'day':
                        return this.addDays();
                    case 'week':
                        return this.addWeeks();
                    case 'month':
                        return this.addMonths();
                    default:
                        return this.addDays();
                }
            },

            subtractDate: function () {
                switch ($scope.currentGrid) {
                    case 'day':
                        return this.subtractDays();
                    case 'week':
                        return this.subtractWeeks();
                    case 'month':
                        return this.subtractMonths();
                    default:
                        return this.subtractDays();
                }
            },

            getDate: function (date) {
                var date = date == null ? $scope.currentDate : new Date(date);
                return date.getDate();
            },

            getMonth: function (date) {
                var date = date == null ? $scope.currentDate : new Date(date);
                return date.getMonth() + 1;
            },

            getMonthName: function (date) {
                var date = date == null ? $scope.currentDate : new Date(date);
                var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                return months[date.getMonth()];
            },

            getWeek: function () {
                var date1 = new Date($scope.currentDate);
                var date2 = new Date($scope.currentDate);
                date1.setDate($scope.currentDate.getDate() - date1.getDay());
                date2.setDate($scope.currentDate.getDate() + (6 - date2.getDay()));

                var firstDateOfCurrWeek = date1.getDate();
                var lastDateOfCurrWeek = date2.getDate();

                return {
                    firstDay: firstDateOfCurrWeek,
                    month1: $scope.calendar.getMonth(date1),
                    lastDay: lastDateOfCurrWeek,
                    month2: $scope.calendar.getMonth(date2)
                }
            },

            getYear: function (date) {
                var date = date == null ? $scope.currentDate : new Date(date);
                return date.getFullYear();
            }

        };

            $scope.generateHeadersForGridWeek = function (){
                var headers = [];
                var date1 = new Date($scope.currentDate);
                date1.setDate($scope.currentDate.getDate() - date1.getDay());
                for (var i = 0; i < 7; i++){
                    var d = date1.getDate();
                    var m = date1.getMonth() + 1;
                    var mn = $scope.calendar.getDayName(date1).substring(0,2);
                    var h = "" + mn + ", " + m + "/" + d;
                    headers.push(h);

                    date1.setDate(date1.getDate() + 1);
                }

                return headers;
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

$(function () {
    $('#Date').datetimepicker({
        locale: 'ru'
    });
});