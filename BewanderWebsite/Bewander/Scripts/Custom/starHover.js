// Five Star Review (with zero star option and half star options)
// Author - Marc Gray

// Stores the rating the last time the click event occurred
var currentClickedRating = 0;

// Resets all stars to empty
function StarReset() {
    
    for (var i = 0; i < 6; i++) {
        var x = "star".concat(String(i));
        document.getElementById(x).removeAttribute("class");
        document.getElementById(x).setAttribute("class", "col-xs-2 stars fa fa-star-o");
    }
}

function StarFill(rating) {
    // Sets display message to show current rating
    //console.log(rating);

    // For even-numbered ratings, only full stars are needed
    if (rating % 2 == 0) {
        var i = rating/2;
        while (i >= 0) {
            var x = "#star".concat(String(i));
            $(x).removeClass("fa-star-o").addClass("fa-star");
            i--;
        }
    }

    // For odd-numbered ratings, makes a half star, then calls the function again to fill in all the full stars to the left
    else {
        var i = (rating+1)/2;
        var x = "#star".concat(String(i));
        $(x).removeClass("fa-star-o").addClass("fa-star-half-o");
        StarFill(rating-1);
    }

    document.getElementById("starMessage").innerHTML = IntRatingtoText(rating);
}

function StarHovering(rating) {
	//console.log("hover")
    StarReset();
    StarFill(rating);
}

// Resets the stars to the state of the last click event when no longer hovering
function mouseOffHover() {
	//console.log("mouseoff");
    StarReset();
    StarFill(currentClickedRating);
}

// Called when a click event takes place
function StarClick(rating) {

    currentClickedRating = rating;
    StarReset();
    StarFill(rating);

    // Sets display message to show current rating
    document.getElementById("starMessage").innerHTML = IntRatingtoText(currentClickedRating);
    
    // Sets StarRating variable to a number from 0-10 based on where the click occurred
    document.getElementById("StarRating").value = rating;
}

function IntRatingtoText(score) {
    switch (score) {
        case 0:
            return "Don't Bother"
        case 1:
        case 2:
            return "Not that great"
        case 3:
        case 4:
            return "Not really worth your time"
        case 5:
        case 6:
            return "So-so, if you're bored"
        case 7:
        case 8:
            return "Solid"
        case 9:
        case 10:
            return "Don't miss out - you'll regret it"
    }
}