$(document).ready(function () {
    let cate = JSON.parse(localStorage.getItem('section'));

    if (!cate) {
        cate = "food";
        localStorage.setItem('section', JSON.stringify("stores"));
    }

    document.querySelectorAll(`.main-nav-item`).forEach((item) => {
        item.classList.remove("active");
    })

    $(`.main-nav-item[code-name='${cate}']`).addClass("active");
});