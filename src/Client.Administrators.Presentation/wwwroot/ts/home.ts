$(function() {
    let section: string | null = localStorage.getItem('section');
    let seeStoresList: JQuery<HTMLElement> = $('.see-stores-list');
    const menuItems: NodeListOf<Element> = document.querySelectorAll(".menu div");
    const vendorId: string | undefined = $('.home-stores-section').attr('vendor-id');
    
    if (!section) {
        section = "stores";
        localStorage.setItem('section', JSON.stringify(section));
    } else {
        section = JSON.parse(section);
    }
    
    document.querySelectorAll(".main-nav-item").forEach((item: Element) => {
        item.classList.remove("active");
    });
    
    $(`.main-nav-item[code-name='${section}']`).addClass("active");

    
    
    getStores(vendorId as string);

    let storeItems: JQuery<HTMLElement> = $('.store-item');
    
    seeStoresList.addClass('active');
    storeItems.show();

    menuItems.forEach(item => {
        item.addEventListener("click", function (this: Element) {
            menuItems.forEach(item => item.classList.remove("active"));
            this.classList.add("active");

            const isStoreList = this.classList.contains('see-stores-list');
            toggleStoreItems(isStoreList);
        });
    });
});

function getStores(vendorId: string): void{
    $.ajax({
        url: `/Home/Index?vendorId=${vendorId}`,
        type: 'POST',
        success: (response: string): void => {
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

function toggleStoreItems(show: boolean) {
    const storeItems = $('.store-item'); // always get fresh reference
    show ? storeItems.show() : storeItems.hide();
}
