$(function () {
    //$('#btn-convert-english').on('click', function () {
    //    converter.convertAmountToRepresentation();
    //});
});

var converter = (function () {
    var getFormData = function () {
        var formData = {};
        var formDataArr = $('form').serializeArray();
        $.each(formDataArr, function () {
            formData[this.name] = this.value;
        });

        return formData;
    };

    var convertCurrencyToRepresentation = function () {
        $.post('/Currency/Convert', getFormData(), function (data) {
            $('#textarea-output').empty();
            $('#error-msg').empty();
            if (data.IsSuccessful) {
                $('#textarea-output').text(data.Data);
            }
            else {
                $('#error-msg').text(data.ResultMessage);
            }
        });
    }

    return {
        convertCurrencyToRepresentation: convertCurrencyToRepresentation
    };
})();