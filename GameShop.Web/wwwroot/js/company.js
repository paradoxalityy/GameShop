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
            { "data": "streetAddress", "width": "10%" },
            { "data": "city", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data": "postalCode", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                     return `<div class="w-100 btn group" role="group">
                                <a href="/Admin/Company/Upsert?id=${data}" class="btn btn-outline-primary mx-2">
                                    <i class="bi bi-pencil-square"></i> &nbsp; Edit
                                </a>
                                <a onclick=Delete('/Admin/Product/Delete/${data}') class="btn btn-outline-danger mx-2">
                                    <i class="bi bi-trash3"></i> &nbsp; Delete
                                </a>
                            </div>
                     `
                },
                "width": "20%"
            }
        ]
    });
}