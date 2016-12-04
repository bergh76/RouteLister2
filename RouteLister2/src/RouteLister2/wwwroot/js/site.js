// Write your Javascript code.

var routeLister = (function () {
    //Token
    const regexForidToken = /^.*-_:\.Id(.{1,10})$/g;
    const regexForidToken2 = "^.*" + idPrefixRegex + "(.{1,10})$/g";
    //Change if you want prefix for id to be something else
    //const idPrefix = '-_:\.Id';
    const idRegex = new RegExp(regexForidToken2);

    //Stop event from happening and bubbling
    var stopClient = function (event) {
        event.stopPropagation();
        event.preventDefault();
    };
    //Request signalr server to change status on a order


    var routeList = (function (routeListId) {


    })();
    //Regex to get id from a reference.
    //The number at the end of a id reference is the id(
    var getValueFromIdRef = function (idRef) {
        var id = routeLister.idRegex.match(idRef);
        return id;
    };
    var orderRow = (function () {

        var NameId;
        var setNameId = function(name){
            NameId = name;
        };
        //Request signalr server to change status on a order
        var requestOrderRowStatusChange = function (event) {

            routeLister.stop(event);

            var idValue = $(event.target).val();
            console.log(idValue);
            var checkBoxId = $(event.target).attr("id");
            console.log(checkBoxId);
            ///sets the idName that client funktion uses + token to change later on
            //Gets id number from id reference
            //var id = routeLister.getId(orderRowId);
            NameId = "#" + checkBoxId;
            //Asks server to change status on orderRow
            signalRClient.server.changeStatusOnOrderRow(idValue);
            //Show waiting message for client while server works
            //TODO
        };
        //Clientside change
        var changeClientSideView = function (event, orderRowId) {

            var htmlId = NameId;
            //what is it set to right now?
            var bool = !$("#" + htmlId).checked === undefined;
            //if not checked, set to checked
            if (bool) {
                $(htmlId).prop("checked", "checked");
            }
                //if checked, uncheck
            else {
                $(htmlId).removeAttr("checked");
            }
            //Remove/hide server animation/message
            //TODO
            //Focus on changed item
            $(htmlId).focus();
        };

        //Regex to get id from a reference.
        //The number at the end of a id reference is the id(

        return {
            server: requestOrderRowStatusChange,
            client: changeClientSideView,
            setNameId : setNameId
        };
    })();

    var order = (function () {
        //Request signalr server to change status on a order
        var requestOrderRowStatusChange = function (orderRowId) {
            //Gets id number from id reference
            var id = routeLister.getId(orderRowId);
            //Asks server to change status on orderRow
            signalRClient.server.changeStatusOnOrderRow(id);
            //Show waiting message for client while server works
            //TODO
        };
        //Clientside change
        var changeClientSideView = function (event, orderRowStatusId) {
            //what is it set to right now?
            var bool = !$("#" + orderRowStatusId).checked === undefined;
            //if not checked, set to checked
            if (bool) {
                $(orderRowStatusId).prop("checked", "checked");
            }
                //if checked, uncheck
            else {
                $(orderRowStatusId).removeAttr("checked");
            }
            //Remove/hide server animation/message
            //TODO
            //Focus on changed item
            $(orderRowStatusId).focus();
        };


        return {
            server: requestOrderRowStatusChange,
            client: changeClientSideView
        };
    })();


    return {
        stop : stopClient,
        token : idPrefix,
        idRegex : regexForidToken2,
        orderRow : orderRow,
        order : order,
        routeList : routeList,
        getId : getValueFromIdRef

    };

})();
