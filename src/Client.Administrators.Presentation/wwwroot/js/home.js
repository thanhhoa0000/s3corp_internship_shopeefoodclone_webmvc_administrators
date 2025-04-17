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
//# sourceMappingURL=home.js.map