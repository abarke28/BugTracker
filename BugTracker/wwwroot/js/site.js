// Write your JavaScript code.

Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

Date.prototype.toShortDateString = function (){
    var date = new Date(this.valueOf());
    date.toISOString().split('T')[0];
}