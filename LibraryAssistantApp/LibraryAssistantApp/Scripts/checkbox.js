    $(document).ready(function () {
        $('#checkCa').click(function () {
            if ($(this).is(":checked"))
                $('.checkC').prop('checked', true);
            else
                $('.checkC').prop('checked', false);
        });
    });

    $(document).ready(function () {
        $('#checkRa').click(function () {
            if ($(this).is(":checked"))
                $('.checkR').prop('checked', true);
            else
                $('.checkR').prop('checked', false);
        });
    });

    $(document).ready(function () {
        $('#checkUa').click(function () {
            if ($(this).is(":checked"))
                $('.checkU').prop('checked', true);
            else
                $('.checkU').prop('checked', false);
        });
    });

    $(document).ready(function () {
        $('#checkDa').click(function () {
            if ($(this).is(":checked"))
                $('.checkD').prop('checked', true);
            else
                $('.checkD').prop('checked', false);
        });
    });