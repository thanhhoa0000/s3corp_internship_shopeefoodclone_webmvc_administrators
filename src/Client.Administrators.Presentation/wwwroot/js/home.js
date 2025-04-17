$(document).ready(function () {
    let section = JSON.parse(localStorage.getItem('section'));

    if (!section) {
        section = "stores";
        localStorage.setItem('section', JSON.stringify(section));
    }

    document.querySelectorAll(`.main-nav-item`).forEach((item) => {
        item.classList.remove("active");
    })

    $(`.main-nav-item[code-name='${section}']`).addClass("active");
});