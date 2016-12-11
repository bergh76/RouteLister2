//TODO remove everything not used
var admin = (function () {

    var orderRow = (function () {
        var NameId;
        var setNameId = function (name) {
            NameId = name;
        };
        //Request signalr server to change status on a order
        var setReg = function (event, referens) {
            //Always stop the event
            external.stop(event);
            //Get id to change, lazymode here. not a good method to generate a unique id.
            var partOfId = referens.attr("id").replace("changeRegNrButton", "");
            var form = $("#" + "form" + partOfId);
            $.ajax({
                type: form.attr("method"),
                url: form.attr("action"),
                data: form.serialize(),
                success: function (data) {
                    $("#" + "row" + partOfId).html(data);
                    //Adding listener again to changed row
                    $(data).find(".changeRegNrButton").click(function (event) {
                        admin.orderRow.setRegistrationNumber(event, $(this));
                    });
                },
                error: function (data) {
                    console.log("something went wong");
                }
            });
        };

        return {
            setRegistrationNumber: setReg,
        };
    })();

    return {
        orderRow: orderRow
    };
})();





