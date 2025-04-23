"use strict";
$(document).on("keypress", '#Name', (event) => {
    const key = event.key;
    if (!/^[a-zA-Z0-9\-|]$/.test(key)) {
        event.preventDefault();
    }
});
$(document).on("input", '#Name', function () {
    this.value = this.value.replace(/[^a-zA-Z0-9\-|]/g, '');
});
$(document).on("keypress", ".customer-phone-number input", function (event) {
    if (!/[0-9]/.test(event.key)) {
        event.preventDefault();
    }
});
//# sourceMappingURL=product-create.js.map