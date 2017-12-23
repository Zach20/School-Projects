// Changes photo button to display number of pictures choosen
var inputs = document.querySelectorAll('.input-file');
Array.prototype.forEach.call(inputs, function (input) {
    var label = input.nextElementSibling,
        labelVal = label.innerHTML;

    input.addEventListener('change', function (e) {
        var fileName = '';
        if (this.files && this.files.length > 1)
            fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length);
        else if (this.files && this.files.length == 1) {
            fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count} Photos', this.files.length + " Photo");
        }
        else
            fileName = e.target.value.split('\\').pop();

        if (fileName)
            label.querySelector('span').innerHTML = fileName;
        else
            label.innerHTML = labelVal;
    });
});

// Display preview of image selected
$("#postPhoto").change(function () {
    //readURL(this);
    var input = this;
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        document.getElementById('post-image-preview').style.display = "block";
        reader.onload = function (e) {
            $('#post-image-preview').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
});
//JMV Marker
//$("#postPhotoPreview").change(function () { 
//    //readURL(this);
//    var input = this;
//    if (input.files && input.files[0]) {
//        var reader = new FileReader();
//        document.getElementById('post-image-preview').style.display = "block";
//        reader.onload = function (e) {
//            $('#post-image-preview').attr('src', e.target.result);
//        }

//        reader.readAsDataURL(input.files[0]);
//    }
//});

function SubmitPost() {
     
    $('#post-form').ajaxSubmit({
        success: function (html) {
            console.log(html);
            $('#newsfeed-list').prepend(html).show(); // Append new post to beginning of newsfeed
            document.getElementById("post-image-preview").style.display = "none"; // Hide the image preview
            var postPhoto = document.getElementById("postPhoto"); // GET: File input element  
            var fileName = postPhoto.getAttribute("data-multiple-caption").replace('{count} Photos', "Photo"); // Reset the photo count attribute
            postPhoto.nextElementSibling.querySelector('span').innerHTML = fileName; // Reset the label
            document.getElementById("post-form").reset();
        },
        
    });
}


//POST: LikePost function
function LikePost(ID, element) {
    var listID = "#" + ID + "-likes-list";
    var likesCount = document.getElementById(ID + "-likes-count");
    $.ajax({
        type: "POST",
        url: "/Posts/LikePost?postID=" + ID,
        error: function (html) {
            $(listID).html("").show();
            return;
        },
        success: function (html) {
            $(listID).html("").show();
            $(listID).html(html).show();
            element.setAttribute("onclick", "UnlikePost('" + ID + "', this)");
            element.setAttribute("class", "unlike-post");
            likesCount.innerHTML = parseInt(likesCount.innerHTML) + 1;
        }
    });
}

//POST: UnlikePost function
function UnlikePost(ID, element) {
    var listID = "#" + ID + "-likes-list";
    var likesCount = document.getElementById(ID + "-likes-count");
    $.ajax({
        type: "POST",
        url: "/Posts/UnlikePost?postID=" + ID,
        error: function (html) {
            $(listID).html("").show();
            return;
        },
        success: function (html) {
            $(listID).html("").show();
            $(listID).html(html).show();
            element.setAttribute("onclick", "LikePost('" + ID + "', this)");
            element.setAttribute("class", "like-post");
            likesCount.innerHTML = parseInt(likesCount.innerHTML) - 1;
        }
    });
}

//(function() {
//    var bar = $('.progress-bar');
//    var percent = $('.progress-bar');
//    var status = $('#status');
//    $('form').ajaxForm({
//        beforeSend: function () {
//            status.empty();
//            var percentVal = '0%';
//            bar.width(percentVal)
//            percent.html(percentVal);
//        },
//        uploadProgress: function (event, position, total, percentComplete) {
//            var percentVal = percentComplete + '%';
//            bar.width(percentVal)
//            percent.html(percentVal);
//        },
//        success: function () {
//            var percentVal = '100%';
//            bar.width(percentVal)
//            percent.html(percentVal);
//        },
//        complete: function (xhr) {
//            status.html(xhr.responseText);
//        }
//    });
//})();