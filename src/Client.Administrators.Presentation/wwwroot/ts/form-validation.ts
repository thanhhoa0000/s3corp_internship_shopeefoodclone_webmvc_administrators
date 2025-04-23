function formFieldValidate(name: string, inputField: JQuery<HTMLElement>, errorMessageElement: JQuery<HTMLElement>, rules: Rule[], minLength: number, maxLength: number) {
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

function returnErrorMessage(name: string, inputText: string, rule: Rule, minLength: number, maxLength: number): string {
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

enum Rule {
    IsRequired,
    HasMinLength,
    HasMaxLength,
    NotContainsSpecialCharacters,
}
