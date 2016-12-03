var signalRClient = $.connection.driverHub;

signalRClient.client.messageReceived = function (originatorUser, message) {
    $("#messages").append('<li><strong>' + originatorUser + '</strong>: ' + message);
};

signalRClient.client.getConnectedUsers = function (userList) {
    for (var i = 0; i < userList.length; i++)
        addUser(userList[i]);
};

signalRClient.client.newUserAdded = function (newUser) {
    addUser(newUser);
}

signalRClient.client.changeOrderRowStatus = function (orderRowId) {
    addUser(orderRowId);
}

function changeOrderRowStatus(orderRowId) {

}

$("#messageBox").focus();

$("#sendMessage").click(function () {
    chat.server.send(userName, $("#messageBox").val());
    $("#messageBox").val("");
    $("#messageBox").focus();
});




$("#messageBox").keyup(function (event) {
    if (event.keyCode == 13)
        $("#sendMessage").click();
});

function addUser(user) {
    $("#userList").append('<li>' + user + '</li>');
}

$.connection.hub.logging = true;
$.connection.hub.start().done(function () {
    chat.server.connect(userName);
});