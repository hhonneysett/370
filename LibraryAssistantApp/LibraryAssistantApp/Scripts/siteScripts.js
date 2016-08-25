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

//run scripts on document ready
$(document).ready(function () {

    var $loading = $('#loadingDiv').hide();
    $(document)
      .ajaxStart(function () {
          $loading.show();
      })
      .ajaxStop(function () {
          $loading.hide();
      });

    //add a custom validation rule to validate date
    jQuery.validator.addMethod("validDateCheck", function (value, element) {
        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        return this.optional(element) || date_regex.test(value)
    });

    //check date isnt in the past
    jQuery.validator.addMethod("datePastCheck", function (value, element) {
        var convertDate = new Date(value);
        var now = new Date();
        if (convertDate < now) {
            return this.optional(element) || false;
        }
        else {
            return this.optional(element) || true;
        }
    }, "Date already taken place");

    //apply validation rules
    $("#form1").validate({
        rules: {
            category: "required",
            topic: "required",
            date: {
                required: true,
                validDateCheck: true,
                datePastCheck : true,
            },
            duration: "required",
            campus: "required",
        },
        messages: {
            category: "Please provide a category selection",
            topic: "Please provide a topic selection",
            duration: "Please provide a duration selection",
            campus: "Please provide a campus selection",
        },
        errorClass: "my-error-class",
    });

    $('#timepicker').removeAttr("data-val");

    //disable the create button and topic select on form load
    $("#topicSelect").attr('disabled', true);

    //show mathcing topics for selected category
    $("#categorySelect").change(function () {
        var id = $("#categorySelect").children(":selected").val();
        $.ajax({
            type: 'GET',
            url: '/Trainer/getCatTopic',
            data: {
                id: id
            },
            success: function (result) {
                var options = $("#topicSelect");

                //remove existing options from the select to add new options
                options.children().remove();

                options.append($("<option disabled selected value />").text("-- select topic --"));

                if (result.length == 0) {
                    options.children().remove();
                    options.append($("<option disabled selected value />").text("-- no topics --"));
                }

                //add an option to the select for each building
                result.forEach(function (entry) {
                    options.append($("<option />").val(entry.id).text(entry.text));
                });
                options.prop('disabled', false)
            },
            error: function (err, result) {
                alert("Error in assigning dataToSave" + err.responseText);
            }
        });
    });

    //load matching venues on button click
    $("#btnSubmit").click(function () {

        var check = $("#form1").valid();
        if (check) {
            $('#possibleTrainers').empty();
            $("#additionalDetails").empty();
            var characteristicsList = [];

            $('input:checkbox.Characteristic_ID').each(function () {
                var sThisVal = (this.checked ? $(this).val() : "");
                if (sThisVal > 0) {
                    characteristicsList.push(sThisVal);
                };
            });

            var model = {
                Category_ID: $("#categorySelect").val(),
                Topic_ID: $("#topicSelect").val(),
                duration: $("#durationSelect").val(),
                startDate: $("#datepicker").val(),
                Campus_ID: $("#campusSelect").val(),
            };

            //clear the session details div
            $("#sessiondetails").empty();

            $.ajax({
                type: 'GET',
                url: '/Trainer/getTrainingVenues',
                contentType: 'application/json; charset=utf-8',
                data: {
                    model: JSON.stringify(model),
                    characteristics: JSON.stringify(characteristicsList)
                },
                success: function (result) {

                    //display the available venues from a partial
                    $('#availableVenues').replaceWith(result);

                    //toggle the session details section
                    $("#toggleMain").toggle(800);
                    $("#detailsTop span").removeClass("glyphicon glyphicon-triangle-bottom");
                    $("#detailsTop span").addClass("glyphicon glyphicon-triangle-right");

                    //disable the proceed button
                    $("#btnProceed").attr('disabled', 'disabled');

                    //call a function on a table row click to capture the selected venue
                    $("#venueTable tbody tr").click(function () {
                        var selected = $(this).hasClass("alert-info");
                        $("#venueTable tbody tr").removeClass("alert-info");

                        if (!selected) {
                            $(this).addClass("alert-info");
                        }
                        var buttonEnabled = $(this).hasClass("alert-info");
                        if (buttonEnabled) {
                            $("#btnProceed").prop('disabled', false)
                        } else {
                            $("#btnProceed").prop('disabled', true)
                        }
                    });

                    //toggle details when clicked
                    $("#venueDetails").click(function () {
                        $("#toggleVenue").toggle(800);
                        if ($("#venueDetails span").hasClass("glyphicon glyphicon-triangle-right")) {
                            $("#venueDetails span").removeClass("glyphicon glyphicon-triangle-right");
                            $("#venueDetails span").addClass("glyphicon glyphicon-triangle-bottom");
                        } else {
                            $("#venueDetails span").removeClass("glyphicon glyphicon-triangle-bottom");
                            $("#venueDetails span").addClass("glyphicon glyphicon-triangle-right");
                        }
                    });

                    //call function on button proceed click
                    $("#btnProceed").click(function () {
                        $('#possibleTrainers').empty();
                        $("#additionalDetails").empty();
                        $.ajax({
                            type: 'GET',
                            url: '/Trainer/addTrainingSessionDetails',
                            success: function (result) {

                                //display the available venues from a partial
                                $('#sessiondetails').replaceWith(result);
                                $("#toggleVenue").toggle(800);
                                $("#venueDetails span").removeClass("glyphicon glyphicon-triangle-bottom");
                                $("#venueDetails span").addClass("glyphicon glyphicon-triangle-right");
                                $("#btnTimeslotProceed").attr('disabled', 'disabled');
                            },
                            error: function (err, result) {
                                alert("Error in assigning dataToSave" + err.responseText);
                            }
                        });
                    })
                },
                error: function (err, result) {
                    alert("Error in assigning dataToSave" + err.responseText);
                }
            });
        }

    });

    //toggle details when clicked
    $("#detailsTop").click(function () {
        $("#toggleMain").toggle(800);
        if ($("#detailsTop span").hasClass("glyphicon glyphicon-triangle-right")) {
            $("#detailsTop span").removeClass("glyphicon glyphicon-triangle-right");
            $("#detailsTop span").addClass("glyphicon glyphicon-triangle-bottom");
        } else {
            $("#detailsTop span").removeClass("glyphicon glyphicon-triangle-bottom");
            $("#detailsTop span").addClass("glyphicon glyphicon-triangle-right");
        }
    });



});

//get list of trainers that are available
function getTrainers(elem) {
    var id = $(elem).data('assigned-id');
    $("#btnTimeslotProceed").attr('disabled', false);
    $.ajax({
        type: 'GET',
        url: '/Trainer/getTrainers',
        data: "id=" + id,
        success: function (result) {
            $('#possibleTrainers').replaceWith(result);

            //select available trainer
            $("#trainerTable tbody tr").click(function () {

                var selected = $(this).hasClass("alert-info");
                $("#trainerTable tbody tr").removeClass("alert-info");

                if (!selected) {
                    $(this).addClass("alert-info");
                }
            });
        },
        error: function (err, result) {
            alert("Error in assigning dataToSave" + err.responseText);
        }
    });
};

//proceed from the select a timeslot section
function timeslotProceed() {
    toggleSection('#sessionTimeslot', '#toggleTimeslot');
    $.ajax({
        type: 'GET',
        url: '/Trainer/additionalDetails',
        success: function (result) {
            $('#additionalDetails').replaceWith(result);

            //only letters check
            jQuery.validator.addMethod("lettersonly", function(value, element) {
                return this.optional(element) || /^[a-z]+$/i.test(value);
            }, "Letters only please"); 



        },
        error: function (err, result) {
            alert("Error in assigning dataToSave" + err.responseText);
        }
    });
};

//get the selected trainer and capture to server
function trainerSelect(elem) {
    var id = $(elem).data('assigned-id');
    $.ajax({
        type: 'GET',
        url: '/Trainer/selectTrainer',
        data: "id=" + id,
        success: function (result) { },
        error: function (err, result) {
            alert("Error in assigning dataToSave" + err.responseText);
        }
    });
}

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

//toggle the repeat type
function getRepeatType() {
    var id = $("#repeatType").children(":selected").val();
    $("#repeatErrorDiv").hide(800);
    if (id == "none") {
        $("#repeatToggle").hide(800);
    } else {
        $("#repeatToggle").show(800);
    }

}

//check if the repeat type is valid
function checkRepeat() {
    var repeatType = $("#repeatType").children(":selected").val();
    var multiple = $("#repeatTimes").val();

    if (multiple > 0) {
        $.ajax({
            type: 'GET',
            url: '/Trainer/reapeatCheck',
            data: {
                repeatType: repeatType,
                multiple: multiple
            },
            success: function (result) {
                if (result == "True") {
                    $("#repeatErrorDiv").show();
                    $("#repeatError").removeClass("glyphicon glyphicon-remove")
                    $("#repeatError").addClass("glyphicon glyphicon-ok")
                    $("#repeatText").text(" No Clashes Detected")
                } else {
                    $("#repeatErrorDiv").show();
                    $("#repeatError").removeClass("glyphicon glyphicon-ok")
                    $("#repeatError").addClass("glyphicon glyphicon-remove")
                    $("#repeatText").text(" Clashes Detected")
                }
            },
            error: function (err, result) {
                alert("Error in assigning dataToSave" + err.responseText);
            }
        });
    }
}

//submit the details for the training session
function submitTrainingSession() {
    //initiate validation for form 2
    $("#form2").validate({
        rules: {
        description: { lettersonly: true },
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
        $.ajax({
            type: 'GET',
            url: '/Trainer/captureTrainingSession',
            data: {
                description: $('#description').val(),
                maxAtt: $("#maxAtt").val(),
                confirmation: $("#confirmationStatus").val(),
                privacy: $("#privacyStatus").val(),
                notify: $("#notifySelect").val(),
                repeatType: $("#repeatType").val(),
                multiple: $("#repeatTimes").val(),
            },
            success: function (result) {
                if (result) {
                    
                } else {
                    
                }
            },
            error: function (err, result) {
                alert("Error in assigning dataToSave" + err.responseText);
            }
        });
    }
}