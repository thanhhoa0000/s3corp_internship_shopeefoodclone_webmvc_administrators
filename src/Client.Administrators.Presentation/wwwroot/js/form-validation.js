"use strict";
function formFieldValidate(name, inputField, errorMessageElement, rules, minLength, maxLength) {
    let inputText = String(inputField.val());
    let errorMessage = '';
    rules.forEach(rule => {
        errorMessage = returnErrorMessage(name, inputText, rule, minLength, maxLength);
        if (errorMessage) {
            inputField.css('border-color', primaryRed);
            errorMessageElement.text(errorMessage);
            errorMessageElement.show();
            return;
        }
    });
}
function returnErrorMessage(name, inputText, rule, minLength, maxLength) {
    let errorMessage = '';
    switch (rule) {
        case Rule.IsRequired:
            if (inputText.length === 0) {
                errorMessage = `${name} không được bỏ trống`;
            }
            break;
        case Rule.HasMinLength:
            if (inputText.length > 0 && inputText.length < minLength) {
                errorMessage = `${name} phải chứa ít nhất ${minLength} ký tự`;
            }
            break;
        case Rule.HasMaxLength:
            if (inputText.length > maxLength) {
                errorMessage = `${name} không thể quá ${maxLength} ký tự`;
            }
            break;
    }
    return errorMessage;
}
var Rule;
(function (Rule) {
    Rule[Rule["IsRequired"] = 0] = "IsRequired";
    Rule[Rule["HasMinLength"] = 1] = "HasMinLength";
    Rule[Rule["HasMaxLength"] = 2] = "HasMaxLength";
    Rule[Rule["NotContainsSpecialCharacters"] = 3] = "NotContainsSpecialCharacters";
})(Rule || (Rule = {}));
//# sourceMappingURL=form-validation.js.map