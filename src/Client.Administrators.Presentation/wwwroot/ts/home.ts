$(function() {
    let section: string | null = localStorage.getItem('section');
    const vendorId: string | undefined = $('.home-stores-section').attr('vendor-id');
    
    console.log(vendorId);
    
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
    
    getStores(vendorId || '', 5, 1);

    $(document).on('click', '.pagination a',function (){
        let pageNumber: number = Number($(this).attr('page') ?? 1);

        console.log(vendorId);
        
        getStores(vendorId || '', 5, pageNumber);
    });
});

function getStores(vendorId: string, pageSize = 5, pageNumber = 1): void{
    $.ajax({
        url: `/Home/Index?vendorId=${vendorId}&pageSize=${pageSize}&pageNumber=${pageNumber}`,
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
