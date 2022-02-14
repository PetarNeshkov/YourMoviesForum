tinymce.init({
    selector: 'textarea#tinymceInput',
    height: 550,
    resize: 'both',
    plugins: [
        'advlist autolink link image imagetools lists charmap preview hr anchor pagebreak',
        'searchreplace wordcount visualblocks visualchars code fullscreen  media nonbreaking',
        'emoticons template paste help'
    ],
    toolbar: [
        'undo redo | bold italic underline| alignleft aligncenter alignright alignjustify | bullist numlist hr| emoticons | link codesample image media'
    ],
    image_title: true,
    automatic_uploads: true,

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
                cb(blobInfo.blobUri(), { title: file.name });
            };
            reader.readAsDataURL(file);
        };
        input.click();
    },
    link_default_protocol: "https",
    menubar: false,
    branding: false
});