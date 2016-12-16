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



//function calculateAndDisplayRoute(directionsService, directionsDisplay) {
//    var start = document.getElementById('start').value;
//    var end = document.getElementById('end').value;
//    directionsService.route({
//        origin: start,
//        destination: end,
//        travelMode: 'DRIVING'
//    }, function (response, status) {
//        if (status === 'OK') {
//            directionsDisplay.setDirections(response);
//        } else {
//            window.alert('Directions request failed due to ' + status);
//        }
//    });
//}
(function () {
    //var key = "AIzaSyAX19N6_xtYwKuIBgNgfqWvCoH6yqIZm8E";
    if (!!navigator.geolocation) {
        var map;
        var latitude;
        var longitude;
        var mapOptions = {
            //zoom: 15,
            mapTypeId: google.maps.MapTypeId.ROA
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