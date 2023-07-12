var ordersDataTable;

$(document).ready(function () {
    loadOrdersDataTable();
})

function loadOrdersDataTable() {
    ordersDataTable = $('#ordersDataTable').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "applicationUser.name", "width": "15%" },
            { "data": "applicationUser.phoneNumber", "width": "15%" },
            { "data": "applicationUser.email", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/Admin/Order/Details?orderId=${data}" class="btn btn-outline-primary mx-4">
                                    <i class="bi bi-pencil-fill"></i> &nbsp;Details
                                </a>
                            </div>
                    `
                },
                "width": "5%"
            }]
        })
}