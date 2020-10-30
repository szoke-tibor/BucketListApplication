// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function SubmitBLEListing() {
    var selectlist = document.getElementById("SelectedBucketList_BucketListID");
    var option = selectlist.options[selectlist.selectedIndex];
    if (option.value != "null")
        document.getElementById("bucketlistselectform").submit();
}