// Write your Javascript code.
(function () {
    $("#selectLanguage select").change(function () {
        $(this).parent().submit();
    });
}());
