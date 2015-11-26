//http://gmap3.net/en/pages/19-demo/
//var lat=$map.gmap3('getLatlng').lat();
//var lon = $map.gmap3('getLatlng').lon();

var current;// current click event
var coordinates = 0;

$(document).ready(function () {
    var menu = new Gmap3Menu($("#my_map"));

    menu.add("SET POINT HERE", "itemA",
        function() {
            menu.close();
            addMarker();
        });

    $("#my_map").gmap3({
        map: {
            options: {
                zoom: 2,
                mapTypeId: window.google.maps.MapTypeId.ROADMAP
            },
            events: {
                rightclick: function (map, event) {
                    current = event;
                    //console.log("rightclick");
                    menu.open(current);
                },
                click: function () {
                    //console.log("click");
                    menu.close();
                },
                dragstart: function () {
                    menu.close();
                },
                zoom_changed: function () {
                    menu.close();
                }
            }
        }
    });
});

// add marker and manage which one it is (A, B)
function addMarker() {

    var info = ""+(coordinates + 1);
    var tag = "marker-" + (coordinates + 1);
    var latlng = current.latLng;
    //console.log("addMarker tag="+tag);
    // add marker and store it
    $("#my_map").gmap3({
        marker: {
            latLng: latlng,
            data: info,
            options: {
                draggable: false, //пок анельзя двигать
                icon: getMarkerIcon("next")
            },
            tag: tag,
            events: {
                mouseover: function (marker, event, context) {
                    var map = $(this).gmap3("get"),
                      infowindow = $(this).gmap3({ get: { name: "infowindow" } });
                    if (infowindow) {
                        infowindow.open(map, marker);
                        infowindow.setContent(context.data);
                    } else {
                        $(this).gmap3({
                            infowindow: {
                                anchor: marker,
                                options: { content: context.data }
                            }
                        });
                    }
                },
                mouseout: function () {
                    var infowindow = $(this).gmap3({ get: { name: "infowindow" } });
                    if (infowindow) {
                        infowindow.close();
                    }
                },
                dragend: function (marker) {
                    //updateMarker(marker);
                }
            },
            callback: function (marker) {
                //updateMarker(marker);
            }
        }
    });
    AddCoordinate(latlng);
    calculateRoute();
}


function removeMarker(id) {
    //console.log("removeMarker="+id);
    jQuery("#my_map").gmap3({
        clear: {
            tag: ["marker-" + id]
        }
    });
    calculateRoute();
}

function AddCoordinate(latlng) {
    coordinates++;
    //console.log("AddCoordinate");
    var $panel = $("#panelCoordinates");
    $panel.append("<div class=\"list-group-item track-id-" + coordinates + "\" \" id=\"" + latlng + "\"> <h6 class=\"list-group-item-text align-left\"><span class=\"align-center\">" + coordinates + "</span><span class=\"pull-right alert-info small\"><button class=\"btn btn-xs btn-danger\" id=\"" + coordinates + "\"><small><span class=\"glyphicon glyphicon-trash\"></span></small></button></span></h6></div>");
}

function calculateRoute() {
    //console.log("calculateRoute");
    //посчитаем сколько координат мы имеем на панели
    var $panel = $("#panelCoordinates");
    var $rows = $panel.find("div");

    var trackCoordsLatLng = [];

    $rows.each(function (index, element) {
        var $row = $(element);
        //console.log("row=", $row);
        var id = $row.attr("id");
        //console.log("id=", id);
        id = id.replace('(','');
        id = id.replace(')','');
        trackCoordsLatLng.push(new window.google.maps.LatLng(id.split(",")[0], id.split(",")[1]));
    });

    var $saveButton = $("#saveButtonMain");

    clearPolylines();

    var $mapStatisticPoints = $("#mapStatisticPoints");
    var $mapStatisticDistance = $("#mapStatisticDistance");
    $mapStatisticPoints.text("Points: " + trackCoordsLatLng.length);
    $mapStatisticDistance.text("Distance: 0");

    if (trackCoordsLatLng.length === 0) {
        coordinates = 0;
    }
    if (trackCoordsLatLng.length > 1)
    {
        addPolyline(trackCoordsLatLng, "red");

        var polyline = new window.google.maps.Polyline({
            path: trackCoordsLatLng
        });
        var distance = window.google.maps.geometry.spherical.computeLength(polyline.getPath()) / 1000;
        $mapStatisticDistance.text("Distance: " + parseInt(distance) + " Km");

        $saveButton.show();
    } else {
        $saveButton.hide();
    }
}