//toggle arrow and div
function toggleArrow(div, span) {
    $(div).toggle(800);
    if ($(span).hasClass("glyphicon glyphicon-triangle-right")) {
        $(span).removeClass("glyphicon glyphicon-triangle-right");
        $(span).addClass("glyphicon glyphicon-triangle-bottom");
    } else {
        $(span).removeClass("glyphicon glyphicon-triangle-bottom");
        $(span).addClass("glyphicon glyphicon-triangle-right");
    }
}

//get venues
function getVenues() {
    $("#buildingFloorTable tbody tr").click(function () {
        var selected = $(this).hasClass("alert-info");
        $("#buildingFloorTable tbody tr").removeClass("alert-info");

        if (!selected) {
            $(this).addClass("alert-info");
        }

        var buildingFloorId = $(this).prop('id');

        var buildingFloorName = $(this).html();

        $.ajax({
            type: 'GET',
            url: '/Venue/getVenues',
            data: { id: buildingFloorId },
            success: function (result) {

                //replace html with partial view
                $('#venues').replaceWith(result);
                $("#venues").show(800);
                $("#toggleBuildingFloor").hide(800);

                //add building to selection summary
                $('#buildingFloorSummary').html(buildingFloorName);
                $('#venueSummary').html("");

                //toggle tab
                $("#buildingFloorDetails").click(function () {
                    toggleArrow("#toggleBuildingFloor", "#buildingFloorDetails span");
                })

            },
            error: function (err, result) {
                alert("Error in assigning dataToSave" + err.responseText);
            }
        });
    });
}

//get building floors
function getBuildingFloors() {
    $("#buildingTable tbody tr").click(function () {
        var selected = $(this).hasClass("alert-info");
        $("#buildingTable tbody tr").removeClass("alert-info");

        if (!selected) {
            $(this).addClass("alert-info");
        }

        var buildingId = $(this).prop('id');

        var buildingName = $(this).html();

        $.ajax({
            type: 'GET',
            url: '/Venue/getBuildingFloors',
            data: { id: buildingId },
            success: function (result) {

                //replace html with partial view
                $('#buildingFloors').replaceWith(result);
                $("#buildingFloors").show(800);
                $("#toggleBuilding").hide(800);

                //add building to selection summary
                $('#buildingSummary').html(buildingName);
                $('#buildingFloorSummary').html("");
                $('#venueSummary').html("");

                //toggle tab
                $("#buildingFloorDetails").click(function () {
                    toggleArrow("#toggleBuildingFloor", "#buildingFloorDetails span");
                });

                getVenues();
            },
            error: function (err, result) {
                alert("Error in assigning dataToSave" + err.responseText);
            }
        });
    });
}

//get buildings
function getBuildings() {
    $("#campusTable tbody tr").click(function () {
        var selected = $(this).hasClass("alert-info");
        $("#campusTable tbody tr").removeClass("alert-info");

        if (!selected) {
            $(this).addClass("alert-info");
        }

        var campusId = $(this).prop('id');

        var campusName = $(this).html();

        $.ajax({
            type: 'GET',
            url: '/Venue/getBuildings',
            data: { id: campusId },
            success: function (result) {

                //replace html with partial view
                $('#buildings').replaceWith(result);
                $("#buildings").show(800);
                $("#toggleCampus").hide(800);
                $("#buildingFloors").hide();

                //add campus to selection summary
                $('#campusSummary').html(campusName);
                $('#buildingSummary').html("");
                $('#buildingFloorSummary').html("");
                $('#venueSummary').html("");

                //toggle tab
                $("#buildingDetails").click(function () {
                    toggleArrow("#toggleBuilding", "#buildingDetails span");
                });

                getBuildingFloors();
            },
            error: function (err, result) {
                alert("Error in assigning dataToSave" + err.responseText);
            }
        });
    })
}