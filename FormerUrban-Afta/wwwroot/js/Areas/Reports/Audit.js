$(document).ready(function () {
    ShowDate();
});

document.addEventListener("submit", function (event) {
    if (event.target.id === "AuditSearchForm")
        AuditSearchFormValidatoin(event);
});

function AuditSearchFormValidatoin(event) {
    var valid = true; 
    
    var date = document.getElementById('Date').value;
    if (date != "" && !window.ValidationService.CheckRequired(date, "تاریخ")) valid = false;

    var IP = document.getElementById('Ip').value;
    if (IP != "" && !window.ValidationService.isValidIpRangeOrCidr(IP)) valid = false;

    var EntityId = document.getElementById('EntityId').value;
    if (EntityId != "" && !window.ValidationService.SanitizeAndValidateInput(EntityId, "شناسه گزارش")) valid = false;

    if (!valid)
        event.preventDefault();
}