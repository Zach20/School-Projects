function Search(element) {
    var text = element.value;
    //If there is no value or just spaces in the text field it wont show anything.
    if (text.length === 0 || /^\s+$/.test(text)) {
        $("#search-results").html("").show();
        return;
    }

    //Call the function when something is typed into the text field
    $.ajax({
        type: "GET",
        url: "/Home/Search?term=" + text,
        error: function (html) {
            $("#search-results").html("").show();
            return;
        },
        success: function (html) {
            $("#search-results").html("").show();
            $("#search-results").html(html).show();
        }
    });

}

//Hide search list when clicked away

$(function () {
    var $win = $(window); // or $box parent container
    var $searchBar = $("#search-bar");
    var searchResult = document.getElementById("search-results");

	
    $win.on("click.Bst", function (event) {
    	if ($searchBar.has(event.target).length <= 0 && !$searchBar.is(event.target)) {
    		if (searchResult != null) {
    			searchResult.style.display = "none";
    		}
        }
    });

});

//Go to Profile page on click for users
function ViewProfile(ID) {
    var url = "/Users/ProfilePage?userID=" + ID;
    window.location.href = url;
}

//Go to Reviews page on click for places
function ViewPlace(ID) {
    var url = "/Reviews/DisplayReviews?PlaceID=" + ID;
    window.location.href = url;
}

function NewReview(ID) {
    var url = "/Reviews/Create?PlaceID=" + ID;
    window.location.href = url;
}
//Dissmiss notification
function DismissNotification(ID) {
    //Make a post to dismiss the notification (change the isread flag to true).
    $.ajax({
        type: "POST",
        url: "/Notifications/DismissNotification?ID=" + ID,
        success: function (data) {
            console.log("Success in dismissing notification!");
            console.log("UserID of target: {0}", data);
            var url = "/Users/ProfilePage?userID=" + data;
            window.location.href = url;
        }
    });
}

//Confirm friend request
function ConfirmFriend(element) {
    // GET: Highest parent node
    var subParent = element.parentElement;

    // GET: Relationship ID
    var ID = subParent.parentElement.getAttribute("relID");

    // POST: Change relationship status to 1 (Accepted)
    $.ajax({
        type: "POST",
        url: "/Relationships/ConfirmFriend?ID=" + ID,
        success: function () {
            // REMOVE: Friend request from list       
            subParent.parentElement.remove();

            // Keep friend request list open after confirm/deny a friend request
            document.getElementById("friend-request-icon").setAttribute("class", "open");

            NotificationCount("friend");
        }
    });
}

//Deny friend request
function DenyFriend(element) {
    // GET: Relationship ID
    var ID = element.parentElement.getAttribute("relID");

    // POST: Change relationship status to 2 (Deny)
    $.ajax({
        type: "POST",
        url: "/Relationships/DenyFriend?ID=" + ID
    });

    // REMOVE: Friend request from list
    element.parentElement.remove();
}

function NotificationCount(type) {
    if (type = "friend") {
        var friendRequestList = document.getElementById("friend-request-list");
        var friendRequestIcon = document.getElementById("friend-request-icon");
        var count = friendRequestList.childElementCount;
        var friendNotification = document.getElementById("friend-request-notification");
        if (count == 1) {
            friendNotification.style.display = "none";
            friendRequestIcon.style.display = "none";

            return;
        }
        friendNotification.innerText = count - 1;
        return;
    }
    else {

    }
}
