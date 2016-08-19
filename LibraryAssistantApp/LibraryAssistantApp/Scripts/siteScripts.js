function AssignButtonClicked(elem) {
        var id = $(elem).data('assigned-id');
        alert(id);
        $.ajax({
            type: 'POST',
            url: '/Booking/venueSelect',
            data: "id=" + id,
            success: function (result) {
                alert("AssignButtonClicked");
            },
            error: function (err, result) {
                alert("Error in assigning dataToSave" + err.responseText);
            }
        });
}

$('#btnSubmit').on('click', function () {
    $("#confirmDetailsForm").dialog({
        autoOpen: true,
        position: { my: "center", at: "top+350", of: window },
        width: 1000,
        resizable: false,
        title: 'Confirm Details:',
        modal: true,
        open: function () {
            $(this).load('/Booking/confirmDetails');
        },
        buttons: {
            "Confirm Booking": function () {
                confirmBooking();
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
    return false;
});
$('#btnRegisterStudent').on('click', function () {
    $("#oneTimePin").dialog({
        autoOpen: true,
        position: { my: "center", at: "top+350", of: window },
        width: 1000,
        resizable: false,
        title: 'Email Confirmation:',
        modal: true,
        open: function () {
            $(this).load('/RegisteredPerson/oneTimePin');
        },
        buttons: {
            "Submit": function () {
                checkPin();
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
    return false;
});
function confirmBooking() {
    alert("Calling Method");
    $.ajax({
        type: 'POST',
        url: '/Booking/captureDetails',
        success: function (result) {
            window.location.href = result;
            alert("Captured");
        },
        error: function (err, result) {
            alert("Error in assigning dataToSave" + err.responseText);
        }
    });
}