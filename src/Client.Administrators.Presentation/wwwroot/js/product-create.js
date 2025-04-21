"use strict";
$(function () {
    $.validator.unobtrusive.parse('form');
    $('form input').on('blur', function () {
        $(this).valid();
    });
});
$(document).on("keypress", '#Name', (event) => {
    const key = event.key;
    if (!/^[a-zA-Z0-9\-|]$/.test(key)) {
        event.preventDefault();
    }
});
$(document).on("input", '#Name', function () {
    this.value = this.value.replace(/[^a-zA-Z0-9\-|]/g, '');
});
//# sourceMappingURL=product-create.js.map