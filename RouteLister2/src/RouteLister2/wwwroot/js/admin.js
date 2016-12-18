//TODO remove everything not used
var admin = (function () {

    var orderRow = (function () {
        var NameId;
        var setNameId = function (name) {
            NameId = name;
        };
        //Request signalr server to change status on a order
        var setReg = function (event, referens) {
            //if (event != undefined && event!=null) {
            //Always stop the event
            external.stop(event);
            var next = $($(event.target).parents()[1]);
            var yousuck = next.prev().find(".dropdown");
            var valz = yousuck.val();
            var OrderId = next.prev().find("#OrderId").val();
            //}
            //Get id to change, lazymode here. not a good method to generate a unique id.

            var partOfId = referens.attr("id").replace("changeRegNrButton", "");
            var form = $("#" + "form" + partOfId);
            $.ajax({
                type: form.attr("method"),
                url: form.attr("action"),
                data: form.serialize(),
                success: function (data) {
                    $("." + "row" + OrderId).find(".dropdown").each(function (index, element) {
                        //console.log(element);
                        //console.log($(element));
                        $(element).val(valz).change();
                    });
                },
                error: function (data) {
                    console.log("something went wong");
                    console.log(data);
                }
            });
        };


        return {
            setRegistrationNumber: setReg
        };
    })();

    return {
        orderRow: orderRow
    };
})();





