$(document).ready(function () {
    $(document).on('click', '.serverMessage', function () {
        $(".serverMessage").hide('slide', { direction: 'up' });
    })

    setTimeout(function () {
        $('.serverMessage').hide('slide', { direction: 'up' });
    }, 3000);
})
