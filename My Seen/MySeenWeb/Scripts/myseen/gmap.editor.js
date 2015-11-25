//http://gmap3.net/en/pages/19-demo/
//var lat=$map.gmap3('getLatlng').lat();
//var lon = $map.gmap3('getLatlng').lon();

var current, m1, m2;

$(document).ready(function () {
    var menu = new Gmap3Menu($("#my_map"));
      //current,  // current click event (used to save as start / end position)
      //m1,       // marker "from"
      //m2;       // marker "to"

    menu.add("Set Start", "itemA",
        function() {
            menu.close();
            addMarker(false);
        });

    menu.add("Set Between", "itemA",
    function () {
        menu.close();
        addMarker(false);
    });


    menu.add("Set End", "itemA",
        function () {
            menu.close();
            addMarker(false);
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
                    console.log("rightclick");
                    menu.open(current);
                },
                click: function () {
                    console.log("click");
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
function addMarker(isM1) {
    // clear previous marker if set
    var clear = { name: "marker" };
    if (isM1 && m1) {
        clear.tag = "from";
        $("#my_map").gmap3({ clear: clear });
    } else if (!isM1 && m2) {
        clear.tag = "to";
        $("#my_map").gmap3({ clear: clear });
    }
    // add marker and store it
    $("#my_map").gmap3({
        marker: {
            latLng: current.latLng,
            options: {
                draggable: true,
                icon: new google.maps.MarkerImage("http://maps.gstatic.com/mapfiles/icon_green" + (isM1 ? "A" : "B") + ".png")
            },
            tag: (isM1 ? "from" : "to"),
            events: {
                dragend: function (marker) {
                    updateMarker(marker, isM1);
                }
            },
            callback: function (marker) {
                updateMarker(marker, isM1);
            }
        }
    });
}

// update marker
function updateMarker(marker, isM1) {
    if (isM1) {
        m1 = marker;
    } else {
        m2 = marker;
    }
    updateDirections();
}

// function called to update direction is m1 and m2 are set
function updateDirections() {
    if (!(m1 && m2)) {
        return;
    }
    $("#my_map").gmap3({
        getroute: {
            options: {
                origin: m1.getPosition(),
                destination: m2.getPosition(),
                travelMode: google.maps.DirectionsTravelMode.DRIVING
            },
            callback: function (results) {
                if (!results) return;
                $("#my_map").gmap3({ get: "directionrenderer" }).setDirections(results);
            }
        }
    });
}