var datatable;
$(document).ready(function () {
    LoadDataTable();
});

function LoadDataTable() {
    datatable = $('#myTable').DataTable({
        "ajax": {
            url: '/Admin/order/getallorders',
        },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'applicationUser.email', "width": "20%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            { data: 'paymentStatus', "width": "10%" },
            { data: 'orderStatus', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<a href="/Admin/Order/OrderDetails?orderId=${data}"class="btn btn-primary mx-2>
                    <i class="bi bi-pencil-square"></i>
                    </a>`

                },
                "width": "5%"
            }

        ]
    });
}