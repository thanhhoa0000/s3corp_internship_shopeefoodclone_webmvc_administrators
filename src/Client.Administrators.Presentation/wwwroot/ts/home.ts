$(function() {
    let section: string | null = localStorage.getItem('section');
    let seeStoresList: JQuery<HTMLElement> = $('.see-stores-list');
    const menuItems: JQuery<HTMLElement> = $('.menu div');
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
    
    seeStoresList.addClass('active');

    menuItems.on('click', function () {
        const isStoreList = $(this).hasClass('see-stores-list');
        const isCreateStore = $(this).hasClass('add-store');

        menuItems.removeClass('active');
        $(this).addClass('active');

        toggleStoreItems(isStoreList);

        if (isStoreList) {
            getStores($('.home-stores-section').attr('vendor-id') as string);
        }

        if (isCreateStore) {
            getCreateStoreForm();
        }
    });

    seeStoresList.trigger('click');
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

function getCreateStoreForm(){
    $.ajax({
        url: `/Store/Create`,
        type: 'GET',
        success: (response: string): void => {
            let parsed = $('<div>').html(response);
            $('.main-content').replaceWith(parsed.find('.main-content'));

            if (gatewayUrl) {
                getProvinces();
            } else {
                document.addEventListener("configLoaded", () => getProvinces(), { once: true });
            }
        },
        error: (xhr, status, error) => {
            console.error("Failed to get form");
        }   
    });
}

function toggleStoreItems(show: boolean) {
    const storeItems = $('.store-item');
    show ? storeItems.show() : storeItems.hide();
}
