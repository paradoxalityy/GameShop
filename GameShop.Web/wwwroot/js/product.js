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
            { "data": "price", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group role="group">
                                <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-outline-primary mx-4">
                                    <i class="bi bi-pencil-square"></i> &nbsp; Edit
                                </a>
                                <a href="/Admin/Product/Delete?id=${data}" class="btn btn-outline-danger mx-4">
                                    <i class="bi bi-trash3"></i> &nbsp; Delete
                                </a>
                            </div>
                    `
                },
                "width":"15%"
            }
        ]
    });
}