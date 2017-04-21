(function () {
    var now = moment().date();

    function MonthCalendar(events) {
        this.events = events;
        this.selector = document.querySelector('#calendar-event');
        this.firstDay = moment().date(1);
        this.events.forEach(function (event) {
            event.startDate = moment(event.startDate);
            event.endDate = moment(event.endDate);
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
            if (!this.calWeek || day.day() === 0) {
                this.calWeek = document.createElement('div');
                this.calWeek.className = 'week';
                this.calMonth.appendChild(this.calWeek);
            }
            var wrapper = document.createElement('div');
            wrapper.className = 'wrapper';

            var name = document.createElement('div');
            name.className = 'd-name';
            name.textContent = day.format('ddd');

            var number = document.createElement('div');
            number.className = 'd-number';
            number.textContent = day.format('DD');

            var event = document.createElement('div');
            event.className = 'd-event';
            this.drawEvent(day, event);

            wrapper.appendChild(name);
            wrapper.appendChild(number);
            wrapper.appendChild(event);
            this.calWeek.appendChild(wrapper);
        };

        this.upToActualMonth = function () {
            var clone = this.firstDay.clone();
            var day = clone.day();

            if (!day) { return; }

            clone.subtract('days', day + 1);

            for (var i = day; i > 0; i--) {
                this.drawDay(clone.add('days', 1));
            }
        };

        this.actualMonth = function () {
            var day = this.firstDay.clone();
            if (!day) { return; }
            while (day.month() === this.firstDay.month()) {
                this.drawDay(day);
                day.add('days', 1);
            }
        };

        this.afterActualMonth = function () {
            var clone = this.firstDay.clone().add('months', 1).subtract('days', 1);
            var day = clone.day();

            if (day === 6) { return; }

            for (var i = day; i < 6; i++) {
                this.drawDay(clone.add('days', 1));
            }
        };

        this.drawMonth = function () {
            this.calMonth = document.createElement('div');
            this.calMonth.className = 'month';
            this.selector.appendChild(this.calMonth);
            this.upToActualMonth();
            this.actualMonth();
            this.afterActualMonth();
        };

        this.drawEvent = function (day, sel) {
            if (day.month() === this.firstDay.month()) {
                var arrayEvents = this.events.filter(function (event) {
                    return event.startDate.isSame(day, 'day') || event.endDate.isSame(day, 'day');
                });
                arrayEvents.forEach(function (event) {
                    var elem = document.createElement('span');
                    elem.className = event.color;
                    sel.appendChild(elem);
                });
            }
        };  
    }
    var events = [{ id: 1, color: 'blue', startDate: '2017-04-17', endDate: '2017-04-19' }
    , { id: 2, color: 'red', startDate: '2017-04-18', endDate: '2017-04-21' }];
    var calendar = new MonthCalendar(events);
    calendar.draw();
})();