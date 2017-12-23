// Show Chat
function ShowChatList(icon) {
    var chatList = document.getElementById("chat-container");
    var chevRight = document.getElementById("chat-list-chevron-right");
    icon.style.display = "none";
    chevRight.style.display = "block";
    TweenLite.to(chatList, .4, { right: "0" });
}

// Hide Chat
function HideChatList(icon) {
    var chatList = document.getElementById("chat-container");
    var chevLeft = document.getElementById("chat-list-chevron-left");
    icon.style.display = "none";
    chevLeft.style.display = "block";
    TweenLite.to(chatList, .4, { right: "-244px" });
}

// Select user from list and generate message container
function SelectUser(relationshipID, targetUserID) {
    // GET: Messages with target user
    $.ajax({
        type: "GET",
        url: "/Home/_ChatBox?relationshipID=" + relationshipID + "&targetUserID=" + targetUserID,
        error: function (html) {
            $("#chat-tabs-container").html("").show();
            return;
        },
        success: function (html) {
            var element = document.getElementById("chat-convo-" + relationshipID);
            if (element == null) {
                $("#chat-tabs-container").append(html);
            }          
            var ulElement = document.getElementById("conversation-" + relationshipID);
            console.log(ulElement);
            ulElement.scrollTop = ulElement.scrollHeight - ulElement.clientHeight;
        }
    });
}

function HideChatConvo(id) {
    var element = document.getElementById(id);
    try {
        var id = element.getAttribute("id");
        element.style.display = "none";

        var tab = document.getElementById(id + "-tab");
        tab.style.display = "block";
    } catch (err) {};
}

function ShowChatConvo(id) {
    var chatConvoContainer = document.getElementById(id);
    chatConvoContainer.style.display = "block";

    var tab = document.getElementById(id + "-tab");
    tab.style.display = "none";
}

function RemoveElement(id) {
    var elem = document.getElementById("chat-convo-" + id);
    elem.parentNode.removeChild(elem);
}


