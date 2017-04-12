(function () {
    var now = moment();

    function Calendar(events) {
        this.selector = document.querySelector('#calendar-event');
        this.day = moment().date(1);
        this.drawHeader = function () {
            this.header = document.createElement('div');
            this.header.className = 'header';
            this.title = document.createElement('h1');
            this.title.innerHTML = this.day.format('MMMM YYYY');
            this.header.appendChild(this.title);
            this.selector.appendChild(this.header);
        }
        this.draw = function () {
            this.drawHeader();
        }
    }


    var data = [];
    var calendar = new Calendar(data);
    calendar.draw();
})();




