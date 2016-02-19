///////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////           Variables
///////////////////////////////////////////////////////////////////////
var GmapLanguage;//en/ru строка
var GmapMarkers; //True/False строка
var GmapMenuName; //set point here

function setGmapMenuName(name) {
    GmapMenuName = name;
};
function setGmapLanguage(language) {
    GmapLanguage = language;
};
function setGmapMarkers(markers) {
    GmapMarkers = markers;
};
///////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////           Icons
///////////////////////////////////////////////////////////////////////
function getMarkerIcon(type) {
    //https://sites.google.com/site/gmapicons/home
    if (type === "start") return "http://www.google.com/mapfiles/dd-start.png";
    else if (type === "end") return "http://www.google.com/mapfiles/dd-end.png";
    else if (type === "next") return "http://maps.google.com/mapfiles/marker_green.png";
    return "http://www.google.com/mapfiles/marker.png";
};
///////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////           Polylines
///////////////////////////////////////////////////////////////////////
function addPolyline(trackCoordsLatLng, color) {

    var strokeColor = "#FF0000";//Красный
    if (color === "green") strokeColor = "#008000";//зеленый

    jQuery("#my_map").gmap3(
    {
        polyline: {
            options: {
                strokeColor: strokeColor,
                strokeOpacity: 1.0,
                strokeWeight: 2,
                path: trackCoordsLatLng
            },
            tag: ["polyline"]
        }
    });
};

function clearPolylines() {
    jQuery("#my_map").gmap3({
        clear: {
            tag: ["polyline"]
        }
    });
};
///////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////           Markers
///////////////////////////////////////////////////////////////////////
function addMarker(markerCoords, data, icon) {
    if (GmapMarkers === 'False') return;

    jQuery("#my_map").gmap3(
    {
        marker: {
            values: [
              {
                  latLng: markerCoords,
                  data: data,
                  options: { icon: getMarkerIcon(icon) }
              }
            ],
            options: {
                draggable: false
            },
            tag: ["marker"],
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
                }
            }
        }
    });
};

function clearMarkers() {
    jQuery("#my_map").gmap3({
        clear: {
            tag: ["marker"]
        }
    });
};
///////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////           FIT
///////////////////////////////////////////////////////////////////////
function autoFit() {
    $("#my_map").gmap3({
        autofit: {}
    });
};
///////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////           DISTANCE
///////////////////////////////////////////////////////////////////////
function CalcDistance(data) {
    var trackCoordsLatLng = [];
    $.each(data, function (i, item) {
        //console.log("history Latitude=", item.Latitude, "Longitude=", item.Longitude);
        trackCoordsLatLng.push(new window.google.maps.LatLng(item.Latitude, item.Longitude));
    });

    var polyline = new window.google.maps.Polyline({
        path: trackCoordsLatLng
    });

    return window.google.maps.geometry.spherical.computeLength(polyline.getPath()) / 1000;
};
function CalcDistanceFromTxt(data) {
    var array = data.split(";");

    var trackCoordsLatLng = [];
    $.each(array, function (i, item) {
        //console.log("CalcDistanceFromTxt item=", item);
        if (item !== "") trackCoordsLatLng.push(new window.google.maps.LatLng(item.split(",")[0], item.split(",")[1]));
    });

    var polyline = new window.google.maps.Polyline({
        path: trackCoordsLatLng
    });

    return window.google.maps.geometry.spherical.computeLength(polyline.getPath()) / 1000;
};
///////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////           Clear
///////////////////////////////////////////////////////////////////////
function clearMap() {
    clearPolylines();
    clearMarkers();
};
///////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////           SHOW ROAD
///////////////////////////////////////////////////////////////////////
function showRoad(roadInfo) {
    //console.log(roadInfo);

    if (roadInfo.Id < 0) return;//skip Id for all

    var arrayOfStrings = roadInfo.Coordinates.split(';');

    var trackCoordsLatLng = [];
    arrayOfStrings.forEach(function (item) {
        if (item) {
            trackCoordsLatLng.push(new window.google.maps.LatLng(parseFloat(item.split(',')[0].trim()), parseFloat(item.split(',')[1].trim())));
        }
    });

    addPolyline(trackCoordsLatLng);

    addMarker(trackCoordsLatLng[0], roadInfo.Name + " - " + roadInfo.DateText + " (" + roadInfo.DistanceText + ")", "start");
    addMarker(trackCoordsLatLng[trackCoordsLatLng.length - 1], roadInfo.Name + " - " + roadInfo.DateText + " (" + roadInfo.DistanceText + ")", "end");
};