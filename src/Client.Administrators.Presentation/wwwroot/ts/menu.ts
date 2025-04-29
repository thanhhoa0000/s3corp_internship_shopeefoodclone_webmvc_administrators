$(function () {
    let section: string | null = localStorage.getItem('section');

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
});

$('form > button').on('click', function (event) {
    event.preventDefault();

    let validateField = $('input');

    validateField.each(function () {
        let $field = $(this);
        let name = $field.closest('.form-group').find('label')
            .clone().children().remove().end().text().trim();
        let span = $field.closest('.form-group').find('.validate-message');

        formFieldValidate(name, $field, span, [Rule.IsRequired], 0, Number.MAX_SAFE_INTEGER);
    });

    if ($('.validate-message:visible').length === 0) {
        $(this).closest('form')[0].submit();
    }
});

$(document).on('blur', 'input', function () {
    let name = $(this).closest('.form-group').find('label')
        .clone().children().remove().end().text().trim();
    let span = $(this).closest('.form-group').find('.validate-message');

    formFieldValidate(name, $(this), span, [Rule.IsRequired], 0, Number.MAX_SAFE_INTEGER);
});

$(document).on('focus', 'input', function () {
    let span = $('.validate-message');
    $('input').css('border-color', '');
    span.text('');
    span.hide().attr("hidden", "true");
});

$(document).on('click', '.update-title-btn', function () {
    let input: string | null = prompt("Nhập tên menu mới:");

    if (input !== null && input.trim() !== "") {
        const confirmed: boolean = confirm(`Thay đổi menu với tên mới "${input}"?`);

        if (confirmed) {
            const menuId: string | undefined = $(this).closest('.menu-item').attr('menu-id');
            
            $.ajax({
                url: `${gatewayUrl}/menus/${menuId}`,
                type: 'GET',
                success: (response: any): void => {
                    let menu = response.body;

                    $.ajax({
                        url: `/Menu/VendorUpdate/`,
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify({
                            id: menu.id,
                            storeId: menu.storeId,
                            title: input,
                            state: menu.state,
                            concurrencyStamp: menu.concurrencyStamp
                        }),
                        success: (response: any): void => {
                            if (!response.success) {
                                toastr.error("Đã xảy ra lỗi");                                
                                return;
                            }
                            toastr.success("Cập nhật menu thành công");
                            document.dispatchEvent(new Event("menuUpdated"));
                            window.location.reload();
                        },
                        error: function (error: any): void {
                            toastr.error("Đã xảy ra lỗi");
                        }
                    });
                },
                error: function (error: any): void {
                    toastr.error("Đã xảy ra lỗi");
                }
            });
        }
    }
});

$(document).on('click', '.delete-btn', function () {
    const confirmed: boolean = confirm(`Xác nhận xoá menu này?`);

    if (confirmed) {
        const menuId: string | undefined = $(this).closest('.menu-item').attr('menu-id');
        
        console.log(menuId);

        $.ajax({
            url: `/Menu/VendorDelete/`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(menuId),
            success: (response: any): void => {
                if (!response.success) {
                    toastr.error("Đã xảy ra lỗi");
                    return;
                }
                toastr.success("Xoá menu thành công");
                document.dispatchEvent(new Event("menuDeleted"));
                window.location.reload();
            },
            error: function (error: any): void {
                toastr.error("Đã xảy ra lỗi");
            }
        });
    }
});
