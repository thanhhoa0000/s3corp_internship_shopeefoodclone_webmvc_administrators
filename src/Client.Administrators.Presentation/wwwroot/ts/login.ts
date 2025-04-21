$(function() {
    let section: string | null = localStorage.getItem('section');

    if (section) {
        section = JSON.parse(section);

        document.querySelectorAll(".nav-link").forEach((item: Element) => {
            item.classList.remove("active");
        });

        const activeItem = document.querySelector(`.nav-link[code-name='${section}']`);
        if (activeItem) {
            activeItem.classList.add("active");
        }
    }    
});

$(function() {
    $.validator.unobtrusive.parse('form');
    
    $('form input').on('blur', function () {
        $(this).valid();
    });
});
