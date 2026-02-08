
function ShowDate() {

    let firstTime = true;
    $(".AmardDate").persianDatepicker({
        calendarType: 'persian',
        responsive: 'true',
        format: 'YYYY/MM/DD',
        autoClose: true,
        initialValue: false,
        initialValueType: 'persian',
    });

}

function ShowTimeDate() {

    let firstTime = true;

    $(".AmardDateTime").persianDatepicker({
        calendarType: 'persian',
        responsive: 'true',
        format: 'YYYY/MM/DD HH:mm:ss',
        autoClose: true,
        initialValue: false,
        initialValueType: 'persian',
        timePicker: {
            enabled: true,
            meridiem: false,
        },
        onShow: function () {
            setTimeout(function () {
                $('.pwt-btn-today').remove();
                $('.pwt-btn-calendar').remove();
                $('.table-days tbody tr').each(function () {
                    $(this).find('td.today').removeClass('today');
                });
                if (firstTime) {
                    $('.table-days tbody tr').each(function () {
                        $(this).find('td.selected').removeClass('selected');
                    });
                    firstTime = false;
                }
            },
                0);
        }
    });

}

function DisabledDate() {
    $(".AmardDate").prop("disabled", true);
}

function EnabledDate() {
    $(".AmardDate").prop("disabled", false);
}

$(".clearDateBtn").on("click", function () {
    $(".AmardDate").val("");
});

function toEnglishDigits(input) {
    const persianNumbers = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
    return input.replace(/[۰-۹]/g, (w) => persianNumbers.indexOf(w).toString());
}
