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

function clearMap()
{
    //Очистка старых
    jQuery("#my_map").gmap3({
        clear: {
            tag: ["polyline"]
        }
    });
}
function addNew_WithZoomAndCenter(data, trackCoordsLatLng, zoom)
{
    jQuery("#my_map").gmap3(
    {
        map: {
            options: {
                zoom: zoom,
                center: new window.google.maps.LatLng(data.Center.Latitude, data.Center.Longitude)
            }
        },
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
function addNew(trackCoordsLatLng) {
    jQuery("#my_map").gmap3(
    {
        map: {
            options: {
                zoom: 2
            }
        },
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

function GetTrackNameByIdIntoField(id, field)
{
    $.getJSON('/Home/GetTrackNameById/' + id + '/', function (data) {
        //console.log('loaded data=', data);
        field.val(data);
        return data;
    });
}
function GetTrackCoordinatesByIdIntoField(id, field) {
    $.getJSON('/Home/GetTrackCoordinatesById/' + id + '/', function (data) {
        //console.log('loaded data=', data);
        field.val(data);
        return data;
    });
}
function showTrack(id,centerAndZoom)
{
    $.getJSON('/Home/GetTrack/' + id + '/', function (data)
    {
        //console.log("location Latitude=", data.Location.Latitude, "Longitude", data.Location.Longitude);

        var trackCoordsLatLng = [];
        $.each(data.Path, function (i, item)
        {
            //console.log("history Latitude=", item.Latitude, "Longitude", item.Longitude);
            trackCoordsLatLng.push(new window.google.maps.LatLng(item.Latitude, item.Longitude));
        });

        if (centerAndZoom) {
            /*
            var polyline = new google.maps.Polyline({
                path: trackCoordsLatLng
            });
            */

            var zoom = 12;

            //расстояние от удаленных точек, есть смысл их считать, только по горизонтали
            var p1 = new window.google.maps.LatLng(data.Max.Latitude, data.Max.Longitude);
            var p2 = new window.google.maps.LatLng(data.Min.Latitude, data.Min.Longitude);
            //console.log("max Latitude=", data.Max.Latitude, "Longitude", data.Max.Longitude);
            //console.log("min Latitude=", data.Min.Latitude, "Longitude", data.Min.Longitude);

            var maxLen = window.google.maps.geometry.spherical.computeDistanceBetween(p1, p2) / 1000;
            console.log("maxLen=", maxLen);
            if (maxLen < 10) {
                zoom = 14;
            }
            else if (maxLen >= 10 && maxLen < 30) {
                zoom = 11;
            }
            else if (maxLen >= 30 && maxLen < 100) {
                zoom = 10;
            }
            else if (maxLen >= 100 && maxLen < 160) {
                zoom = 9;
            }
            else if (maxLen >= 160 && maxLen < 400) {
                zoom = 8;
            }
            else if (maxLen >= 400 && maxLen < 600) {
                zoom = 7;
            }
            else if (maxLen >= 600 && maxLen < 1000) {
                zoom = 6;
            }

            clearMap();
            addNew_WithZoomAndCenter(data, trackCoordsLatLng, zoom);
        }
        else
        {
            addNew(trackCoordsLatLng);
        }
    });
}