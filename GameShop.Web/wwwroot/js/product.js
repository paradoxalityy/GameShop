var productsDataTable;

$(document).ready(function () {
    loadProductsDataTable();
})

function loadProductsDataTable() {
    productsDataTable = $('#productsDataTable').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "developer", "width": "15%" },
            { "data": "publisher", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            { "data": "platform.name", "width": "15%" },
            { "data": "price", "width": "15%" }
        ]
    });
}