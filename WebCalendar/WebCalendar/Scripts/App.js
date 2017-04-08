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

            var startDate, endDate, timePeriodStr;
            switch ($scope.currentGrid) {
                case 'day':
                    var startHour12 = selectedElems[0].getAttribute("data-hour");
                    var endHour12 = selectedElems[selectedElems.length - 1].getAttribute("data-hour");

                    var startHour24 = $scope.convertFrom12periodTo24(startHour12);
                    var endHour24 = $scope.convertFrom12periodTo24(endHour12);

                    startDate = moment({
                        y: $scope.currentDate.format("Y"),
                        M: $scope.currentDate.format("M") - 1,
                        d: $scope.currentDate.format("D"),
                        h: startHour24
                    });

                    endDate = moment({
                        y: $scope.currentDate.format("Y"),
                        M: $scope.currentDate.format("M") - 1,
                        d: $scope.currentDate.format("D"),
                        h: endHour24
                    });

                    timePeriodStr = $scope.currentDate.format("Y") + ", " +
                        $scope.currentDate.format("D") + " " + startDate.format('MMMM') + ", " +
                        startHour12 + " - " +
                        endHour12;

                    break;
                case 'week':
                    break;
                case 'month':
                    break;
            }

            $scope.$apply(function () {
                $scope.timePeriod.startDate = startDate;
                $scope.timePeriod.endDate = endDate;
                $scope.timePeriodStr = timePeriodStr;

                $('#modal-new-event').modal({
                    show: true
                });
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

        $scope.days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

        $scope.fileName = 'GridForDay';
        $scope.filePath = function () {
            return '/Calendar/GridHtml?fileName=' + $scope.fileName;
        };
        $scope.nav = function (fileName, grid) {
            $scope.fileName = fileName;
            $scope.currentGrid = grid;
        };

        $scope.currentDate = moment();
        $scope.currentGrid = 'day';

        $scope.timePeriod = {
            startDate: moment($scope.currentDate),
            endDate: moment($scope.currentDate)
        };

        $scope.timePeriodStr = "";

        $scope.convertFrom12periodTo24 = function (hour) {
            var h = parseInt(hour);

            if (~hour.indexOf("pm")) {
                h += 12;
            }

            return h;
        };

        $scope.convertFrom24periodTo12 = function (hour) {

            if (hour <= 12)
                hour = hour + 'am';
            else {
                hour -= 12;
                hour = hour + 'pm';
            }

            return hour;
        };

        $scope.addDate = function () {
            switch ($scope.currentGrid) {
                case 'day':
                    $scope.currentDate.add(1, "days");
                    break;
                case 'week':
                    $scope.currentDate.add(1, "weeks");
                    break;
                case 'month':
                    $scope.currentDate.add(1, "months");
                    break;
                default:
                    $scope.currentDate.add(1, "days");
            }
        };

        $scope.subtractDate = function () {
            switch ($scope.currentGrid) {
                case 'day':
                    $scope.currentDate.subtract(1, "days");
                    break;
                case 'week':
                    $scope.currentDate.subtract(1, "weeks");
                    break;
                case 'month':
                    $scope.currentDate.subtract(1, "months");
                    break;
                default:
                    $scope.currentDate.subtract(1, "days");
            }
        };

        $scope.getDayName = function () {
            return $scope.currentDate.format("dddd");
        };

        $scope.getDate = function () {
            return $scope.currentDate.format("D");
        };

        $scope.getMonth = function () {
            return $scope.currentDate.format("M");
        };

        $scope.getMonthName = function () {
            return $scope.currentDate.format("MMMM");
        };

        $scope.getYear = function () {
            return $scope.currentDate.format("Y");
        };

        $scope.calendar = {

            getDayName: function (date) {
                var date = date == null ? $scope.currentDate : new Date(date);
                var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
                return days[date.getDay()];
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

            generateMonthDays: function () {
                var month = $scope.currentDate.getMonth();
                var date = new Date($scope.currentDate.getFullYear(), $scope.currentDate.getMonth(), 1);
                var days = [];
                var i = 0;
                while (date.getMonth() === month) {
                    days.push(new Date(date));
                    date.setDate(date.getDate() + 1);
                }
                return days;
            }
        };

        $scope.getWeek = function () {
            var date1 = moment($scope.currentDate);
            var date2 = moment($scope.currentDate);
            date1.subtract(date1.day(), "days");
            date2.add(6 - date2.day(), "days");

            var firstDateOfCurrWeek = date1.format("D");
            var lastDateOfCurrWeek = date2.format("D");

            return {
                firstDay: firstDateOfCurrWeek,
                month1: date1.format("M"),
                lastDay: lastDateOfCurrWeek,
                month2: date2.format("M"),
                year: $scope.currentDate.format("Y")
            }
        };

        $scope.generateHeadersForGridWeek = function () {
            var headers = [];
            var date1 = moment($scope.currentDate);
            date1.subtract(date1.day(), "days");

            for (var i = 0; i < 7; i++) {
                var d = date1.format("D");
                var m = date1.format("M");
                var mn = date1.format('dddd').substring(0, 3);
                var h = "" + mn + ", " + m + "/" + d;
                headers.push(h);

                date1.add(1, "days");
            }

            return headers;
        };


        $scope.generateCalMonthPage = function () {

            var currDate = moment($scope.currentDate);
            var prevDate = moment($scope.currentDate);
            var nextDate = moment($scope.currentDate);
            var currDateMonth = currDate.format('M');
            var currDateYear = currDate.format('Y');
            var currDateDay = currDate.format('D');

            nextDate.add(1, "months");
            prevDate.subtract(1, 'months');

            var nextDateMonth = nextDate.format('M');
            var prevDateMonth = prevDate.format('M');


            var nextDateYear = nextDate.format('Y');
            var prevDateYear = prevDate.format('Y');

            var prevDateLastDayOfMonth = prevDate.endOf("month").format('D');
            currDate.date(1);
            var currDateDayOfTheWeek = currDate.day();
            var currDateNumOfDaysInMonth = currDate.daysInMonth();
            currDate.date(currDateNumOfDaysInMonth);
            var currDateDayOfTheWeekLastDay = currDate.day();

            var a = currDateDayOfTheWeek;
            var c = prevDateLastDayOfMonth - a + 1;
            var b = currDateDayOfTheWeekLastDay;


            var calDays = [];

            while (a > 0) {
                var dayObj = {};
                dayObj.day = c;
                dayObj.month = prevDateMonth;
                dayObj.year = prevDateYear;
                dayObj.class = 'prev-month';

                calDays.push(dayObj);

                c++;
                a--;
            }

            for (var i = 1; i <= currDateNumOfDaysInMonth; i++) {
                var dayObj = {};
                dayObj.day = i;
                dayObj.month = currDateMonth;
                dayObj.year = currDateYear;
                dayObj.class = i == currDateDay ? 'curr-month curr-day' : 'curr-month';

                calDays.push(dayObj);

            }

            var i = 1;
            if (b != 6)
                for (var i = 1; b < 6; i++, b++) {
                    var dayObj = {};
                    dayObj.day = i;
                    dayObj.month = nextDateMonth;
                    dayObj.year = nextDateYear;
                    dayObj.class = 'next-month';

                    calDays.push(dayObj);

                }
            //
            //                for (var i = 0; i < calDays.length; i++)
            //                    alert(calDays[i]);
            //                alert(calDays.length);

            var rows, cols = 7;
            if (calDays.length == 35)
                rows = 5;
            else if (calDays.length == 42)
                rows = 6;


            var cal = new Array(rows);
            for (var i = 0; i < rows; i++) {
                cal[i] = new Array(cols);
            }

            var cnt = 0;
            for (var i = 0; i < rows; i++) {
                for (var j = 0; j < cols; j++) {
                    cal[i][j] = calDays[cnt];
                    cal[i][j].id = cnt;
                    cnt++;
                }
            }


            return cal;
        };


        $scope.sendEventInfo = function () {
            var event = document.getElementById("event-title").value;

            $http({
                url: "/",
                method: "POST",
                data: {
                    'event': event,
                    'startDate': $scope.timePeriod.startDate.format("YYYY-MM-DD HH:mm"),
                    'endDate': $scope.timePeriod.endDate.format("YYYY-MM-DD HH:mm")
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