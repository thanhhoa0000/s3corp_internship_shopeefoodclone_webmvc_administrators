$(function () {
    $.validator.unobtrusive.parse('form');

    $('form input').on('blur', function () {
        $(this).valid();
    });
});
