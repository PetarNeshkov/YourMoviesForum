
var $select = $('#input-category').selectize({
    create: false,
    sortField: 'text',
});

$('#input-tags').selectize({
    create: false,
    sortField: 'text',
    plugins: ['remove_button'],
    delimiter: ',',
    persist: false
});

tinymce.init({
    selector: 'textarea#tinymceInput',
    height: 450,
    plugins: [
        'advlist autolink link image imagetools lists charmap preview hr anchor pagebreak',
        'searchreplace wordcount visualblocks visualchars code fullscreen  media nonbreaking',
        'emoticons template paste help'
    ],
    toolbar: [
        'undo redo | bold italic underline | alignleft aligncenter alignright alignjustify  bullist numlist hr | emoticons link codesample image media |'
    ],
    link_default_protocol: "https",
    automatic_uploalds: true,
    file_picker_types: 'image',
    file_picker_callback: function (cb, value, meta) {
        var input = document.createElement('input');
        input.setAttribute('type', 'file');
        input.setAttribute('accept', 'image/*');

        input.onchange = function () {
            var file = this.files[0];
            var reader = new FileReader();

            reader.onload = function () {
                var id = 'blobid' + (new Date()).getTime();
                var blobCache = tinymce.activeEditor.editorUpload.blobCache;
                var base64 = reader.result.split(',')[1];
                var blobInfo = blobCache.create(id, file, base64);
                blobCache.add(blobInfo);

                // call the callback and populate the Title field with the file name
                cb(blobInfo.blobUri(), { title: file.name });
            };
            reader.readAsDataURL(file);
        };

        input.click();
    },
    images_upload_handler: function (blobInfo, success, failure) {
        var xhr, formData;

        xhr = new XMLHttpRequest();
        xhr.withCredentials = false;
        xhr.open('POST', 'upload.php');

        xhr.onload = function () {
            var json;

            if (xhr.status != 200) {
                failure('HTTP Error: ' + xhr.status);
                return;
            }

            json = JSON.parse(xhr.responseText);

            if (!json || typeof json.location != 'string') {
                failure('Invalid JSON: ' + xhr.responseText);
                return;
            }

            success(json.location);
        };

        formData = new FormData();
        formData.append('file', blobInfo.blob(), blobInfo.filename());
        xhr.send(formData);
    },
    menubar: false
});
