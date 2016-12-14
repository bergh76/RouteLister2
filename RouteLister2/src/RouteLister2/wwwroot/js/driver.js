



var routeLister = (function () {
    //Token
    const regexForidToken = /^.*-_:\.Id(.{1,10})$/g;
    const regexForidToken2 = "^.*" + idPrefixRegex + "(.{1,10})$/g";
    //Change if you want prefix for id to be something else
    //const idPrefix = '-_:\.Id';
    const idRegex = new RegExp(regexForidToken2);


    var connectionStatus = false;
    var tryingToReconnect = false;
    //Stop event from happening and bubbling
    var stopClient = function (event) {
        event.stopPropagation();
        event.preventDefault();
    };
    //Request signalr server to change status on a order
    var _connectionId;

    var setConnectionId = function (connectionId) {
            routeLister._connectionId = connectionId;
            routeLister.setConnectionStatus.client(routeLister._connectionId !== undefined);   
    };
    var setConnectionStatus = (function () {
        var changeStatus = function (status) {
            //Determine client status(What the client sees right now
            //Clients representation of online
            var clientGraphicsStatus = true;
            var onlineCircle = $(".online");
            var offlineCircle = $(".offline-dim");
            if (onlineCircle.length === 0 ) {
                onlineCircle = $(".online-dim");
                clientGraphicsStatus = false;
            }
            if (offlineCircle.length === 0) {
                offlineCircle = $(".offline");
                clientGraphicsStatus = false;
            }
            var statusText = $("#status-text");
            //If status is true = online
            if (status) {
                if (!clientGraphicsStatus) {
                    onlineCircle.addClass("online");
                    onlineCircle.removeClass("online-dim");
                    offlineCircle.addClass("offline-dim");
                    offlineCircle.removeClass("offline");
                    statusText.text("[online");
                }
            }
            else {
                if (clientGraphicsStatus) {
                    onlineCircle.removeClass("online");
                    onlineCircle.addClass("online-dim");
                    offlineCircle.removeClass("offline-dim");
                    offlineCircle.addClass("offline");
                    statusText.text("[offline");
                }
            }

        };
        var disconnectFromServer = function(){
            routeLister.client(false);
            signalRClient.setConnectionStatus(false);
        };
        return {
            client : changeStatus,
            server : disconnectFromServer
        };
    })();
 
    
    var routeList = (function (routeListId) {
        //Since its one routelist per driver, reloading whole page
        var reload = (function () {
            //Reloading page
            location.reload();
        });
        return {
            refresh : reload
        };
    })();
    //Regex to get id from a reference.
    //The number at the end of a id reference is the id(
    var getValueFromIdRef = function (idRef) {
        var id = routeLister.idRegex.match(idRef);
        return id;
    };
    var orderRow = (function () {
        var NameId;
        var setNameId = function (name) {
            NameId = name;
        };
        //Request signalr server to change status on a order
        var requestOrderRowStatusChange = function (event) {
            //Always stop the event
            routeLister.stop(event);
            if (routeLister._connectionId) {
                $(event.target).prop('disabled', true);
                if (event.type === "click") {
                    //Remove all listeners
                    $(event.target).unbind("click");
                    var idValue = $(event.target).val();
                    //console.log(idValue);
                    var checkBoxId = $(event.target).attr("id");
                    //console.log(checkBoxId);
                    ///sets the idName that client funktion uses + token to change later on
                    //Gets id number from id reference
                    //var id = routeLister.getId(orderRowId);
                    NameId = "#" + checkBoxId;
                    //Asks server to change status on orderRow
                    signalRClient.server.changeStatusOnOrderRow(idValue, checkBoxId);
                    //Show waiting message for client while server works
                    var spinner = $(event.target).parents().siblings('.fa-spin');
                    spinner.removeClass("hidden");
                    //TODO
                }
            }
        };
        //Clientside change
        var changeClientSideView = function (orderRowId, checked) {
            var checkbox = $("#" + orderRowId);
            console.log("Value: " + checkbox.val());
            //what is it set to right now?
            checkbox.prop("checked", checked);
            bool = checkbox.prop("checked");
            console.log("box is checked?: " + bool);
            //Remove/hide server animation/message
            //TODO
            //Focus on changed item
            checkbox.focus();
            //Add click listener again
            checkbox.click(function (event) {
                routeLister.orderRow.server(event);
            });
            //Enable checkbox again
            $(checkbox).prop('disabled', false);
            var spinner = $(checkbox).parents().siblings('.fa-spin');
            spinner.addClass("hidden");
        };

        //Regex to get id from a reference.
        //The number at the end of a id reference is the id(

        return {
            server: requestOrderRowStatusChange,
            client: changeClientSideView,
            setNameId: setNameId
        };
    })();
    var order = (function () {
        //Request signalr server to change status on a order
        var requestOrderRowStatusChange = function (orderRowId) {

        };
        //Clientside change
        var changeClientSideView = function (orderRowStatusId) {

        };
        return {
            server: requestOrderRowStatusChange,
            client: changeClientSideView
        };
    })();


  
    return {
        stop: stopClient,
        token: idPrefix,
        idRegex: regexForidToken2,
        orderRow: orderRow,
        order: order,
        routeList: routeList,
        getId: getValueFromIdRef,
        setConnectionStatus: setConnectionStatus,
        setConnectionId: setConnectionId,
        _connectionId : _connectionId,
        tryingToReconnect: tryingToReconnect

    };

})();




//SignalR setting
var signalRClient = $.connection.driverHub;
//Regex used to extract idToken

$.connection.hub.logging = true;
external.loadingScreen.show();
$.connection.hub.start().done(function () {
    console.log("connected");
    signalRClient.server.connect();
    //should enable all buttons
    external.loadingScreen.hide();
}).fail(function (error) {
    console.log('Invocation of start failed. Error:' + error);
    //TODO show error msg
});
$.connection.hub.reconnecting(function () {
    routeLister.tryingToReconnect = true;
});
$.connection.hub.reconnected(function () {
    routeLister.tryingToReconnect = false;
});
$.connection.hub.disconnected(function () {
    if (routeLister.tryingToReconnect) {
        routeLister.setConnectionStatus.client(false);
    }
    setTimeout(function () {
        $.connection.hub.start().done(function () {
            console.log("connected");
            //should enable all buttons
        }).fail(function (error) {
            console.log('Invocation of start failed. Error:' + error);
            //TODO show error msg
        });
    }, 5000);
});


signalRClient.client.changeClientRowStatus = function (id, checked) {
    console.log("changestatus triggered");
    routeLister.orderRow.client(id, checked);
};



signalRClient.client.setConnectionId = function (connectionId) {
    routeLister.setConnectionId(connectionId);
    console.log("connectId set!");
};
signalRClient.client.enableEverything = function () {
    //Enable all gui touchy touchy

};
signalRClient.client.disableEverything = function () {
    //Enable all gui touchy touchy
};

signalRClient.client.newRouteListAdded = function () {
    routeLister.routeList.refresh();
};

signalRClient.client.AddedOrder = function () {
    routeLister.order.add();
};



