$(document).ready(function () {
    $('#result-table-1').click(function () {
        alert($(this).attr('id'));
    });
});

$(document).ready(function () {
    $('#result-table-1 tr').click(function () {
        $('.here').attr('id', 'collection');
    });
});