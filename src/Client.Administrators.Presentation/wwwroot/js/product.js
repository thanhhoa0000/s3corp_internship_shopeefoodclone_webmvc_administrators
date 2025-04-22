"use strict";
$(function () {
    let section = localStorage.getItem('section');
    if (!section) {
        section = "stores";
        localStorage.setItem('section', JSON.stringify(section));
    }
    else {
        section = JSON.parse(section);
    }
    document.querySelectorAll(".main-nav-item").forEach((item) => {
        item.classList.remove("active");
    });
    $(`.main-nav-item[code-name='${section}']`).addClass("active");
});
$(document).on('blur', '.validate-field', function () {
    let minLength = 0;
    let maxLength = Number.MAX_SAFE_INTEGER;
    let rules = [Rule.IsRequired];
    let name = $(this).closest('.form-group').find('label')
        .clone().children().remove().end().text().trim();
    let span = $(this).closest('.form-group').find('.validate-message');
    let className = span.attr('class').split(' ').find(c => c !== 'validate-message');
    if (className === "name") {
        minLength = 10;
        maxLength = 50;
        rules.push(Rule.HasMinLength, Rule.HasMaxLength);
    }
    if (className === "description") {
        minLength = 20;
        maxLength = 200;
        rules.push(Rule.HasMinLength, Rule.HasMaxLength);
    }
    formFieldValidate(name, $(this), span, rules, minLength, maxLength);
});
$('form button').on('click', function (event) {
    event.preventDefault();
    let validateField = $('.validate-field');
    validateField.each(function () {
        let minLength = 0;
        let maxLength = Number.MAX_SAFE_INTEGER;
        let rules = [Rule.IsRequired];
        let $field = $(this);
        let name = $field.closest('.form-group').find('label')
            .clone().children().remove().end().text().trim();
        let span = $field.closest('.form-group').find('.validate-message');
        let className = span.attr('class').split(' ').find(c => c !== 'validate-message');
        if (className === "name") {
            minLength = 10;
            maxLength = 50;
            rules.push(Rule.HasMinLength, Rule.HasMaxLength);
        }
        if (className === "description") {
            minLength = 20;
            maxLength = 200;
            rules.push(Rule.HasMinLength, Rule.HasMaxLength);
        }
        if (className === "price") {
            rules = [Rule.IsRequired];
        }
        formFieldValidate(name, $field, span, rules, minLength, maxLength);
    });
    const allValid = $('.validate-message').toArray().every(span => $(span).text().trim() === '');
    if (allValid) {
        $(this).closest('form')[0].submit();
    }
});
$(document).on('focus', '.validate-field', function () {
    let span = $(this).closest('.form-group').find('span');
    $(this).css('border-color', '');
    span.text('');
    span.hide();
});
//# sourceMappingURL=product.js.map