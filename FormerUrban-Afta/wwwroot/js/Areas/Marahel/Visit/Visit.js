$(document).ready(function () {
    var shop = document.getElementById("shop").value;
    var shod = document.getElementById("shod").value;
});

document.addEventListener("click", function (event) {
    let item = event.target.closest(".ParvandehTreeView");
    if (item && item.id) {
        var selectedItem = SelectParvandehTree(item.id);
        //if (selectedItem) {
        //    var shop = selectedItem.getAttribute("data-shop");
        //    ShowParvandehInfo(shop);
        //}
    }
});