document.addEventListener("configLoaded", getProvinces);

document.addEventListener("provinceLoaded", function () {
    $(`.update-store-province-item a[province-code='${$('#hidden-province-code').val()}']`).trigger('click');
});

document.addEventListener("districtLoaded", function () {
    $(`.update-store-district-item a[district-code='${$('#hidden-district-code').val()}']`).trigger('click');
});

document.addEventListener("wardLoaded", function () {
    $(`.update-store-ward-item a[ward-code='${$('#WardCode').val()}']`).trigger('click');
});


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

$('form > button').on('click', function (event) {
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
    
    console.log(isValid);

    if (isValid) {
        $(this).closest('form')[0].submit();
    }
});

$(document).on('focus', '.validate-field', function () {
    let span = $('.validate-message');
    $('.validate-field').css('border-color', '');
    span.text('');
    span.hide().attr("hidden", "true");
    $('.store-address-section div button').css('border-color', '');
    $('.address-validation-message').hide().attr("hidden", "true");
});

$('.update-store-img button').on('click', function () {
    $('.update-store-img').hide().attr("hidden", "true");
    $('.update-store-img-input').removeAttr("hidden").show();
});

$(document).on("click", ".store-address-section div button", function () {
    let span = $('.validate-message');
    $('.validate-field').css('border-color', '');
    span.text('');
    span.hide().attr("hidden", "true");
    $('.store-address-section div button').css('border-color', '');
    $('.address-validation-message').hide().attr("hidden", "true");
});

$('#detail-address')
    .on('blur', function () {
        if ($(this).val() === '') {
            $(this).css('border-color', primaryRed);
        }
    })
    .on('click', function () {
        let span = $('.validate-message');
        $('.validate-field').css('border-color', '');
        span.text('');
        span.hide().attr("hidden", "true");
        $('.store-address-section div button').css('border-color', '');
        $('.address-validation-message').hide().attr("hidden", "true");
    });

$('.store-address-section div button').on('blur', function () {
    const keywords = ["Tỉnh/Thành Phố", "Quận/Huyện", "Phường/Xã"];
    const validationMessage = $('.address-validation-message');
    let inputAddress = $('#detail-address');

    setTimeout(function () {
        $('*').each(function () {
            const text = $(this).text().trim();
            if (keywords.includes(text)) {
                $(this).css('border-color', primaryRed);
                validationMessage.removeAttr("hidden").show();
            }
        });

        if (inputAddress.val()?.toString().trim() === '') {
            inputAddress.css('border-color', primaryRed);
        }
    }, 0);
});

function getProvinces(): void {
    $.ajax({
        url: `${gatewayUrl}/provinces`,
        type: 'GET',
        success: function (response: any): void {
            if (!response || !response.isSuccessful || !Array.isArray(response.body)) {
                console.error("Invalid API response format:", response);
                return;
            }

            let dropdown = $('#update-store-province-dropdown');
            dropdown.empty();

            response.body.sort((a, b) => {
                if (a.codeName === "ho_chi_minh") return -1;
                if (b.codeName === "ho_chi_minh") return 1;
                if (a.codeName === "ha_noi") return -1;
                if (b.codeName === "ha_noi") return 1;
                return 0;
            });

            response.body.forEach(function (province: any) {
                let provinceItem = `<li class="update-store-province-item"><a class="dropdown-item" province-code="${province.code}" onclick="getDistricts('${province.code}')">${province.name}</a></li>`
                dropdown.append(provinceItem);
            });

            dropdown.on('click', '.update-store-province-item', function (): void {
                let button = $('#province-btn');
                if (this.textContent !== button.text()) {
                    $('#district-btn').text("Quận/Huyện");
                    $('#ward-btn').text("Phường/Xã");
                }
                button.text(this.textContent);
            });

            document.dispatchEvent(new Event("provinceLoaded"));
        },
        error: function (): void {
            console.error("Failed to fetch provinces.");
        }
    });
}

function getDistricts(province: string) {
    $.ajax({
        url: `${gatewayUrl}/districts?province=${province}`,
        type: 'GET',
        success: function (response: any) {
            if (!response || !response.isSuccessful || !Array.isArray(response.body)) {
                console.error("Invalid API response format:", response);
                return;
            }

            let dropdown = $('#update-store-district-dropdown');
            dropdown.empty();

            let districtsList = response.body.sort();

            districtsList.forEach(function (district: any): void {
                let districtItem = `<li class="update-store-district-item"><a class="dropdown-item" onclick="getWards('${district.code}');" district-code="${district.code}" province-code="${province}">${district.name.trim().split(/\s+/).length === 1 ? "Quận " + district.name : district.name}</a></li>`;
                dropdown.append(districtItem);
            });

            dropdown.on('click', '.update-store-district-item', function (): void {
                let button = $('#district-btn');
                if (this.textContent !== button.text()) {
                    $('#ward-btn').text("Phường/Xã")
                }
                button.text(this.textContent);
            });

            document.dispatchEvent(new Event("districtLoaded"));
        },
        error: function () {
            console.error("Failed to fetch districts.");
        }
    });
}

function getWards(district: string) {
    $.ajax({
        url: `${gatewayUrl}/wards?district=${district}`,
        type: 'GET',
        success: function (response: any): void {
            if (!response || !response.isSuccessful || !Array.isArray(response.body)) {
                console.error("Invalid API response format:", response);
                return;
            }

            let dropdown = $('#update-store-ward-dropdown');
            dropdown.empty();

            let wardsList = response.body.sort();

            wardsList.forEach(function (ward: any): void {
                let wardItem = `<li class="update-store-ward-item"><a class="dropdown-item" ward-code="${ward.code}" district-code="${district}">${ward.name.split(/\s+/).length === 1 ? "Phường " + ward.name : ward.name}</a></li>`;
                dropdown.append(wardItem);
            });

            dropdown.on('click', '.update-store-ward-item', function (): void {
                let button = $('#ward-btn');
                $('#WardCode').val($(this).attr('ward-code') || '');
                button.text(this.textContent);
            });

            document.dispatchEvent(new Event("wardLoaded"));
        },
        error: function () {
            console.error("Failed to fetch wards.");
        }
    });
}

