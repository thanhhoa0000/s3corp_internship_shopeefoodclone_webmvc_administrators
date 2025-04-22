$(function() {
    let section: string | null = localStorage.getItem('section');
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
    
    getStores(vendorId || '');
});

function getStores(vendorId: string): void{
    $.ajax({
        url: `/Home/Index?vendorId=${vendorId}`,
        type: 'POST',
        success: (response: string): void => {
            let parsed = $('<div>').html(response);
            $('.main-content').replaceWith(parsed.find('.main-content'));
        },
        error: (xhr, status, error) => {
            console.error("Failed to get stores");
        }
    });
}
