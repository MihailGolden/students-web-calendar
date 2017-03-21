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
});