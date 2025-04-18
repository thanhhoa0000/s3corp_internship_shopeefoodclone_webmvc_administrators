"use strict";
$(function () {
    let section = localStorage.getItem('section');
    let seeStoresList = $('.see-stores-list');
    const menuItems = document.querySelectorAll(".menu div");
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
    getStores(vendorId);
    let storeItems = $('.store-item');
    seeStoresList.addClass('active');
    storeItems.show();
    menuItems.forEach(item => {
        item.addEventListener("click", function () {
            menuItems.forEach(item => item.classList.remove("active"));
            this.classList.add("active");
            const isStoreList = this.classList.contains('see-stores-list');
            toggleStoreItems(isStoreList);
        });
    });
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
function toggleStoreItems(show) {
    const storeItems = $('.store-item'); // always get fresh reference
    show ? storeItems.show() : storeItems.hide();
}
//# sourceMappingURL=home.js.map