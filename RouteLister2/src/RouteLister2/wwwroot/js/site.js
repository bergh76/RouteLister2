var external = (function () {
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
            loadingDiv.css('position','absolute');
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
            for (var i in css) {
                var l = css[i].split(": ");
                s[l[0].toLowerCase()] = (l[1]);
            }
        }
        return s;
    };

    return {
        css: css,
        loadingScreen: loadingScreen,
        css2json: css2json
    }
})();