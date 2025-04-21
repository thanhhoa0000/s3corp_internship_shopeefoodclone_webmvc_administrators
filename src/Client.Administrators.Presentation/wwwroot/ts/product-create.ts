$(document).on("keypress", '#Name', (event: JQuery.KeyPressEvent) => {
    const key = event.key;
    if (!/^[a-zA-Z0-9\-|]$/.test(key)) {
        event.preventDefault();
    }
});

$(document).on("input", '#Name', function (this: HTMLInputElement) {
    this.value = this.value.replace(/[^a-zA-Z0-9\-|]/g, '');
});

$(document).on("keypress", ".customer-phone-number input", function (event: JQuery.KeyPressEvent) {
    if (!/[0-9]/.test(event.key)) {
        event.preventDefault();
    }
});

