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

 //Note: This example requires that you consent to location sharing when
 //prompted by your browser. If you see the error "The Geolocation service
 //failed.", it means you probably did not give permission for the browser to
 //locate you.

//function initMap() {
//    var infoWindow = new google.maps.({ map: map });

//    // Try HTML5 geolocation.
//    if (navigator.geolocation) {
//        navigator.geolocation.getCurrentPosition(function (position) {
//            var pos = {
//                lat: position.coords.latitude,
//                lng: position.coords.longitude
//            };
//            //var script = document.createElement('script');

//            infoWindow.setPosition(pos);
//            infoWindow.setContent('Location found.');
//            map.setCenter(pos);
//            document.getElementsByTagName('head')[0].appendChild(pos);

//        }, function () {
//            handleLocationError(true, infoWindow, map.getCenter());

//        });
//    } else {
//        // Browser doesn't support Geolocation
//        handleLocationError(false, infoWindow, map.getCenter());
//    }

//    // Checks that the PlacesServiceStatus is OK, and adds a marker
//    // using the place ID and location from the PlacesService.
//    function callback(results, status) {
//        if (status == google.maps.places.PlacesServiceStatus.OK) {
//            var marker = new google.maps.Marker({
//                map: map,
//                place: {
//                    placeId: results[0].place_id,
//                    location: results[0].geometry.location
//                }
//            });
//            console.log(marker)
//        }

//        function handleLocationError(browserHasGeolocation, infoWindow, pos) {
//            infoWindow.setPosition(pos);
//            infoWindow.setContent(browserHasGeolocation ?
//            'Error: The Geolocation service failed.' :
//            'Error: Your browser doesn\'t support geolocation.');
//        }
//    }
//}
(function () {

    if (!!navigator.geolocation) {
        var map;
        var latitude;
        var longitude;
        var mapOptions = {
            zoom: 15,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
        //navigator.geolocation.watchPosition(
        //    (function (position) {

            navigator.geolocation.getCurrentPosition(function (position) {

            var geolocate = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);

            var infowindow = new google.maps.InfoWindow({
                map: map,
                position: geolocate,
                latitude: $('#latitude').val(position.coords.latitude),
                longitude: $('#longitude').val(position.coords.longitude),
                content:
                    '<span><i class="fa fa-pin"></i></span>' +
                    '<span>Latitude: ' + position.coords.latitude + '</span></br>' +
                    '<span>Longitude: ' + position.coords.longitude + '</span>'
            });
            map.setCenter(geolocate);
            
            })
        ;

    } else {
        document.getElementById('map_canvas').innerHTML = 'No Geolocation Support.';
    }

})();