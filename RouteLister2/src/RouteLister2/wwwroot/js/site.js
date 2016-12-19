var external = (function () {



    var stop = function (event) {
        event.stopPropagation();
        event.preventDefault();
    }
    return {
        stop: stop
    };

    var loadingScreen = (function () {


        var show = function () {
            var bodyCss = external.css($('.body-content'));
            var topValue = $('.navbar-fixed-top').height();
            var height = $('.body-content').height();
            var width = $('.body-content').width();
            var loadingDiv = $('#loadingDiv');
            loadingDiv.css(bodyCss);
            loadingDiv.css('top', topValue);
            loadingDiv.width(width);
            loadingDiv.height(height);
            loadingDiv.css('position', 'absolute');
            loadingDiv.removeClass("hidden");
        };
        var hide = function () {
            $('#loadingDiv').addClass("hidden");
        };
        return {
            show: show,
            hide: hide
        };
    })();

    function css(a) {
        var sheets = document.styleSheets, o = {};
        for (var i in sheets) {
            var rules = sheets[i].rules || sheets[i].cssRules;
            for (var r in rules) {
                if (a.is(rules[r].selectorText)) {
                    o = $.extend(o, external.css2json(rules[r].style), external.css2json(a.attr('style')));
                }
            }
        }
        return o;
    };

    function css2json(css) {
        var s = {};
        if (!css) return s;
        if (css instanceof CSSStyleDeclaration) {
            for (var i in css) {
                if ((css[i]).toLowerCase) {
                    s[(css[i]).toLowerCase()] = (css[css[i]]);
                }
            }
        } else if (typeof css == "string") {
            css = css.split("; ");
            for (var f in css) {
                var l = css[f].split(": ");
                s[l[0].toLowerCase()] = (l[1]);
            }
        }
        return s;
    };
    function startTime() {
        var today = new Date();
        var h = today.getHours();
        var m = today.getMinutes();
        var s = today.getSeconds();
        m = checkTime(m);
        s = checkTime(s);
        var clock = document.getElementById('clock');
        if (clock != null) {
            clock.innerHTML = h + ":" + m + ":" + s;
            var t = setTimeout(startTime, 500);
        }
    }
    function checkTime(i) {
        if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
        return i;
    }
    return {
        css: css,
        loadingScreen: loadingScreen,
        css2json: css2json,
        startTime: startTime,
        stop:stop
    }


})();


//*****/* SIDENAV JQUERY *\*********//
function openNav() {
    document.getElementById("mySidenav").style.width = "400px";
}
function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}
//*****/* END SIDENAV JQUERY *\*********//

//*****/* PAGE PRELOADER *\*********//
//// Wait for window load
$(window).load(function () {
    $(".loader").fadeOut("slow").domManip.ready;
});

$(".download-spinner").hide();
// SPINNER FOR DATADOWNLOAD \\
function ShowProgress() {
    setTimeout(function () {
        var loading = $(".download-spinner");
        loading.show();
    },  $(document).ready(function () {
        $(".download-spinner").fadeOut("slow");        
    })
    )
}

//*****/* END PAGE PRELOADER *\*********//


//*****/* GOOGLE MAP *\*********//
//
// This requires the Places library. Include the libraries=places
// parameter when you first load the API. For example:
// <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAX19N6_xtYwKuIBgNgfqWvCoH6yqIZm8E&libraries=places">
var map;
var directionsDisplay;
var directionsService;
function initMap() {
     directionsDisplay = new google.maps.DirectionsRenderer;
     directionsService = new google.maps.DirectionsService;
     navigator.geolocation.getCurrentPosition(
         function (position) {
             var geolocate = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
             //console.log(geolocate);
             map = new google.maps.Map(document.getElementById('map_canvas'), {
                 mapTypeControl: true,
                 //position: geolocate, /* must be outcomment otherwise no waypointdata shows on map*/
                 center: { lat: position.coords.latitude, lng: position.coords.longitude },
                 zoom: 13
             });

             var latitude = position.coords.latitude;
             var longitude = position.coords.longitude;
             var geocoder = new google.maps.Geocoder;
             var infowindow = new google.maps.InfoWindow;
             //console.log("Kartdata:\n", map);

             new geocodeLatLng(geocoder, map, infowindow, latitude, longitude);
         })
}


function geocodeLatLng(geocoder, map, infowindow, latitude, longitude) {
    var placeId;
    var address;
    var input = latitude + "," + longitude;
    //console.log("Input value latitude, longitude: n\{0}", input)
    var latlngStr = input.split(',', 2);
    var latlng = { lat: parseFloat(latlngStr[0]), lng: parseFloat(latlngStr[1]) };
    geocoder.geocode({ 'location': latlng }, function (results, status)
    {
        if (status === 'OK')
        {
            if (results[0])
            {
                map.setZoom(11);
                var marker = new google.maps.Marker({
                    position: latlng,
                    map: map
                });
                // sets positioninfo to map NOT needed becaus data sets to textbox value
                //infowindow.setContent(results[0].formatted_address);
                //infowindow.open(map, marker);
            } else
            {
                window.alert('Destination kunde inte hittas');
            }
        } else
        {
            window.alert('Ett fel uppstod: \n. Vänligen kontrollera destinationen:' + status);
        }
        placeId = results[0].place_id;
        address = results[0].formatted_address
        //console.log(placeId);
        //console.log(address);
        //console.log("Adressdata:", marker.position);
        //console.log(results[0])

        // sets actual position to textboxvalue do not move otherwiser no data gets to function
        document.getElementById('origin-input').value = address;
        new AutocompleteDirectionsHandler(map, placeId, address);
    });

}

function AutocompleteDirectionsHandler(map, placeId, address) {
    this.map = map;
    this.originPlaceId = null; 
    this.destinationPlaceId = null;
    this.travelMode = 'DRIVING';
    var originInput = document.getElementById('origin-input');
    var destinationInput = document.getElementById('destination-input');
    var modeSelector = document.getElementById('mode-selector');
    this.directionsService = new google.maps.DirectionsService;
    this.directionsDisplay = new google.maps.DirectionsRenderer;
    this.directionsDisplay.setMap(map);

    var originAutocomplete = new google.maps.places.Autocomplete(
        originInput, { placeIdOnly: true });

    var destinationAutocomplete = new google.maps.places.Autocomplete(
        destinationInput, { placeIdOnly: true });

    //// 
    //this.setupClickListener('changemode-walking', 'WALKING');
    //this.setupClickListener('changemode-transit', 'TRANSIT');
    //this.setupClickListener('changemode-driving', 'DRIVING');

    this.setupPlaceChangedListener(originAutocomplete, 'ORIG');
    this.setupPlaceChangedListener(destinationAutocomplete, 'DEST');

    this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(originInput);
    this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(destinationInput);
    this.map.controls[google.maps.ControlPosition.LEFT].push(modeSelector);

    new calculateAndDisplayRoute();
 

}


// Sets a listener on a radio button to change the filter type on Places
// Autocomplete.
//AutocompleteDirectionsHandler.prototype.setupClickListener = function (id, mode) {
//    var radioButton = document.getElementById(id);
//    var me = this;
//    radioButton.addEventListener('click', function () {
//        me.travelMode = mode;
//        me.route();
//    });
//};

AutocompleteDirectionsHandler.prototype.setupPlaceChangedListener = function (autocomplete, mode) {
    var me = this;
    autocomplete.bindTo('bounds', this.map);
    autocomplete.addListener('place_changed', function () {
        var place = autocomplete.getPlace();
        if (!place.place_id) {
            window.alert("Du har inte valt någon adress.");
            return;
        }
        if (mode === 'ORIG') {
            me.originPlaceId = place.place_id;
        } else {
            me.destinationPlaceId = place.place_id;
        }
        me.route();
    });

};

AutocompleteDirectionsHandler.prototype.route = function () {
    if (!this.originPlaceId || !this.destinationPlaceId) {
        return;
    }
    var me = this;

    this.directionsService.route({
        origin: { 'placeId': this.originPlaceId },
        destination: { 'placeId': this.destinationPlaceId },
        travelMode: this.travelMode
    }, function (response, status) {
        if (status === 'OK') {
            me.directionsDisplay.setDirections(response);
        } else {
            window.alert('Ett fel uppstod: \n. Vänligen kontrollera destinationen:' + status);
        }
    });
};

function calculateAndDisplayRoute() {
    directionsDisplay.setMap(map);
    directionsDisplay.setPanel(document.getElementById('right-panel'));
    // SETS UP THE TRIPDETAIL IN RIGHT-PANEL    
    var onChangeHandler = function () {
        calculateAndDisplayRoute(directionsService, directionsDisplay);
    };
    document.getElementById('origin-input').addEventListener('change', onChangeHandler);
    document.getElementById('destination-input').addEventListener('change', onChangeHandler);
    var start = document.getElementById('origin-input').value;
    var end = document.getElementById('destination-input').value;
    directionsService.route({
        origin: start,
        destination: end,
        travelMode: 'DRIVING'
    },
    function (response, status) {
        if (status === 'OK') {
            directionsDisplay.setDirections(response);
        } else {
            window.alert('Ett fel uppstod: \n. Vänligen kontrollera destinationen:' + status);
        }
    });
}
//*****/* END GOOGLE MAP *\*********//