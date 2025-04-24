"use strict";
$(function () {
    let section = localStorage.getItem('section');
    if (section) {
        section = JSON.parse(section);
        document.querySelectorAll(".nav-link").forEach((item) => {
            item.classList.remove("active");
        });
        const activeItem = document.querySelector(`.nav-link[code-name='${section}']`);
        if (activeItem) {
            activeItem.classList.add("active");
        }
    }
});
$('form > button').on('click', function (event) {
    event.preventDefault();
    let validateField = $('input');
    validateField.each(function () {
        let $field = $(this);
        let name = $field.closest('.form-group').find('label')
            .clone().children().remove().end().text().trim();
        let span = $field.closest('.form-group').find('.validate-message');
        formFieldValidate(name, $field, span, [Rule.IsRequired], 0, Number.MAX_SAFE_INTEGER);
    });
    if ($('.validate-message:visible').length === 0) {
        $(this).closest('form')[0].submit();
    }
});
$(document).on('blur', 'input', function () {
    let name = $(this).closest('.form-group').find('label')
        .clone().children().remove().end().text().trim();
    let span = $(this).closest('.form-group').find('.validate-message');
    formFieldValidate(name, $(this), span, [Rule.IsRequired], 0, Number.MAX_SAFE_INTEGER);
});
$(document).on('focus', 'input', function () {
    let span = $('.validate-message');
    $('input').css('border-color', '');
    span.text('');
    span.hide().attr("hidden", "true");
});
//# sourceMappingURL=login.js.map