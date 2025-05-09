"use strict";
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
        if (span.text().trim() !== '') {
            isValid = false;
        }
    });
    if (window.location.pathname === '/Product/Create') {
        const fileInput = $('input[type="file"]')[0];
        if (!fileInput.files?.length) {
            isValid = false;
            $(fileInput).closest('.form-group').find('.validate-message').text('Image is required.');
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
$(document).on('click', '.menu-title', function (event) {
    const menuTitle = $(event.currentTarget);
    const menuId = menuTitle.attr('menu-id');
    if (!menuId)
        return;
    const updateSection = $(`.menu-update-section[menu-id="${menuId}"]`);
    const deleteSection = $(`.menu-delete-products-section[menu-id="${menuId}"]`);
    if (updateSection.is(':hidden') && deleteSection.is(':hidden')) {
        updateSection.removeAttr("hidden").show();
    }
    else {
        updateSection.hide().attr("hidden", "true");
        deleteSection.hide().attr("hidden", "true");
    }
});
$(document).on('click', '.menu-update-section div button', function () {
    const menuId = $(this).closest('.menu-update-section').attr('menu-id');
    if (!menuId)
        return;
    const updateSection = $(`.menu-update-section[menu-id="${menuId}"]`);
    const deleteSection = $(`.menu-delete-products-section[menu-id="${menuId}"]`);
    updateSection.hide().attr("hidden", "true");
    deleteSection.removeAttr("hidden").show();
});
$(document).on('click', '.menu-delete-products-section div button', function () {
    const menuId = $(this).closest('.menu-delete-products-section').attr('menu-id');
    if (!menuId)
        return;
    const updateSection = $(`.menu-update-section[menu-id="${menuId}"]`);
    const deleteSection = $(`.menu-delete-products-section[menu-id="${menuId}"]`);
    deleteSection.hide().attr("hidden", "true");
    updateSection.removeAttr("hidden").show();
});
$(document).on('input', '.menu-update-section .products-search input', function () {
    const keyword = $(this).val().trim().toLowerCase();
    $('.menu-update-section .products-list .menu-update-product-item').each(function () {
        const productName = $(this).find('label')
            .text().trim()
            .toLowerCase()
            .normalize('NFD')
            .replace(/\p{Diacritic}/gu, '');
        if (keyword === "" || productName.includes(keyword)) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
});
$(document).on('input', '.menu-delete-products-section .products-search input', function () {
    const keyword = $(this).val().trim().toLowerCase();
    $('.menu-delete-products-section .products-list .menu-update-product-item').each(function () {
        const productName = $(this).find('label')
            .text().trim()
            .toLowerCase()
            .normalize('NFD')
            .replace(/\p{Diacritic}/gu, '');
        if (keyword === "" || productName.includes(keyword)) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
});
$(document).on('click', '.add-products-to-menu-btn', function () {
    const menuId = $(this).attr('menu-id');
    const $target = $(`.menu-update-section[menu-id="${menuId}"]`);
    let productIds = [];
    const $input = $target.find('input[type="checkbox"]');
    $input.each(function () {
        const val = $(this).val();
        if ($(this).is(':checked') && typeof val === 'string') {
            productIds.push(val);
        }
    });
    $.ajax({
        url: `/Menu/AddProductsToMenu`,
        type: "POST",
        data: {
            menuId: menuId,
            productIds: productIds
        },
        traditional: true,
        success: function (response) {
            if (response.success) {
                location.reload();
                toastr.success(response.message);
                document.dispatchEvent(new Event("menuUpdated"));
            }
            else {
                toastr.error(response.message);
            }
        },
        error: function () {
            console.error("Failed to update menu.");
        }
    });
});
$(document).on('click', '.remove-products-from-menu-btn', function () {
    const menuId = $(this).attr('menu-id');
    const $target = $(`.menu-delete-products-section[menu-id="${menuId}"]`);
    let productIds = [];
    const $input = $target.find('input[type="checkbox"]');
    $input.each(function () {
        const val = $(this).val();
        if ($(this).is(':checked') && typeof val === 'string') {
            productIds.push(val);
        }
    });
    $.ajax({
        url: `/Menu/RemoveProductsFromMenu`,
        type: "POST",
        data: {
            menuId: menuId,
            productIds: productIds
        },
        traditional: true,
        success: function (response) {
            if (!response.success) {
                toastr.error("Đã xảy ra lỗi");
            }
            location.reload();
            toastr.success("Cập nhật menu thành công");
            document.dispatchEvent(new Event("menuUpdated"));
        },
        error: function () {
            console.error("Failed to update menu.");
        }
    });
});
function getProvinces() {
    $.ajax({
        url: `${gatewayUrl}/provinces`,
        type: 'GET',
        success: function (response) {
            if (!response || !response.isSuccessful || !Array.isArray(response.body)) {
                console.error("Invalid API response format:", response);
                return;
            }
            let dropdown = $('#update-store-province-dropdown');
            dropdown.empty();
            response.body.sort((a, b) => {
                if (a.codeName === "ho_chi_minh")
                    return -1;
                if (b.codeName === "ho_chi_minh")
                    return 1;
                if (a.codeName === "ha_noi")
                    return -1;
                if (b.codeName === "ha_noi")
                    return 1;
                return 0;
            });
            response.body.forEach(function (province) {
                let provinceItem = `<li class="update-store-province-item"><a class="dropdown-item" province-code="${province.code}" onclick="getDistricts('${province.code}')">${province.name}</a></li>`;
                dropdown.append(provinceItem);
            });
            dropdown.on('click', '.update-store-province-item', function () {
                let button = $('#province-btn');
                if (this.textContent !== button.text()) {
                    $('#district-btn').text("Quận/Huyện");
                    $('#ward-btn').text("Phường/Xã");
                }
                button.text(this.textContent);
            });
            document.dispatchEvent(new Event("provinceLoaded"));
        },
        error: function () {
            console.error("Failed to fetch provinces.");
        }
    });
}
function getDistricts(province) {
    $.ajax({
        url: `${gatewayUrl}/districts?province=${province}`,
        type: 'GET',
        success: function (response) {
            if (!response || !response.isSuccessful || !Array.isArray(response.body)) {
                console.error("Invalid API response format:", response);
                return;
            }
            let dropdown = $('#update-store-district-dropdown');
            dropdown.empty();
            let districtsList = response.body.sort();
            districtsList.forEach(function (district) {
                let districtItem = `<li class="update-store-district-item"><a class="dropdown-item" onclick="getWards('${district.code}');" district-code="${district.code}" province-code="${province}">${district.name.trim().split(/\s+/).length === 1 ? "Quận " + district.name : district.name}</a></li>`;
                dropdown.append(districtItem);
            });
            dropdown.on('click', '.update-store-district-item', function () {
                let button = $('#district-btn');
                if (this.textContent !== button.text()) {
                    $('#ward-btn').text("Phường/Xã");
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
function getWards(district) {
    $.ajax({
        url: `${gatewayUrl}/wards?district=${district}`,
        type: 'GET',
        success: function (response) {
            if (!response || !response.isSuccessful || !Array.isArray(response.body)) {
                console.error("Invalid API response format:", response);
                return;
            }
            let dropdown = $('#update-store-ward-dropdown');
            dropdown.empty();
            let wardsList = response.body.sort();
            wardsList.forEach(function (ward) {
                let wardItem = `<li class="update-store-ward-item"><a class="dropdown-item" ward-code="${ward.code}" district-code="${district}">${ward.name.split(/\s+/).length === 1 ? "Phường " + ward.name : ward.name}</a></li>`;
                dropdown.append(wardItem);
            });
            dropdown.on('click', '.update-store-ward-item', function () {
                let button = $('#ward-btn');
                $('#WardCode').val($(this).find('a').attr('ward-code') || '');
                button.text(this.textContent);
            });
            document.dispatchEvent(new Event("wardLoaded"));
        },
        error: function () {
            console.error("Failed to fetch wards.");
        }
    });
}
//# sourceMappingURL=store.js.map