$(document).ready(function () {
    $('#chooseImg').change(function (e) {
        debugger
        var url = $('#chooseImg').val();
        var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
        if (chooseImg.files && chooseImg.files[0] && (ext == "gif" || ext == "jpg" || ext == "jfif" || ext == "png" || ext == "bmp")) {
            var reader = new FileReader();
            reader.onload = function () {
                var output = document.getElementById('imgPreview');
                output.src = reader.result;
            }
            reader.readAsDataURL(e.target.files[0]);
        }




        //var reader = new FileReader();
        //reader.onload = function () {
        //    var output = document.getElementById('imgPreview');
        //    output.src = reader.result;
        //}
        //reader.readAsDataURL(e.target.files[0]);




        /*imgPreview.src = URL.createObjectURL(e.target.files[0]);*/


        //const [file] = chooseImg.files;
        //if (file) {
        //    imgPreview.src = URL.createObjectURL(file);
        //}
    });
})