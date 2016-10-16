function AssignButtonClicked(elem) {
    var id = $(elem).data('assigned-id');
    $.ajax({
        type: 'POST',
        url: '/Booking/venueSelect',
        data: "id=" + id,
        success: function (result) { },
        error: function (err, result) {
            alert("Error in assigning dataToSave" + err.responseText);
        }
    });
}

$('#btnSubmit').on('click', function () {
    $("#confirmDetailsForm").dialog({
        autoOpen: true,
        position: {
            my: "center",
            at: "top+350",
            of: window
        },
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
        position: {
            my: "center",
            at: "top+350",
            of: window
        },
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
});

function confirmBooking() {
    $.ajax({
        type: 'POST',
        url: '/Booking/captureDetails',
        success: function (result) {
            window.location.href = result;
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
        data: {
            id: inputId,
            idType: inputType
        },
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
            scheduler.parse(events, "json"); //takes the name and format of the data source

            scheduler.attachEvent("onClick", function (id, e) {
                //any custom logic here
                $("#bookingDetailsForm").dialog({
                    autoOpen: true,
                    position: {
                        my: "center",
                        at: "top+350",
                        of: window
                    },
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
                                $.ajax({
                                    type: 'GET',
                                    url: '/Booking/updateStatus',
                                    data: "status=" + selectList,
                                    success: function (result) {
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
                        "Update Booking": {
                            text: "Update Booking",
                            id: "btnUpdateBooking",
                            click: function () {
                                location = "/Booking/updateBookingDetails";
                            }
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                        },
                    }
                });
            });
        },
        error: function (err, result) {
            alert("Error in assigning dataToSave" + err.responseText);
        }
    });
    scheduler.updateView();
};

//run scripts on document ready
$(document).ready(function () {

    //check inputted person id is an employee person id
    jQuery.validator.addMethod("validTrainerID", function (value, element) {
        var regex = /^[p][0-9]{8}$/;
        return this.optional(element) || regex.test(value);
    }, "Invalid person ID");

    jQuery.validator.addMethod("validStudentID", function (value, element) {
        var regex = /^[u][0-9]{8}$/;
        return this.optional(element) || regex.test(value);
    }, "You have entered an invalid studnet number");

    $("#manageTraining").validate({
        rules: {
            trainerID: {
                validTrainerID: true,
                required: true,
            }
        },
        errorClass: "my-error-class",
    })

    $('#timepicker').removeAttr("data-val");

    //hide training details panel
    $('#trainingDetails').hide();

    //show calendar for trainer
    $("#btnTrainerID").click(function () {
        if ($("#manageTraining").valid()) {
            manageTrainingCalendar($("#trainerID").val())
            $('#manageCampusSelect').prop('value', 0);
        }
    })

    //show calendar for campus
    $("#manageCampusSelect").change(function () {
        manageTrainingCalendar($(manageCampusSelect).children(":selected").attr("value"))
    })

});

//toggle the inputted sections
function toggleSection(a, b) {
    $(b).toggle(800);
    if ($(a + " span").hasClass("glyphicon glyphicon-triangle-right")) {
        $(a + " span").removeClass("glyphicon glyphicon-triangle-right");
        $(a + " span").addClass("glyphicon glyphicon-triangle-bottom");
    } else {
        $(a + " span").removeClass("glyphicon glyphicon-triangle-bottom");
        $(a + " span").addClass("glyphicon glyphicon-triangle-right");
    }
}

//submit the details for the training session
function submitTrainingSession() {
    //initiate validation for form 2
    $("#form2").validate({
        rules: {
        description: "required",
        confirmation: "required",
        privacy: "required",
        attendance: { required:true, number: true },
        notify: "required",
        },
        errorClass: "my-error-class",
    });
    
    //submit the form details
    if ($("#form2").valid())
    {
        var repeat;

        if ($("#repeatError").hasClass("glyphicon-remove") && $("#repeatType").children(":selected").val() != "none") {
            if (confirm("Clash booking detected, you can continue with your booking but the booking won't recur as requested. Would you like to continue?") == true) {
                repeat = "none";
                $.ajax({
                    type: 'GET',
                    url: '/Trainer/captureTrainingSession',
                    data: {
                        description: $('#description').val(),
                        maxAtt: $("#maxAtt").val(),
                        confirmation: $("#confirmationStatus").val(),
                        privacy: $("#privacyStatus").val(),
                        notify: $("#notifySelect").val(),
                        repeatType: repeat,
                        multiple: $("#repeatTimes").val(),
                    },
                    success: function (result) {
                        window.location.href = "/Trainer/manageTrainingSession"
                    },
                    error: function (err, result) {
                        alert("Error in assigning dataToSave" + err.responseText);
                    }
                });
            }
            else return;
            
        } else {

            repeat = $("#repeatType").children(":selected").val();
            $.ajax({
                type: 'GET',
                url: '/Trainer/captureTrainingSession',
                data: {
                    description: $('#description').val(),
                    maxAtt: $("#maxAtt").val(),
                    confirmation: $("#confirmationStatus").val(),
                    privacy: $("#privacyStatus").val(),
                    notify: $("#notifySelect").val(),
                    repeatType: repeat,
                    multiple: $("#repeatTimes").val(),
                },
                success: function (result) {
                    window.location.href = "/Trainer/manageTrainingSession"
                },
                error: function (err, result) {
                    alert("Error in assigning dataToSave" + err.responseText);
                }
            });
        }
    }
}

//show calendar for training sessions
function manageTrainingCalendar(id) {
    $.ajax({
        type: 'GET',
        url: '/Trainer/getTrainingSessions',
        data: "id=" + id,
        success: function (result) {

            //clear the calendar
            scheduler.clearAll();

            //configure the calendar
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

                //create an event for each booking
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
            scheduler.parse(events, "json"); //takes the name and format of the data source

            //make events clickable
            scheduler.attachEvent("onClick", function (id, e) {
                //any custom logic here
                showTrainingDetails(id);
            });

        },
        error: function (err, result) {
            alert("Unfortunately, something went wrong, please contact IT support for further assistance. Have a nice day!");
        }
    });
}

//show popup of details
function showTrainingDetails(id) {
    $.ajax({
        type: 'GET',
        url: '/Trainer/trainingSessionDetails',
        data: { id: id },
        success: function (result) {

            $('#trainingDetails').show(1100);
            $('#trainingDetails').replaceWith(result);
            $("#btnGenerateAttendance").click(function () {
                getRegister();
            })

            $("#cancelTraining").click(function () {
                $("#cancelConfirm").dialog({
                    autoOpen: true,
                    position: {
                        my: "center",
                        at: "top+350",
                        of: window
                    },
                    width: 600,
                    resizable: false,
                    title: 'Cancel Confirmation:',
                    modal: true,
                    open: function () {
                        var markup = 'Are you sure you want to cancel this training session?';
                        $(this).html(markup);
                    },
                    buttons: {
                        "Confirm": function () {
                            cancelTraining();
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            });
        },
        error: function (err, result) {
            alert("Error in assigning dataToSave" + err.responseText);
        }
    });
}

function cancelTraining() {
    $.ajax({
        type: 'GET',
        url: '/Trainer/cancelTraining',
        success: function (result) {
            window.location.href = "/Trainer/manageTrainingSession";
        },
        error: function (err, result) {
            alert("Unfortunately, something went wrong, please contact IT support for further assistance. Have a nice day!");
        }
    });
}

//request one time pin
function oneTimePin()
{
    var email = $("#Person_Email").val();
    $.ajax({
        type: 'GET',
        url: '/RegisteredPerson/oneTimePin',
        data: { email: email },
        success: function (result) {
            $("#onetimepin").dialog({
                autoOpen: true,
                position: {
                    my: "center",
                    at: "top+350",
                    of: window
                },
                width: 1000,
                resizable: false,
                title: 'Email Confirmation:',
                modal: true,
                open: function () {
                    $(this).load('/RegisteredPerson/showOTP');
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
        },
        error: function (err, result) {
            alert("Error in assigning dataToSave" + err.responseText);
        }
    });
}

function getSelectedTraining(elem) {
    var id = $(elem).data('assigned-id');
    $.ajax({
        type: 'GET',
        url: '/Booking/sessionSelect',
        data: "id=" + id,
        success: function (result) { },
        error: function (err, result) {
            alert("Error in assigning dataToSave" + err.responseText);
        }
    });
}

function getTrainingBookingDetails() {
    $("#confirmTrainingBooking").dialog({
        autoOpen: true,
        position: {
            my: "center",
            at: "top+350",
            of: window
        },
        width: 600,
        resizable: false,
        title: 'Confirm Booking:',
        modal: true,
        open: function () {
            $(this).load('/Booking/confirmStudentTraining');
        },
        buttons: {
            "Confirm": function () {
                studentBook();
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
}

function studentBook() {
    $.ajax({
        type: 'POST',
        url: '/Booking/captureStudentTraining',
        success: function (result) {
            window.location.href = "/Booking/ViewBookings/";
        },
        error: function (err, result) {
            alert("Error in assigning dataToSave" + err.responseText);
        }
    });
}
