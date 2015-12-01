
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

function getZoom(min, max) {

    var p1 = new window.google.maps.LatLng(max.Latitude, max.Longitude);
    var p2 = new window.google.maps.LatLng(min.Latitude, min.Longitude);
    var maxLen = window.google.maps.geometry.spherical.computeDistanceBetween(p1, p2) / 1000;
    console.log("len=", maxLen);

    var zoom = 12;
    if (maxLen < 10) zoom = 14;
    else if (maxLen >= 10 && maxLen < 30) zoom = 11;
    else if (maxLen >= 30 && maxLen < 100) zoom = 10;
    else if (maxLen >= 100 && maxLen < 160) zoom = 9;
    else if (maxLen >= 160 && maxLen < 400) zoom = 8;
    else if (maxLen >= 400 && maxLen < 600) zoom = 7;
    else if (maxLen >= 600 && maxLen < 1000) zoom = 6;
    else if (maxLen >= 1000 && maxLen < 1300) zoom = 5;
    else if (maxLen >= 1300 && maxLen < 3500) zoom = 3;
    else if (maxLen >= 3500) zoom = 2;
    console.log("zoom=", zoom);
    return zoom;
}
function SetZoom(zoom) {
    jQuery("#my_map").gmap3(
    {
        map: {
            options: {
                zoom: zoom
            }
        }
    });
}
function SetCenter(center) {
    jQuery("#my_map").gmap3(
    {
        map: {
            options: {
                center: new window.google.maps.LatLng(center.Latitude, center.Longitude)
            }
        }
    });
}
function SetCenterDefault(center) {
    jQuery("#my_map").gmap3(
    {
        map: {
            options: {
                center: new window.google.maps.LatLng(48.86745543642139, 2.1407350835937677)
            }
        }
    });
}

function SetZoomAndCenter(center, zoom) {
    SetZoom(zoom);
    SetCenter(center);
}