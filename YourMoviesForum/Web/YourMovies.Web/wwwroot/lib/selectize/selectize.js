
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

var $select = $('#input-users').selectize({
    create: false,
    sortField: 'text',
    placeholder: 'Write a message to'
});
$select.each(function () {
    $(this)[0].selectize.clear(true);
});
