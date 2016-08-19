    $(document).ready(function () {
        $('#checkBoxCreateAll').click(function () {
            if ($(this).is(":checked"))
                $('.checkCreate').prop('checked', true);
            else
                $('.checkCreate').prop('checked', false);
        });
    });

    $(document).ready(function () {
        $('#checkBoxReadAll').click(function () {
            if ($(this).is(":checked"))
                $('.checkRead').prop('checked', true);
            else
                $('.checkRead').prop('checked', false);
        });
    });

    $(document).ready(function () {
        $('#checkBoxUpdateAll').click(function () {
            if ($(this).is(":checked"))
                $('.checkUpdate').prop('checked', true);
            else
                $('.checkUpdate').prop('checked', false);
        });
    });

    $(document).ready(function () {
        $('#checkBoxDeleteAll').click(function () {
            if ($(this).is(":checked"))
                $('.checkDelete').prop('checked', true);
            else
                $('.checkDelete').prop('checked', false);
        });
    });