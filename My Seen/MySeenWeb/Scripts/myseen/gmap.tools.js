
function getMarkerIcon(type) {
    //https://sites.google.com/site/gmapicons/home
    if (type === "start") return "http://www.google.com/mapfiles/dd-start.png";
    else if (type === "end") return "http://www.google.com/mapfiles/dd-end.png";
    else if (type === "next") return "http://maps.google.com/mapfiles/marker_green.png";
    return "http://www.google.com/mapfiles/marker.png";
}

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
}

function clearPolylines() {
    jQuery("#my_map").gmap3({
        clear: {
            tag: ["polyline"]
        }
    });
}

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
}