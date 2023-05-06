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
                                <a onclick=Delete('/Admin/Product/Delete/${data}') class="btn btn-outline-danger mx-4">
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

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this action!",
        icon: 'warning',
        showCancelButton: true, 
        confirmButtonColor: '#3f9622',
        cancelButtonColor: '#8b0909',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
                })
        }
    })
}