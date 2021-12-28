
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

