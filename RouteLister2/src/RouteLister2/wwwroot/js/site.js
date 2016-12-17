var external = (function () {



    var stop = function (event) {
        event.stopPropagation();
        event.preventDefault();
    }

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
        stop: stop
    };


})();


//One should only have to use id to remove a particular item
var item = function (IdRef, urlAction, cref) {

    //The idPrefix
    this.ref = IdRef;
    //The combined idReference
    this.thisRef = "#" + IdRef;
    //Url action to get a particular viewmodel
    this.url = urlAction;
    //Reference to the container containing the viewmodel/viewmodels
    this.containerRef = "#" + cref;

};

//Remove a specific viewmodel
item.prototype.remove = function (idToRemove) {
    var current = this;
    //remove from server
    //....
    //$.ajax({
    //    type: "GET",
    //    url: current.url + "/Delete/" + idToUpdate
    //}).done(function (data) {
    //    $("#" + current.thisRef + idToUpdate).html(data);
    //}).fail(function (data) {

    //}).always(function (data) {

    //});
    //remove from client-gui
    //...
    $(current.thisRef + idToRemove).remove();
    


};
//Update a specfic viewModel(overwrite it)
item.prototype.update = function (idToUpdate) {
    //This becomes a reference in ajax so can't use ajax variables in a ajax call
    var current = this;
    $.ajax({
        type: "GET",
        url: current.url + "/Edit/" + idToUpdate
    }).done(function (data) {
        $("#" + current.thisRef + idToUpdate).html(data);
    }).fail(function (data) {

    }).always(function (data) {

    });
};
//Get a viewModel
item.prototype.get = function (idToGet) {

    var current = this;
    $.ajax({
        type: "GET",
        url: current.url + "/" + idToGet

    }).done(function (data) {
        return data;
    }).fail(function (data) {

    }).always(function (data) {

    });
};
item.prototype.add = function (idToGet) {
    var current = this;
    $.ajax({
        type: "GET",
        url: current.url + "/" + idToGet

    }).done(function (data) {
        $(cont).append(data);
    }).fail(function (data) {

    }).always(function (data) {

    });
};


