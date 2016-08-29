$(document).ready(function () {
    $('input.timepicker').timepicker({});
});
$('.timepicker').timepicker({
    timeFormat: 'h:mm p',
    interval: 30,
    minTime: '08:00am',
    maxTime: '5:00pm',
    //defaultTime: '8',
    startTime: '10:00',
    dynamic: true,
    dropdown: true,
    scrollbar: true
});