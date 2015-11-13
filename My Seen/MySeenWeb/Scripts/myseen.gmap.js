$(document).ready(function () {
    $('#my_map').gmap3(
        {
            map: {
                options: {
                    zoom: 2,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                }
            }
        });
});

function CalcDistance(data)
{
    var track_coords_LatLng = [];
    $.each(data, function (i, item) {
        //console.log("history lat=", item.lat, "lng=", item.lng);
        track_coords_LatLng.push(new google.maps.LatLng(item.lat, item.lng));
    });

    var polyline = new google.maps.Polyline({
        path: track_coords_LatLng
    });

    return google.maps.geometry.spherical.computeLength(polyline.getPath()) / 1000;
}
function CalcDistanceFromTxt(data)
{
    var array = data.split(";");

    var track_coords_LatLng = [];
    $.each(array, function (i, item) {
        //console.log("CalcDistanceFromTxt item=", item);
        track_coords_LatLng.push(new google.maps.LatLng(item.split(",")[0], item.split(",")[1]));
    });

    var polyline = new google.maps.Polyline({
        path: track_coords_LatLng
    });

    return google.maps.geometry.spherical.computeLength(polyline.getPath()) / 1000;
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
function addNew_WithZoomAndCenter(data, track_coords_LatLng, Zoom)
{
    jQuery("#my_map").gmap3(
    {
        map: {
            options: {
                zoom: Zoom,
                center: new google.maps.LatLng(data.Center.lat, data.Center.lng)
            }
        },
        polyline: {
            options: {
                strokeColor: "#FF0000",//Красный
                strokeOpacity: 1.0,
                strokeWeight: 2,
                path: track_coords_LatLng
            },
            tag: ["polyline"]
        }
    });
}
function addNew(track_coords_LatLng) {
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
                path: track_coords_LatLng
            },
            tag: ["polyline"]
        }
    });
}

function showTrack(id,centerAndZoom)
{
    $.getJSON('/Home/GetTrack/' + id + '/', function (data)
    {
        //console.log("location lat=", data.Location.lat, "lng", data.Location.lng);

        var track_coords_LatLng = [];
        $.each(data.Path, function (i, item)
        {
            //console.log("history lat=", item.lat, "lng", item.lng);
            track_coords_LatLng.push(new google.maps.LatLng(item.lat, item.lng));
        });

        if (centerAndZoom) {
            var polyline = new google.maps.Polyline({
                path: track_coords_LatLng
            });

            var Zoom = 12;

            //расстояние от удаленных точек, есть смысл их считать, только по горизонтали
            var p1 = new google.maps.LatLng(data.Max.lat, data.Max.lng);
            var p2 = new google.maps.LatLng(data.Min.lat, data.Min.lng);
            //console.log("max lat=", data.Max.lat, "lng", data.Max.lng);
            //console.log("min lat=", data.Min.lat, "lng", data.Min.lng);

            var maxLen = google.maps.geometry.spherical.computeDistanceBetween(p1, p2) / 1000;
            console.log("maxLen=", maxLen);
            if (maxLen < 10) {
                Zoom = 14;
            }
            else if (maxLen >= 10 && maxLen < 30) {
                Zoom = 11;
            }
            else if (maxLen >= 30 && maxLen < 100) {
                Zoom = 10;
            }
            else if (maxLen >= 100 && maxLen < 200) {
                Zoom = 9;
            }
            else if (maxLen >= 200 && maxLen < 400) {
                Zoom = 8;
            }
            else if (maxLen >= 400 && maxLen < 600) {
                Zoom = 7;
            }
            else if (maxLen >= 600 && maxLen < 1000) {
                Zoom = 6;
            }

            clearMap();
            addNew_WithZoomAndCenter(data, track_coords_LatLng, Zoom);
        }
        else
        {
            addNew(track_coords_LatLng);
        }
    });
}