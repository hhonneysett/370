    $(document).ready(function () {
        $('.check-create-a').click(function () {
            if ($(this).is(":checked"))
                $('.check-create').prop('checked', true);
            else
                $('.check-create').prop('checked', false);
        });
    });

    $(document).ready(function () {
        $('.check-read-a').click(function () {
            if ($(this).is(":checked"))
                $('.check-read').prop('checked', true);
            else
                $('.check-read').prop('checked', false);
        });
    });

    $(document).ready(function () {
        $('.check-update-a').click(function () {
            if ($(this).is(":checked"))
                $('.check-update').prop('checked', true);
            else
                $('.check-update').prop('checked', false);
        });
    });

    $(document).ready(function () {
        $('.check-delete-a').click(function () {
            if ($(this).is(":checked"))
                $('.check-delete').prop('checked', true);
            else
                $('.check-delete').prop('checked', false);
        });
    });