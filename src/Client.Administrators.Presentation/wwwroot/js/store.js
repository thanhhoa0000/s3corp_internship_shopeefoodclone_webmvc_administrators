"use strict";
$(document).on('blur', 'form input', function () {
    $(this).valid();
});
$(document).on("click", "form div div button", function () {
    const validationAddress = $('.address-validation-message');
    if (validationAddress.is(':visible')) {
        validationAddress.hide();
        $(this).css('border', '');
    }
});
$(document).on("click", "#detail-address", function (event) {
    const validationAddress = $('.address-validation-message');
    if (validationAddress.is(':visible')) {
        validationAddress.hide();
        $(this).css('border', '');
    }
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
$(document).on("keypress", '#detail-address', (event) => {
    const key = event.key;
    if (!/^[a-zA-Z0-9\-|]$/.test(key)) {
        event.preventDefault();
    }
});
$(document).on("input", '#detail-address', function () {
    this.value = this.value.replace(/[^a-zA-Z0-9\-|]/g, '');
});
function proceedCreateStoreForm() {
    const isProvinceSelected = isValidSelection("province-btn", "Tỉnh/Thành phố");
    const isDistrictSelected = isValidSelection("district-btn", "Quận/Huyện");
    const isWardSelected = isValidSelection("ward-btn", "Phường/Xã");
    const addressInputEl = document.querySelector('#detail-address');
    const addressInput = addressInputEl?.value.trim() || '';
    if (isProvinceSelected && isDistrictSelected && isWardSelected && addressInput !== "") {
        let streetAddress = addressInput;
        let ward = getSelectedText("ward-btn");
        let district = getSelectedText("district-btn");
        let province = getSelectedText("province-btn");
        if (ward.split(/\s+/).length === 1)
            ward = "Phường " + ward;
        if (district.split(/\s+/).length === 1)
            district = "Quận " + district;
        return $(".create-store-form").valid();
    }
    else {
        $('.address-validation-message').show();
        if (!isProvinceSelected)
            $('#province-btn').css('border', '1px solid #e74c3c');
        if (!isDistrictSelected)
            $('#district-btn').css('border', '1px solid #e74c3c');
        if (!isWardSelected)
            $('#ward-btn').css('border', '1px solid #e74c3c');
        if (addressInput === "")
            $('#detail-address').css('border', '1px solid #e74c3c');
        return false;
    }
}
function getProvinces() {
    $.ajax({
        url: `${gatewayUrl}/provinces`,
        type: 'GET',
        success: function (response) {
            if (!response || !response.isSuccessful || !Array.isArray(response.body)) {
                console.error("Invalid API response format:", response);
                return;
            }
            $.validator.unobtrusive.parse('form');
            let dropdown = $('#create-store-province-dropdown');
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
                let provinceItem = `<li class="create-store-province-item"><a class="dropdown-item" province-code="${province.code}" onclick="getDistricts('${province.code}')">${province.name}</a></li>`;
                dropdown.append(provinceItem);
            });
            dropdown.on('click', '.create-store-province-item', function () {
                let button = $('#province-btn');
                const text = this.textContent || '';
                if (this.textContent !== button.text()) {
                    $('#district-btn').text("Quận/Huyện");
                    $('#ward-btn').text("Phường/Xã");
                }
                button.text(text);
            });
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
            let dropdown = $('#create-store-district-dropdown');
            dropdown.empty();
            let districtsList = response.body.sort((a, b) => a.name.localeCompare(b.name));
            districtsList.forEach(function (district) {
                let districtItem = `<li class="create-store-district-item"><a class="dropdown-item" onclick="getWards('${district.code}');" district-code="${district.code}">${district.name.trim().split(/\s+/).length === 1 ? "Quận " + district.name : district.name}</a></li>`;
                dropdown.append(districtItem);
            });
            dropdown.on('click', '.create-store-district-item', function () {
                let button = $('#district-btn');
                const text = this.textContent || '';
                if (text !== button.text()) {
                    $('#ward-btn').text("Phường/Xã");
                }
                button.text(text);
            });
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
            let dropdown = $('#create-store-ward-dropdown');
            dropdown.empty();
            let wardsList = response.body.sort((a, b) => a.name.localeCompare(b.name));
            wardsList.forEach(function (ward) {
                let wardItem = `<li class="create-store-ward-item"><a class="dropdown-item" ward-code="${ward.code}">${ward.name.split(/\s+/).length === 1 ? "Phường " + ward.name : ward.name}</a></li>`;
                dropdown.append(wardItem);
            });
            dropdown.on('click', '.create-store-ward-item', function () {
                let button = $('#ward-btn');
                const text = this.textContent || '';
                const wardCode = $(this).find('a').attr('ward-code') || '';
                $('#ward-code').val(wardCode);
                button.text(text);
            });
        },
        error: function () {
            console.error("Failed to fetch wards.");
        }
    });
}
function getSelectedText(buttonId) {
    const btn = document.getElementById(buttonId);
    return btn?.textContent?.trim() || '';
}
function isValidSelection(buttonId, defaultText) {
    return getSelectedText(buttonId) !== defaultText;
}
//# sourceMappingURL=store.js.map