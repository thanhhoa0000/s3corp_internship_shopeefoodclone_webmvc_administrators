document.addEventListener("DOMContentLoaded", function () {
    const navLinks = document.querySelectorAll(".main-nav .main-nav-item");

    navLinks.forEach(link => {
        link.addEventListener("click", function () {
            navLinks.forEach(item => item.classList.remove("active"));
            this.classList.add("active");
        });
    });
});

function getSectionItems(section) {
    if (window.location.pathname !== "/") {
        window.location.href = "/";
    }

    localStorage.setItem('section', JSON.stringify(section));
}
