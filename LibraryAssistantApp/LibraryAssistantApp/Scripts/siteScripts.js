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

//enable button on employeeViewBookings when status is updated
function enableButton() {
    var button = $("#btUpdateStatus");
    button.prop("disabled", false);
    button.removeClass("ui-button-disabled");
    button.removeClass("ui-state-disabled");
};

//display calendar on employeeViewBookings
function displayCalendar(inputId, inputType) {
    $.ajax({
        type: 'GET',
        url: '/Booking/getEmpBookings',
        data: { id: inputId, idType: inputType },
        success: function (result) {
            scheduler.clearAll();
            $("#scheduler_here").show();

            //configuration for the scheduler
            scheduler.config.resize_month_events = false;
            scheduler.config.first_hour = 8;
            scheduler.config.last_hour = 17;
            scheduler.config.start_on_monday = true;
            scheduler.locale.labels.agenda_tab = "Agenda";
            scheduler.config.readonly = true;


            //initialize the scheduler
            scheduler.init('scheduler_here', new Date(), "month");

            //create an events json object
            var events = [];

            //go through list of events and add each event to the json events object
            result.forEach(function (entry) {
                var event = {
                    id: entry.id,
                    text: entry.text,
                    start_date: moment(entry.start_date).format('MM[/]DD[/]YYYY h:mm:ss'),
                    end_date: moment(entry.end_date).format('MM[/]DD[/]YYYY h:mm:ss'),
                }

                //adds the event to the events object
                events.push(event);
            });

            //passes the events to the scheduler
            scheduler.parse(events, "json");//takes the name and format of the data source

            scheduler.attachEvent("onClick", function (id, e) {
                //any custom logic here
                $("#bookingDetailsForm").dialog({
                    autoOpen: true,
                    position: { my: "center", at: "top+350", of: window },
                    width: 1000,
                    resizable: false,
                    title: 'Booking Details',
                    modal: true,
                    open: function () {
                        $(this).load('/Booking/getEmpBookingDetails/?id=' + id);
                    },
                    buttons: {
                        "Update Booking Status": {
                            click: function () {
                                var selectList = $("#bookingStatusList").val();
                                alert(selectList);
                                $.ajax({
                                    type: 'GET',
                                    url: '/Booking/updateStatus',
                                    data: "status=" + selectList,
                                    success: function (result) {
                                        alert("Captured");
                                        $("#bookingDetailsForm").dialog("close");
                                        displayCalendar(inputId, inputType);
                                    },
                                    error: function (err, result) {
                                        alert("Error in assigning dataToSave" + err.responseText);
                                    }
                                });
                            },
                            text: "Update Booking Status",
                            id: "btUpdateStatus",
                            disabled: "disabled",
                        },
                        "Update Booking" :
                            {
                                text : "Update Booking",
                                id: "btnUpdateBooking",
                                click: function ()
                                {
                                    location = "/Booking/updateBookingDetails";
                                }
                            },
                        Cancel: function () {
                            $(this).dialog("close");
                        },
                    }
                });
                return false;
                return true;
            });
        },
        error: function (err, result) {
            alert("Error in assigning dataToSave" + err.responseText);
        }
    });
    scheduler.updateView();
};
