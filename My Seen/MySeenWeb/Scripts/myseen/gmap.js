var Language;//en/ru
var Markers;

function initialGmap(language,markers) {
    Language = language;
    Markers = markers;

    $("#my_map").gmap3({
        map: {
            options: {
                zoom: 2,
                mapTypeId: window.google.maps.MapTypeId.ROADMAP
            }
        }
    });
}

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

function addMarker(markerCoords, data, icon, id, key) {

    if (Markers==="False") return;

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
                        //console.log("key="+key);
                        if (key) showTrackByKey(key, id);
                        else showTrack(id, true, "green");
                    }
                }
            }
        }
    });
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
        
        addMarker(trackInfo.Start, trackInfo.Name + " - " + trackInfo.DateText, "start", trackInfo.Id);
        addMarker(trackInfo.End, trackInfo.Name + " - " + trackInfo.DateText, "end", trackInfo.Id);
    });
}

function showTracks(id, key, shareTrackInfo) {
    clearMap();

    $.each(shareTrackInfo.Data, function (i, item) {
        var trackCoordsLatLng = [];
        $.each(item.Path, function (ip, itemp) {
            trackCoordsLatLng.push(new window.google.maps.LatLng(itemp.Latitude, itemp.Longitude));
        });

        addPolyline(trackCoordsLatLng, id === item.Id ? "green" : "");
        addMarker(item.Start, "Start " + item.Name + " - " + item.DateText, "start", item.Id, key);
        addMarker(item.End, "End " + item.Name + " - " + item.DateText, "end", item.Id, key);
    });

    SetZoomAndCenter(shareTrackInfo.Center, getZoom(shareTrackInfo.Min, shareTrackInfo.Max));
}

function showTrackByKey(id, key) {
    $.getJSON('/Home/GetTrackByKey/' + key + '/', function (shareTrackInfo) {
        showTracks(id, key, shareTrackInfo);
    });
}

function showTrackByType(id, type) {
    $.getJSON('/Home/GetTrackByType/' + type + '/', function (shareTrackInfo) {
        showTracks(id, type, shareTrackInfo);
    });
}