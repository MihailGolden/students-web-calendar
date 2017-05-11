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
        selecting: function (event, ui) {

            var selectedElems = $(".ui-selecting");

            var elem1 = selectedElems[0];
            var elem2 = selectedElems[selectedElems.length - 1];

            var rowid1 = +elem1.getAttribute("data-rowid");
            var rowid2 = +elem2.getAttribute("data-rowid");

            var id1 = +elem1.getAttribute("data-id");
            var id2 = +elem2.getAttribute("data-id");

            if (rowid1 != rowid2) {
                for (var i = id1; i < id2; i++) {
                   
                    var el = $(".selectable td[data-id='" + i + "']");

                    for (var j = 0; j < el.length; j++) {
                        $(el[j]).addClass('ui-selected')
                    }
                }
            }
        },
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
                    var firstSelectedElem = selectedElems[0];
                    var lastSelectedElem = selectedElems[selectedElems.length - 1];

                    var startHour12 = selectedElems[0].getAttribute("data-hour12");
                    var endHour12 = selectedElems[selectedElems.length - 1].getAttribute("data-hour12");

                    startDate = moment({
                        y: firstSelectedElem.getAttribute("data-year"),
                        M: firstSelectedElem.getAttribute("data-month") - 1,
                        d: firstSelectedElem.getAttribute("data-day"),
                        h: firstSelectedElem.getAttribute("data-hour")
                    });

                    endDate = moment({
                        y: lastSelectedElem.getAttribute("data-year"),
                        M: lastSelectedElem.getAttribute("data-month") - 1,
                        d: lastSelectedElem.getAttribute("data-day"),
                        h: lastSelectedElem.getAttribute("data-hour")
                    });

                    timePeriodStr = startDate.format('dddd').substring(0, 3) + ", " +
                        startDate.format('D') + " " + startDate.format('MMMM') + ", " +
                        startDate.format('Y') + " " + startHour12 +
                        " - " + endHour12;

                    break;
                case 'month':
                    var firstSelectedElem = selectedElems[0];
                    var lastSelectedElem = selectedElems[selectedElems.length - 1];

                    startDate = moment({
                        y: firstSelectedElem.getAttribute("data-year"),
                        M: firstSelectedElem.getAttribute("data-month") - 1,
                        d: firstSelectedElem.getAttribute("data-day"),
                        h: 0,
                        m: 0
                    });

                    endDate = moment({
                        y: lastSelectedElem.getAttribute("data-year"),
                        M: lastSelectedElem.getAttribute("data-month") - 1,
                        d: lastSelectedElem.getAttribute("data-day"),
                        h: 23,
                        m: 59
                    });

                    timePeriodStr = startDate.format('dddd').substring(0, 3) + ", " +
                        startDate.format('D') + " " + startDate.format('MMMM') + ", " +
                        startDate.format('Y') +
                        " - " +
                        endDate.format('dddd').substring(0, 3) + ", " +
                        endDate.format('D') + " " + endDate.format('MMMM') + ", " +
                        endDate.format('Y');

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

        $scope.init = function (currentCalendarId) {
            $scope.currentCalendarId = currentCalendarId;
        };

        //do not modify////
        $scope.nowDate = moment();
        //////////////////

        $scope.currentDate = moment();
        $scope.currentGrid = 'day';

        $scope.timePeriod = {
            startDate: moment($scope.currentDate),
            endDate: moment($scope.currentDate)
        };

        $scope.timePeriodStr = "";

        $scope.convertFrom12periodTo24 = function (hour) {
            var h = parseInt(hour);

            if (hour == '12am')
                h = 0;
            else if (hour == '12pm')
                h = 12;
            else if (~hour.indexOf("pm")) {
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
                    $scope.getEvents();
                    break;
                case 'week':
                    $scope.currentDate.add(1, "weeks");
                    break;
                case 'month': 
                    $scope.currentDate.add(1, "months");
                    $scope.getEvents();
                    break;
                default:
                    $scope.currentDate.add(1, "days");
            }

        };

        $scope.subtractDate = function () {
            switch ($scope.currentGrid) {
                case 'day':
                    $scope.currentDate.subtract(1, "days");
                    $scope.getEvents();
                    break;
                case 'week':
                    $scope.currentDate.subtract(1, "weeks");
                    break;
                case 'month':
                    $scope.currentDate.subtract(1, "months");
                    $scope.getEvents();

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

         $scope.generateDaysForGridWeek = function (events) {

            var weekGrid = [];

            var date1 = moment($scope.currentDate);
            date1.subtract(date1.day(), "days");

            $scope.hours.forEach(function (h) {
                var weekGridRow = {};
                weekGridRow.hour12 = h;
                weekGridRow.weekDays = [];

                for (var j = 0; j < 7; j++) {
                    var dayObj = {};
                    dayObj.day = date1.format("D");
                    dayObj.month = date1.format("M");
                    dayObj.year = date1.format("Y");
                    dayObj.hour = parseInt(h);
                    dayObj.class = 'selectable-cell';
                    //dayObj.class = (h == nowDay && currDateMonth == nowMonth &&  currDateYear == nowYear) ? 'curr-month curr-day' : 'curr-month';
                    weekGridRow.weekDays.push(dayObj);

                    date1.add(1, "days");
                }

                weekGrid.push(weekGridRow);

                date1.subtract(7, "days");
            });

            return weekGrid;
        };


         /* $scope.generateDaysForGridWeek = function (events) {

              events.sort(function (a, b) { return (moment(a.BeginTime).format("YYYY-MM-DD") > moment(b.BeginTime).format("YYYY-MM-DD")) ? 1 : ((moment(b.BeginTime).format("YYYY-MM-DD") > moment(a.BeginTime).format("YYYY-MM-DD")) ? -1 : 0); });

              var rows = 24;
              var cols = events.length + 8;
              var arrGrid = new Array(rows);
              for (var i = 0; i < rows; i++) {
                  arrGrid[i] = new Array(cols);
              }

              for (var i = 0; i < rows; ++i) {
                  for (var j = 0; j < cols; ++j) {
                      var eventObj = {};
                      if (j == 0)
                          eventObj.title = $scope.hours[i];
                      else {
                          eventObj.class = 'selectable-cell';
                          eventObj.hour = $scope.hours[i];
                      }
                      arrGrid[i][j] = eventObj;
                  }
              }

              var date1 = moment($scope.currentDate);
              date1.subtract(date1.day(), "days");

              var c = 1;
              events.forEach(function (event) {

                  var arrEvent = new Array(24);

                  for (var i = 0; i < arrEvent.length; i++) {
                      var eventObj = {};
                      eventObj.class = 'selectable-cell';
                      eventObj.hour = $scope.hours[i];
                      eventObj.day = date1.format("D");
                      eventObj.month = date1.format("M");
                      eventObj.year = date1.format("Y");
                      arrEvent[i] = eventObj;
                  }

                      for (var i = 0; i < arrEvent.length; i++) {
                          //arrEvent[i] = {};
                          if (i == moment(event.BeginTime).format("H")) {
                              //var eventObj = arrEvent[i];
                              arrEvent[i].id = event.ID;
                              arrEvent[i].title = event.Title;
                              arrEvent[i].titleAttr = event.Title;
                              arrEvent[i].color = "#00ff00";
                              arrEvent[i].class = undefined;


                              //                            arrEvent[i] = eventObj;

                              while (i < arrEvent.length && i != moment(event.EndTime).format("H")) {
                                  i++;
                                  arrEvent[i].id = event.ID;
                                  arrEvent[i].titleAttr = event.Title;
                                  arrEvent[i].color = "#00ff00";
                                  arrEvent[i].class = undefined;
                              }

                              break;
                          }

                      }
                  
                      while(moment(date1.BeginTime).format("YYYY-MM-DD") != moment(event.BeginTime).format("YYYY-MM-DD")) {
                          c++;
                          date1.add(1, "days");
                      }

                  for (var i = 0; i < 24; i++)
                      arrGrid[i][c] = arrEvent[i];

                  c++;

              });

              return arrGrid;
        };
        */

        function checkEvent(day, month, year, events) {//return number of events on current date
            var c = 0; 
            var currDate = moment({ years: year, months: month - 1, days: day }).format("YYYY-MM-DD");
            events.forEach(function (event) {

                if (currDate >= moment(event.BeginTime).format("YYYY-MM-DD") &&
                    currDate <= moment(event.EndTime).format("YYYY-MM-DD"))
                {
                    c++;
                }
            })

            return c;
        };

        

        $scope.generateCalMonthPage = function (events) {

            var currDate = moment($scope.currentDate);
            var prevDate = moment($scope.currentDate);
            var nextDate = moment($scope.currentDate);
            var currDateMonth = currDate.format('M');
            var currDateYear = currDate.format('Y');
            var currDateDay = currDate.format('D');

            var nowDay = $scope.nowDate.format('D');
            var nowMonth = $scope.nowDate.format('M');
            var nowYear = $scope.nowDate.format('Y');

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
                dayObj.class = (i == nowDay && currDateMonth == nowMonth &&  currDateYear == nowYear) ? 'curr-month curr-day' : 'curr-month';

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

                    var eventCount = 0;
                    if ((eventCount = checkEvent(cal[i][j].day, cal[i][j].month, cal[i][j].year, events)) > 0) {
                        cal[i][j].class = cal[i][j].class + " gridEvent";
                    }

                    cal[i][j].eventCount = eventCount;

                    cnt++;
                }
            }

            
            return cal;
        };

        $scope.eventInfo = function (elem) {
            if (elem != undefined) {
                location.href = '/Event/Details?id=' + elem;
            }

        };

        $scope.generateDayPageGrid = function (events) {
            
            var rows = 24;
            var cols = events.length + 2;
            var arrGrid = new Array(rows);
            for (var i = 0; i < rows; i++) {
                arrGrid[i] = new Array(cols);
            }

            for (var i = 0; i < rows; ++i) {
                for (var j = 0; j < cols; ++j) {
                    var eventObj = {};
                    if (j == 0)
                        eventObj.title = $scope.hours[i];
                    else {
                        eventObj.class = 'selectable-cell';
                        eventObj.hour = $scope.hours[i];
                    }
                    arrGrid[i][j] = eventObj;
                }
            }


            var c = 1;
            events.forEach(function (event) {
                var arrEvent = new Array(24);

                for (var i = 0; i < arrEvent.length; i++) {
                    var eventObj = {};
                    eventObj.class = 'selectable-cell';
                    eventObj.hour = $scope.hours[i];
                    arrEvent[i] = eventObj;
                }

                for (var i = 0; i < arrEvent.length; i++) {
                    if (i != moment(event.BeginTime).format("H") && $scope.currentDate.format("YYYY-MM-DD") == moment(event.BeginTime).format("YYYY-MM-DD")) {
                        continue;
                    }

                        arrEvent[i].id = event.ID;
                        arrEvent[i].title = event.Title;
                        arrEvent[i].titleAttr = event.Title;
                        arrEvent[i].eventLink = "link...";
                        arrEvent[i].color = event.EventColor;
                        arrEvent[i].class = undefined;
                        i++;
                        while (i < arrEvent.length) {
                            
                            if (i > moment(event.EndTime).format("H") && $scope.currentDate.format("YYYY-MM-DD") == moment(event.EndTime).format("YYYY-MM-DD"))
                                break;

                            arrEvent[i].id = event.ID;
                            arrEvent[i].titleAttr = event.Title;
                            arrEvent[i].eventLink = "link...";
                            arrEvent[i].color = event.EventColor;
                            arrEvent[i].class = undefined;

                            i++;
                        }

                        break;
                    }

                

                for (var i = 0; i < 24; i++)
                    arrGrid[i][c] = arrEvent[i];

                c++;

            });

            return arrGrid;

        };

        $scope.currentPageEvents = [];

        $scope.sendEventInfo = function (calendarID) {

            var eventTitle = document.getElementById("event-title").value; 
            var color = document.getElementById("color-value").defaultValue;

            $http({
                url: "/Event/Create",
                method: "POST",
                data: {
                    'title': eventTitle,
                    'beginTime': $scope.timePeriod.startDate.format("YYYY-MM-DD HH:mm"),
                    'endTime': $scope.timePeriod.endDate.format("YYYY-MM-DD HH:mm"),
                    'color': color,
                    'calendarID': calendarID
                },
            }).then(function mySucces(response) {
                $scope.getEvents();
               // location.reload(true);
            }, function myError(response, ajaxOptions, throwError) {
                alert('error');
            });
        };

        $scope.eventDetails = function (eventid) {

            $http({
                url: "/Event/Details?id=" + eventid,
                method: "GET"
            }).then(function mySucces(response) {
                //alert('success');
            }, function myError(response) {
                //alert('error');
            });
        };
        
        $scope.getEvents = function () {

            var sPeriod = moment($scope.currentDate);
            var ePeriod = moment($scope.currentDate);
            switch ($scope.currentGrid) {
                case 'day':
                    sPeriod.hour(0); sPeriod.minute(0);
                    ePeriod.hour(23); ePeriod.minute(59);
                    break;
                case 'week':
                    break;
                case 'month':
                    sPeriod.date(1);
                    ePeriod.date(ePeriod.daysInMonth()); 
                    break;
                default:
                    alert("error: undefined grid");
            }

                        $.ajax({
                            method: "GET",
                            url: "/Event/ListEventsInTimePeriod?calendarId=" + $scope.currentCalendarId + "&start=" + sPeriod.format("YYYY-MM-DD HH:mm") +
                                "&end=" + ePeriod.format("YYYY-MM-DD HH:mm"),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (data) {
                                $scope.currentPageEvents = data;
                                switch ($scope.currentGrid) {
                                    case 'day':
                                        $scope.dataForGridDay = $scope.generateDayPageGrid(data);
                                        break;
                                    case 'week':
                                        $scope.daysForGridWeek = $scope.generateDaysForGridWeek(data);
                                        break;
                                    case 'month':
                                        $scope.calMonthPage = $scope.generateCalMonthPage(data);
                                        break;
                                    default:
                                        alert("error: undefined grid");
                                }
                            }
                        });

                    };

                    $scope.nav = function (fileName, grid) {
                        $scope.fileName = fileName;
                        $scope.currentGrid = grid;

                        $scope.getEvents();

                    };

                    $scope.isDataLoaded = function (grid) {
                        switch (grid) {
                            case 'day':
                                return $scope.dataForGridDay != undefined;
                            case 'week':
                                return $scope.daysForGridWeek != undefined;
                            case 'month':
                                return $scope.calMonthPage != undefined;
                            default:
                                alert("error: undefined grid");
                        }
                    }

                    $scope.currentDateEvents = [];


                    $scope.showEvents = function (year, month, day) {
                        var events = [];
                        var currDate = moment({ years: year, months: month - 1, days: day }).format("YYYY-MM-DD");
                        $scope.currentPageEvents.forEach(function (event) {
                            
                            var beginTime = moment(event.BeginTime).format("YYYY-MM-DD");
                            var endTime = moment(event.EndTime).format("YYYY-MM-DD");

                            if (currDate >= beginTime && currDate <= endTime) {
                                var eventObj = {}
                                eventObj.id = event.ID;
                                eventObj.title = event.Title;
                                eventObj.beginTime = beginTime;
                                eventObj.endTime = endTime;

                                events.push(eventObj);

                            }

                        });

                        $scope.currentDateEvents = events;

                        $('#modal-list-event').modal({
                            show: true
                        });

                        
                     //   event.stopImmediatePropagation();
                    }


    });

$(function () {
    $('#Date').datetimepicker({
        locale: 'ru'
    });
});