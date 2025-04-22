"use strict";
$(function () {
    let section = localStorage.getItem('section');
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
    getStores(vendorId || '');
});
function getStores(vendorId) {
    $.ajax({
        url: `/Home/Index?vendorId=${vendorId}`,
        type: 'POST',
        success: (response) => {
            let parsed = $('<div>').html(response);
            $('.main-content').replaceWith(parsed.find('.main-content'));
        },
        error: (xhr, status, error) => {
            console.error("Failed to get stores");
        }
    });
}
//# sourceMappingURL=home.js.map