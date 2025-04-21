window.addEventListener("load", adjustMainContentHeight);
window.addEventListener("resize", adjustMainContentHeight);

document.addEventListener("DOMContentLoaded", () => {
    const navLinks: NodeListOf<Element> = document.querySelectorAll(".main-nav .main-nav-item");

    navLinks.forEach(link => {
        link.addEventListener("click", function (this: Element) {
            navLinks.forEach(item => item.classList.remove("active"));
            this.classList.add("active");

            const section = this.getAttribute('code-name');
            if (section) {
                getSectionItems(section);
            }
        });
    });
});

function getSectionItems(section: any): void {
    if (window.location.pathname !== "/") {
        window.location.href = "/";
    }

    localStorage.setItem('section', JSON.stringify(section));
}

function adjustMainContentHeight() {
    const header = document.getElementById("main-nav");
    const footer = document.querySelector("footer");
    const mainContent = document.querySelector(".main-content") as HTMLElement;

    if (!header || !footer || !mainContent) return;

    const headerHeight = header.offsetHeight + getVerticalMargins(header);
    const footerHeight = footer.offsetHeight + getVerticalMargins(footer);

    const availableHeight = window.innerHeight - headerHeight - footerHeight;
    
    mainContent.style.height = `${availableHeight}px`;
}

function getVerticalMargins(element: Element): number {
    const style = window.getComputedStyle(element);
    const marginTop = parseFloat(style.marginTop || "0");
    const marginBottom = parseFloat(style.marginBottom || "0");
    return marginTop + marginBottom;
}