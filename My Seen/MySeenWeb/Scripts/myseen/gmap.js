$(document).ready(function () {
    $('#my_map').gmap3(
        {
            map: {
                options: {
                    zoom: 2,
                    mapTypeId: window.google.maps.MapTypeId.ROADMAP
                }
            }
        });
});

function CalcDistance(data)
{
    var trackCoordsLatLng = [];
    $.each(data, function (i, item) {
        //console.log("history Latitude=", item.Latitude, "Longitude=", item.Longitude);
        trackCoordsLatLng.push(new window.google.maps.LatLng(item.Latitude, item.Longitude));
    });

    var polyline = new window.google.maps.Polyline({
        path: trackCoordsLatLng
    });

    return window.google.maps.geometry.spherical.computeLength(polyline.getPath()) / 1000;
}

function CalcDistanceFromTxt(data)
{
    var array = data.split(";");

    var trackCoordsLatLng = [];
    $.each(array, function (i, item) {
        //console.log("CalcDistanceFromTxt item=", item);
        trackCoordsLatLng.push(new window.google.maps.LatLng(item.split(",")[0], item.split(",")[1]));
    });

    var polyline = new window.google.maps.Polyline({
        path: trackCoordsLatLng
    });

    return window.google.maps.geometry.spherical.computeLength(polyline.getPath()) / 1000;
}

function clearPolylines()
{
    jQuery("#my_map").gmap3({
        clear: {
            tag: ["polyline"]
        }
    });
}

function clearMap() {
    clearPolylines();
}

function SetZoomAndCenter(center, zoom) {
    jQuery("#my_map").gmap3(
    {
        map: {
            options: {
                zoom: zoom,
                center: new window.google.maps.LatLng(center.Latitude, center.Longitude)
            }
        }
    });
}

function addPolyline(trackCoordsLatLng) {
    jQuery("#my_map").gmap3(
    {
        polyline: {
            options: {
                strokeColor: "#FF0000",//Красный
                strokeOpacity: 1.0,
                strokeWeight: 2,
                path: trackCoordsLatLng
            },
            tag: ["polyline"]
        }
    });
}

function showTrack(id,centerAndZoom)
{
    $.getJSON('/Home/GetTrack/' + id + '/', function (data)
    {
        var trackCoordsLatLng = [];
        $.each(data.Path, function (i, item)
        {
            trackCoordsLatLng.push(new window.google.maps.LatLng(item.Latitude, item.Longitude));
        });

        if (centerAndZoom) {

            clearMap();

            addPolyline(trackCoordsLatLng);

            SetZoomAndCenter(data.Center, getZoom(data.Min,data.Max));
        }
        else
        {
            addPolyline(trackCoordsLatLng);
        }
    });
}

function getZoom(min, max) {

    var p1 = new window.google.maps.LatLng(max.Latitude, max.Longitude);
    var p2 = new window.google.maps.LatLng(min.Latitude, min.Longitude);
    var maxLen = window.google.maps.geometry.spherical.computeDistanceBetween(p1, p2) / 1000;

    var zoom = 12;
    if (maxLen < 10) zoom = 14;
    else if (maxLen >= 10 && maxLen < 30) zoom = 11;
    else if (maxLen >= 30 && maxLen < 100) zoom = 10;
    else if (maxLen >= 100 && maxLen < 160) zoom = 9;
    else if (maxLen >= 160 && maxLen < 400) zoom = 8;
    else if (maxLen >= 400 && maxLen < 600) zoom = 7;
    else if (maxLen >= 600 && maxLen < 1000) zoom = 6;
    else if (maxLen >= 1000 && maxLen < 1300) zoom = 5;
    return zoom;
}

function showTrackByKey(key) {

    $.getJSON('/Home/GetTrackByKey/' + key + '/', function (shareTrackInfo) {

        clearMap();

        $.each(shareTrackInfo.Data, function (i, item)
        {
            var trackCoordsLatLng = [];
            $.each(item, function (ip, itemp)
            {
                trackCoordsLatLng.push(new window.google.maps.LatLng(itemp.Latitude, itemp.Longitude));
            });
            addPolyline(trackCoordsLatLng);
        });

        SetZoomAndCenter(shareTrackInfo.Center, getZoom(shareTrackInfo.Min, shareTrackInfo.Max));
    });
}