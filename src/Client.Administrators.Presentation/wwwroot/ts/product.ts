$(function () {
    let section: string | null = localStorage.getItem('section');

    if (!section) {
        section = "stores";
        localStorage.setItem('section', JSON.stringify(section));
    } else {
        section = JSON.parse(section);
    }

    document.querySelectorAll(".main-nav-item").forEach((item: Element) => {
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
    let className = span.attr('class')!.split(' ').find(c => c !== 'validate-message');

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

$('form > button').on('click', function(event) {
    event.preventDefault();

    let isValid = true;
    
    let validateField = $('.validate-field');

    validateField.each(function () {
        let minLength = 0;
        let maxLength = Number.MAX_SAFE_INTEGER;
        let rules = [Rule.IsRequired];
        
        let $field = $(this);
        let name = $field.closest('.form-group').find('label')
            .clone().children().remove().end().text().trim();
        let span = $field.closest('.form-group').find('.validate-message');
        let className = span.attr('class')!.split(' ').find(c => c !== 'validate-message');

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

        if (span.text().trim() !== '') {
            isValid = false;
        }
    });

    if (window.location.pathname === '/Product/Create') {
        const fileInput = $('input[type="file"]')[0] as HTMLInputElement;
        if (!fileInput.files?.length) {
            isValid = false;
            $(fileInput).closest('.form-group')!.find('.validate-message').text('Image is required.');
        }
    }


    if (isValid) {
        $(this).closest('form')[0].submit();
    }
});

$(document).on('focus', '.validate-field', function () {
    let span = $('.validate-message');
    $('.validate-field').css('border-color', '');
    span.text('');
    span.hide().attr("hidden", "true");
});

$(document).on("keypress", '#Price', function (event) {
    if (!/[0-9]/.test(event.key)) {
        event.preventDefault();
    }
});

$(document).on("input", '#Price', function () {
    this.value = this.value.replace(/[^0-9]/g, '');
});

$('.product-img button').on('click', function () {
   $('.product-img').hide().attr("hidden", "true");
   $('.update-product-img').removeAttr("hidden").show();
});
