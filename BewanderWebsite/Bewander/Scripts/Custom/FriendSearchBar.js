function friendSearch(element) {
	event.preventDefault();
	
	var text = element.value;

    //If there is no value or just spaces in the text field it wont show anything.
    if (!text || text.length === 0 || /^\s+$/.test(text)) {
        $("#friendSearch-results").html("").show();
        return;
    }

    //Call the function when something is typed into the text field
    $.ajax({
        type: "GET",
        url: "/Home/friendSearch?term=" + text,
        error: function (html) {
            $("#friendSearch-results").html("").show();
            return;
        },
        success: function (html) {
            $("#friendSearch-results").html("").show();
            $("#friendSearch-results").html(html).show();
        }
    });

}

//Hide friendSearch list when clicked away
$(function () {
    var $win = $("body"); // or $box parent container
    var $friendSearchBar = $("#friendSearch-bar");
    var friendSearchResult = document.getElementById("friendSearch-results");
    // To make the dropdown work correctly
    $("#friend-icon a").on("click", function () {
        $("#navbar-friend-list").toggle();
        $('#friend-icon').toggleClass('open');
    });
    $('body').on('click', function (e) {
        if (!$('#navbar-friend-list').is(e.target)
            && $('#navbar-friend-list').has(e.target).length === 0
            && $("#friend-icon").has(e.target).length === 0
        ) {
            $('#navbar-friend-list').hide();
            $('#friend-icon').removeClass('open');
        }
    });

    $win.on("click", function (event) {
        
        if ($friendSearchBar.has(event.target).length === 0) {
            friendSearchResult.style.display = "none";
        }
    });

});

//Go to Profile page on click for users
function ViewProfile(ID) {
    var url = "/Users/ProfilePage?userID=" + ID;
    window.location.href = url;
}


function NewReview(ID) {
    var url = "/Reviews/Create?PlaceID=" + ID;
    window.location.href = url;
}