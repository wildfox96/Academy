function ShowLoader() {
    $("#spinner").fadeIn(200);;
}
function HideLoader() {
    $("#spinner").fadeOut(200);
}


function FillDropdownDataAsync(url, dataToPost, $targetDropdown, placeholder, useLoader, selectedValue) {
    console.log("'FillDropdownAsync' was called");
    var blockAsync = "";

    if (useLoader)
        ShowLoader();
    try {
        $.ajax({
            url: url,
            async: false,
            data: dataToPost,
            dataType: "json",
            type: "POST",
            error: function (error) {
                console.log(error);

                if (useLoader)
                    HideLoader();

            },
            success: function (data) {
                blockAsync = "async is blocked";

                var items = "";

                items += "<option value='' selected='selected'>" + placeholder + "</option>";


                $.each(data, function (i, item) {

                    items += "<option value=\"" + item.Value + "\">" + item.Text + "</option>";


                });

                $targetDropdown.html(items);
                if (selectedValue)
                    $targetDropdown.val(selectedValue);

                if (useLoader)
                    HideLoader();
            }
        });
    } catch (e) {
        console.log("'FillDropdownAsync' error" + e);

        if (useLoader)
            HideLoader();
    }
    return blockAsync;
}


var clearForm = function ($form) {
    console.log("'clearFormErrors' was called");

    //clear inputs
    $form.find(".field").removeClass("error");

    //clearFormErrors validation summury
    $form.find(".validation-summary-valid").find("li").remove();
    $form.find(".validation-summary-errors").find("li").remove();
    $form.find(".validation-summary-errors").removeClass("validation-summary-errors")
                                            .addClass("validation-summary-valid");
}

var displayErrors = function (form, errors) {

    console.log("'displayErrors' was called");
    var errorSummary = getValidationSummaryErrors(form)
            .removeClass('validation-summary-valid')
            .addClass('validation-summary-errors');

    var errorsAray = new Array();

    $.isArray(errors)
        ? errorsAray = errors
        : errorsAray.push(errors);




    var items = $.map(errorsAray, function (error) {
        return '<li>' + error + '</li>';
    }).join('');

    var ul = errorSummary
            .find('ul')
            .empty()
            .append(items);
};

var getValidationSummaryErrors = function ($form) {

    console.log("'getValidationSummaryErrors' was called");
    // We verify if we created it beforehand
    var errorSummary = $form.find('.validation-summary-errors, .validation-summary-valid');
    if (!errorSummary.length) {
        errorSummary = $('<div class="validation-summary-errors"><span>Пожалуйста, исправьте ошибки и попытайтесь снова.</span><ul></ul></div>')
                .prependTo($form);
    }

    return errorSummary;
};