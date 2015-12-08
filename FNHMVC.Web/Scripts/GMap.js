// Inicialización de variables.
//var map = null;
//var geocoder = null;


//$(document).ready(function () {
//    load();
//});


$(document).ready(function () {
    initMap();
    //$("#accordion2").hide();
    //$("#coordenadas").hide();
    //$("#TookItHome").click(function () {
    //    $("#accordion2").toggle("slow");
    //    $("#coordenadas").toggle("slow");
    //});

});

var map;
var markersArray = [];

function initMap() {
    var Latitude = document.getElementById("Latitude").value;
    var Altitude = document.getElementById("Altitude").value;

    if (Latitude == '' || Latitude == null) {
        Latitude = '15.452268357024213';
    }

    if (Altitude == '' || Altitude == null) {
        Altitude = '-88.04443359375';
    }

    var latlng = new google.maps.LatLng(Latitude, Altitude);
    var myOptions = {
        zoom: 7,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map"), myOptions);
    placeMarker(latlng);


    // add a click event handler to the map object
    google.maps.event.addListener(map, "click", function (event) {
        // place a marker
        placeMarker(event.latLng);

        // display the lat/lng in your form's lat/lng fields
        document.getElementById("Latitude").value = event.latLng.lat();
        document.getElementById("Altitude").value = event.latLng.lng();
    });
}
function placeMarker(location) {
    // first remove all markers if there are any
    deleteOverlays();

    var marker = new google.maps.Marker({
        position: location,
        map: map
    });

    // add marker in markers array
    markersArray.push(marker);

    //map.setCenter(location);
}

// Deletes all markers in the array by removing references to them
function deleteOverlays() {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }
}

function initialize() {
    var mapOptions = {
        center: new google.maps.LatLng('15.452268357024213', '-88.04443359375'),
        zoom: 7,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById("map"),
      mapOptions);
    // create a marker  
    var latlng = new google.maps.LatLng('15.452268357024213', '-88.04443359375');
    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        title: 'Latitude: ' + latlng.Ya + ' Longitude :' + latlng.Za,
        draggable: false
    });

    geocoder = new google.maps.Geocoder();
    google.maps.event.addListener(map, 'click', function (point) {

        map.panTo(marker.getPosition());
    });


    //Update postal address when the marker is dragged  
    //google.maps.event.addListener(marker, 'click', function () { //dragend  

    //    geocoder.geocode({ latLng: marker.getPosition() }, function (responses) {
    //        if (responses && responses.length > 0) {
    //            $("#Coor").text("Latitude: " + marker.getPosition().lat() + "&nbsp" + "Longitude: " + marker.getPosition().lng());
    //            infoWindow.setContent(
    //            "<div style=\"font-size:smaller;\">" + responses[0].formatted_address
    //            + "<br />"
    //            + "Latitude: " + marker.getPosition().lat() + "&nbsp"
    //            + "Longitude: " + marker.getPosition().lng() + "</div>"
    //            );
    //            infoWindow.open(map, marker);
    //        } else {
    //            alert('Error: Google Maps could not determine the address of this location.');
    //        }
    //    });
    //    map.panTo(marker.getPosition());
    //});
}




//function load() {                                      // Abre LLAVE 1.
//    if (GBrowserIsCompatible()) {                                                // Abre LLAVE 2.
//        map = new GMap2(document.getElementById("map"));

//        map.setCenter(new GLatLng(15.452268357024213, -88.04443359375), 7);
//        map.addControl(new GSmallMapControl());
//        map.addControl(new GMapTypeControl());

//        geocoder = new GClientGeocoder();

//        //---------------------------------//
//        //   MARCADOR AL HACER CLICK
//        //---------------------------------//
//        GEvent.addListener(map, "click",
//    function (marker, point) {
//        if (marker) {
//            null;
//        } else {
//            map.clearOverlays();
//            var marcador = new GMarker(point);
//            map.addOverlay(marcador);
//            //marcador.openInfoWindowHtml("<b><br>Coordenadas:<br></b>Latitud : "+point.y+"<br>Longitud : "+point.x+"<a href=http://www.mundivideo.com/fotos_pano.htm?lat="+point.y+"&lon="+point.x+"&mapa=3 TARGET=fijo><br><br>Fotografias</a>");
//            //marcador.openInfoWindowHtml("<b>Coordenadas:</b> "+point.y+","+point.x);
//            document.form_mapa.coordenadas.value = point.y + "," + point.x;
//        }
//    }
//    );
//        //---------------------------------//
//        //   FIN MARCADOR AL HACER CLICK
//        //---------------------------------//

//    } // Cierra LLAVE 1.
//}   // Cierra LLAVE 2.

////---------------------------------//
////           GEOCODER
////---------------------------------//
//function showAddress(address, zoom) {
//    if (geocoder) {
//        geocoder.getLatLng(address,
//        function (point) {
//            if (!point) {
//                alert(address + " not found");
//            } else {
//                map.setCenter(point, zoom);
//                var marker = new GMarker(point);
//                map.addOverlay(marker);
//                //marker.openInfoWindowHtml("<b>"+address+"</b><br>Coordenadas:<br>Latitud : "+point.y+"<br>Longitud : "+point.x+"<a href=http://www.mundivideo.com/fotos_pano.htm?lat="+point.y+"&lon="+point.x+"&mapa=3 TARGET=fijo><br><br>Fotografias</a>");
//                // marker.openInfoWindowHtml("<b>Coordenadas:</b> "+point.y+","+point.x);
//                document.form_mapa.coordenadas.value = point.y + "," + point.x;
//            }
//        }
//    );
//    } 
//}
//---------------------------------//
//     FIN DE GEOCODER
//---------------------------------//