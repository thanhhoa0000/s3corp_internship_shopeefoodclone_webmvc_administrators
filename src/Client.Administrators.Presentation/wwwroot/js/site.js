"use strict";
document.addEventListener("DOMContentLoaded", () => {
    const navLinks = document.querySelectorAll(".main-nav .main-nav-item");
    navLinks.forEach(link => {
        link.addEventListener("click", function () {
            navLinks.forEach(item => item.classList.remove("active"));
            this.classList.add("active");
            const section = this.getAttribute('code-name');
            if (section) {
                getSectionItems(section);
            }
        });
    });
});
function getSectionItems(section) {
    if (window.location.pathname !== "/") {
        window.location.href = "/";
    }
    localStorage.setItem('section', JSON.stringify(section));
}
//# sourceMappingURL=site.js.map