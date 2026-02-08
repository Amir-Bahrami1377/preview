function SelectParvandehTree(id) {
    document.querySelectorAll(".ParvandehTreeView").forEach(el => el.classList.remove("active"));
    let selectedItem = document.getElementById(id);
    if (selectedItem) {
        selectedItem.classList.add("active");
        return selectedItem;
    }

}