//"likes" a public photo: turns thumbs up icon green and triggers LikeImage ActionResult in PostsController
function LikeImage(ImageID, element) {
    var likeIcon = document.getElementById(ImageID);
    var imageLiked = false;
    $.ajax({
        type: "POST",
        url: "/Posts/LikeImage?ID=" + ImageID,
        success: function (html) {
            element.setAttribute("onclick", "UnlikeImage('" + ImageID + "', this)");
        }
    });
    imageLiked = true;
    if (imageLiked == true) {
        $(likeIcon).removeClass('like-post').addClass('unlike-post');
    }
}

//"unlikes" a public photo: turns thumbs up icon gray and triggers UnlikeImage ActionResult in PostsController
function UnlikeImage(ImageID, element) {
    var likeIcon = document.getElementById(ImageID);
    var imageLiked = true;
    $.ajax({
        type: "POST",
        url: "/Posts/UnlikeImage?ID=" + ImageID,
        success: function (html) {
            element.setAttribute("onclick", "LikeImage('" + ImageID + "', this)");
        }
    });
    imageLiked = false;
    if (imageLiked == false) {
        $(likeIcon).removeClass('unlike-post').addClass('like-post');
    }
}