//Google Autocomplete API

// This example displays an address form, using the autocomplete feature
// of the Google Places API to help users fill in the information.

// This example requires the Places library. Include the libraries=places
// parameter when you first load the API. For example:
// <script src="https://maps.googleapis.com/maps/api/js?key=YOUR_API_KEY&libraries=places">

var placeSearch, autocomplete;

//These detail values come from 'address_components' https://developers.google.com/maps/documentation/javascript/places#place_details_responses
var componentForm = {
    street_number: 'short_name',
    route: 'short_name',
    locality: 'long_name',
    postal_town: 'long_name',
    administrative_area_level_1: 'long_name',
    country: 'long_name',
    postal_code: 'short_name'
};



function initAutocomplete() {
    autocomplete = new google.maps.places.Autocomplete(document.getElementById('autocomplete'));

    // When the user selects an address from the dropdown, populate the address fields in the form.
    autocomplete.addListener('place_changed', fillInAddress);
}

function fillInAddress() {
    // Get the place details from the autocomplete object.
    var place = autocomplete.getPlace();

    // Get ID of the chosen place. This value will be saved into the Place Table as PlaceID.
    var placeID = place.place_id;
    var placeName = place.name;

    document.getElementById("PlaceID").value = placeID;
    document.getElementById("PlaceName").value = placeName;

    /*-- After value is chosen, textbox are no longer disabled
    --> Commented out: Do not want user to change values manually --*/
    //for (var component in componentForm) {
    //    document.getElementById(component).value = '';
    //    document.getElementById(component).disabled = false;
    //}

    // Get each component of the address from the place details
    // and fill the corresponding field on the form.
    //for (var i = 0; i < place.address_components.length; i++) {
    //    var addressType = place.address_components[i].types[0];
    //    if (componentForm[addressType]) {
    //        if (addressType == "postal_town") {
    //            document.getElementById("locality").removeAttribute("name");
    //        }
    //        var val = place.address_components[i][componentForm[addressType]];
    //        document.getElementById(addressType).value = val;
    //        console.log(val);
    //    }
    //}

}


// Bias the autocomplete object to the user's geographical location,
// as supplied by the browser's 'navigator.geolocation' object.
function geolocate() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: position.coords.accuracy
            });
            autocomplete.setBounds(circle.getBounds());
        });
    }
}

