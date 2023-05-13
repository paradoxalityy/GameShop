var companiesDataTable;

$(document).ready(function () {
    loadCompaniesDataTable();
})

function loadCompaniesDataTable() {
    companiesDataTable = $('#companiesDataTable').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "postalCode", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",
                "render": function () {
                    `<div class="w-75 btn group" role="group">
                        <a href="/Admin/Company/Upsert?id=${data}" class="btn btn-outline-primary mx-4">
                            <i class="bi bi-pencil-square"></i> &nbsp; Edit
                        </a>
                        <a onclick=Delete('/Admin/Product/Delete/${data}') class="btn btn-outline-danger mx-4">
                            <i class="bi bi-trash3"></i> &nbsp; Delete
                        </a>
                    </div>
                    `
                },
                "width": "15%"
            }
        ]
    });
}