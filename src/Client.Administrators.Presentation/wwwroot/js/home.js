"use strict";
$(function () {
    let section = localStorage.getItem('section');
    let seeStoresList = $('.see-stores-list');
    const menuItems = $('.menu div');
    const vendorId = $('.home-stores-section').attr('vendor-id');
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
    seeStoresList.addClass('active');
    menuItems.on('click', function () {
        const isStoreList = $(this).hasClass('see-stores-list');
        const isCreateStore = $(this).hasClass('add-store');
        menuItems.removeClass('active');
        $(this).addClass('active');
        toggleStoreItems(isStoreList);
        if (isStoreList) {
            getStores($('.home-stores-section').attr('vendor-id'));
        }
        if (isCreateStore) {
            getCreateStoreForm();
        }
    });
    seeStoresList.trigger('click');
});
function getStores(vendorId) {
    $.ajax({
        url: `/Home/Index?vendorId=${vendorId}`,
        type: 'POST',
        success: (response) => {
            let parsed = $('<div>').html(response);
            $('.main-content').replaceWith(parsed.find('.main-content'));
            const isSeeStoresList = $('.menu .see-stores-list').hasClass('active');
            toggleStoreItems(isSeeStoresList);
        },
        error: (xhr, status, error) => {
            console.error("Failed to get stores");
        }
    });
}
function getCreateStoreForm() {
    $.ajax({
        url: `/Store/Create`,
        type: 'GET',
        success: (response) => {
            let parsed = $('<div>').html(response);
            $('.main-content').replaceWith(parsed.find('.main-content'));
            if (gatewayUrl) {
                getProvinces();
            }
            else {
                document.addEventListener("configLoaded", () => getProvinces(), { once: true });
            }
        },
        error: (xhr, status, error) => {
            console.error("Failed to get form");
        }
    });
}
function toggleStoreItems(show) {
    const storeItems = $('.store-item');
    show ? storeItems.show() : storeItems.hide();
}
//# sourceMappingURL=home.js.map