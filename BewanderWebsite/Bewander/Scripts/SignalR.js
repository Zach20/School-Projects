// SignalR Connection

//$(function () {
//    var signalR = $.connection.notificationsHub;

//    signalR.client.updateClient = function (message) {
//        alert(message);
//    };

//    $("#open").click(function () {
//        signalR.server.updateServer();
//    });

//    $.connection.hub.start(function () {
//        alert("Connected");
//    });
//});

//$(function () {
//    // Declare a proxy to reference the hub.
//    var AppHub = $.connection.AppHub;
//    var userID = "@Model.UserID";

//    // Start the connection.
//    $.connection.hub.start().done(function () {
//        // Connection has started
//        $('#chat-header').append('<h4>Chat connection established.</h4>');
//        // Add user to userConnLookupTable
//        notifications.server.registerConn(userID);
//        console.log("Register: " + userID);
//    });

//    //Display list of current users received from the hub
//    notifications.client.displayListOfConnected = function (connectedUsers) {
//        $('#chat-user-list').empty();
//        $.each(connectedUsers, function (index, value) {
//            //$('#chat-user-list').append('<li onclick="SelectUser(this)" value=' + value.UserID + ' connID=' + value.ConnectionID + '>' + value.Name + '</li>');
//            $('#chat-user-list').append('<li onclick="SelectUser(this)" connID=' + value.ConnectionID + ' count=' + value.Count + '>' + value.Name + '</li>');
//            connIDList.push(value.ConnectionID);
//            console.log("User List Add: " + value.Name + " " + value.ConnectionID);
//        });
//    };
//});