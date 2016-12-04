var signalRClient = $.connection.driverHub;
//Regex used to extract idToken

$.connection.hub.logging = true;
$.connection.hub.start().done(function () {
    var connected = signalRClient.server.connect();
}).fail(function (error) {
    console.log('Invocation of start failed. Error:' + error);
});
//signalRClient.start().done(function () {
//    var connected = signalRClient.server.connect();
//})

//signalRClient.client.messageReceived = function (originatorUser, message) {
//    $("#messages").append('<li><strong>' + originatorUser + '</strong>: ' + message);
//};

//signalRClient.client.getConnectedUsers = function (userList) {
//    for (var i = 0; i < userList.length; i++)
//        addUser(userList[i]);
//};

//signalRClient.client.newUserAdded = function (newUser) {
//    addUser(newUser);
//}

signalRClient.client.changeOrderRowStatus = function (id) {

    routeLister.orderRow.client(id);
    
};
//var orderRow = (function () {
//    //Request signalr server to change status on a order
//    var requestOrderRowStatusChange = function (orderRowId) {
//        var value = $("#" + orderRowId).val();
//        signalRClient.server.changeStatusOnOrderRow(orderRowId);
//        //Show waiting for server animation/message underneath
//        //TODO
//    };
//    //Clientside change
//    var changeClientSideView = function (event, orderRowStatusId) {
//        //what is it set to right now?
//        var bool = !$("#"+orderRowStatusId).checked === undefined;
//        //if not checked, set to checked
//        if (bool) {
//            $(orderRowStatusId).prop("checked", "checked");
//        }
//        //if checked, uncheck
//        else {
//            $(orderRowStatusId).removeAttr("checked");
//        }
//        //Remove/hide server animation/message
//        //TODO
//        //Focus on changed item
//        $(orderRowStatusId).focus();
//    };

//    //Regex to get id from a reference.
//    //The number at the end of a id reference is the id(
//    var getValueFromIdRef = function (idRef) {
//        var id = routeLister.idRegex.match(idRef);
//        return id;
//    };
//    return {
//        server : requestOrderRowStatusChange,
//        client: changeClientSideView
//    };
//})();
//function changeOrderRowStatus(orderRowStatusId, orderRowId) {
//    //Client things
//    var bool = !$(orderRowStatusId).checked === undefined;
//    console.log("Value or orderRowId: " + bool);

//    //Server
//    var result = signalRClient.server.changeStatusOnOrderRow(orderRowId);
//    if (result) {
//        $(orderRowStatusId).prop("checked", "checked");
//        $(orderRowStatusId).focus();
//    }
//    else {
//        $(orderRowStatusId).removeAttr("checked");
//        //show error? or let the server do that?

//    }
//    console.log("test");
//    console.log("changing orderstatus");
//}



//$("#messageBox").focus();

//$("#sendMessage").click(function () {
//    chat.server.send(userName, $("#messageBox").val());
//    $("#messageBox").val("");
//    $("#messageBox").focus();
//});




//$("#messageBox").keyup(function (event) {
//    if (event.keyCode == 13)
//        $("#sendMessage").click();
//});

//function addUser(user) {
//    $("#userList").append('<li>' + user + '</li>');
//}




