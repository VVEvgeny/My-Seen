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

function clearMarkers() {
    jQuery("#my_map").gmap3({
        clear: {
            tag: ["marker"]
        }
    });
}

function clearMap() {
    clearPolylines();
    clearMarkers();
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

function addMarker(markerCoords, data, icon, id, key) {

    var trackCoordsLatLng = new window.google.maps.LatLng(markerCoords.Latitude, markerCoords.Longitude);

    jQuery("#my_map").gmap3(
    {
        marker: {
            values: [
              {
                  latLng: trackCoordsLatLng,
                  data: data,
                  options: { icon: getMarkerIcon(icon) }
              }
            ],
            options: {
                draggable: false
            },
            tag: ["marker"]
            ,
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
                click: function () {
                    if (id) {
                        console.log("key="+key);
                        if (key) showTrackByKey(key, id);
                        else showTrack(id, true, "green");
                    }
                }
            }
        }
    });
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
    console.log("zoom=", zoom);
    return zoom;
}

function showTrack(id, centerAndZoom,color) {

    $.getJSON('/Home/GetTrack/' + id + '/', function (trackInfo) {
        var trackCoordsLatLng = [];
        $.each(trackInfo.Path, function (i, item) {
            trackCoordsLatLng.push(new window.google.maps.LatLng(item.Latitude, item.Longitude));
        });

        if (centerAndZoom) {

            clearMap();

            addPolyline(trackCoordsLatLng, color);
            SetZoomAndCenter(trackInfo.Center, getZoom(trackInfo.Min, trackInfo.Max));
        }
        else {
            addPolyline(trackCoordsLatLng, color);
        }
        
        addMarker(trackInfo.Start, "Start " + trackInfo.Name + " - " + trackInfo.DateText, "start", trackInfo.Id);
        addMarker(trackInfo.End, "End " + trackInfo.Name + " - " + trackInfo.DateText, "end", trackInfo.Id);
    });
}

function showTrackByKey(key,id) {

    $.getJSON('/Home/GetTrackByKey/' + key + '/', function (shareTrackInfo) {

        clearMap();

        $.each(shareTrackInfo.Data, function (i, item)
        {
            var trackCoordsLatLng = [];
            $.each(item.Path, function (ip, itemp)
            {
                trackCoordsLatLng.push(new window.google.maps.LatLng(itemp.Latitude, itemp.Longitude));
            });

            addPolyline(trackCoordsLatLng, id === item.Id ? "green" : "");
            addMarker(item.Start, "Start " + item.Name + " - " + item.DateText, "start", item.Id, key);
            addMarker(item.End, "End " + item.Name + " - " + item.DateText, "end", item.Id, key);
        });

        SetZoomAndCenter(shareTrackInfo.Center, getZoom(shareTrackInfo.Min, shareTrackInfo.Max));
    });
}