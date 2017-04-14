(function () {
    var now = moment();

    function Calendar(events) {
        this.events = events;
        this.selector = document.querySelector('#calendar-event');
        this.firstDay = moment().date(1);
        this.events.forEach(function (event) {
            event.startDate = moment(event.startDate);
        });

        this.draw = function () {
            this.drawHeader();
            this.drawMonth();
        };

        this.drawHeader = function () {
            this.header = document.createElement('div');
            this.header.className = 'header';
            this.title = document.createElement('h1');
            this.title.innerHTML = this.firstDay.format('MMMM YYYY');
            this.header.appendChild(this.title);
            this.selector.appendChild(this.header);
        };

        this.drawDay = function (day) {
            if (!this.calWeek || day.day() == 0) {
                this.calWeek = document.createElement('div');
                this.calWeek.className = 'week';
                this.calMonth.appendChild(this.calWeek);
            }
            var wrapper = document.createElement('div');
            wrapper.className = 'wrapper';

            var name = document.createElement('div');
            name.className = 'd-name';
            name.textContent = day.format('dddd');

            var number = document.createElement('div');
            number.className = 'd-number';
            number.textContent = day.format('DD');

            var event = document.createElement('div');
            event.className = 'd-event';

            wrapper.appendChild(name);
            wrapper.appendChild(number);
            wrapper.appendChild(event);
            this.calWeek.appendChild(wrapper);
        };

        this.drawMonth = function () {
            this.calMonth = document.createElement('div');
            this.calMonth.className = 'month';
            this.selector.appendChild(this.calMonth);

            var day = this.firstDay.clone();
            while (day.month() === this.firstDay.month()) {
                this.drawDay(day);
                day.add('days', 1);
            }
        };
    };
    var obj = { id: 1, startDate: '2017-14-02' };

    var events = [obj];

    var calendar = new Calendar(events);
    calendar.draw();
})();