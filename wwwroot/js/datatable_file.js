$(document).ready(function () {
    GetCustomer();
});
function GetCustomer() {
    $.ajax({
        url: '/Excel/GetCustomerList',
        type: 'Get',
        dataType: 'json',
        success: OnSuccess
    })
}

function OnSuccess(response) {
   $('#dataTableData').DataTable({
        bProcessing: true,
        bLenghtChange: true,
        lengthMenu: [[5, 10, 25, -1], [5, 10, 25, "All"]],
        bfilter: true,
        bSort: true,
        bPaginate: true,
        data: response,
        columns: [
            {
                data: 'Id',
                render: function (data, type, row, meta) {
                    return row.id
                }
            },
            {
                data: 'CustomerCode',
                render: function (data, type, row, meta) {
                    return row.customerCode
                }
            },
            {
                data: 'FirstName',
                render: function (data, type, row, meta) {
                    return row.firstName
                }
            },
            {
                data: 'LastName',
                render: function (data, type, row, meta) {
                    return row.lastName
                }
            },
            {
                data: 'Gender',
                render: function (data, type, row, meta) {
                    return row.gender
                }
            },
            {
                data: 'Age',
                render: function (data, type, row, meta) {
                    return row.age
                }
            },
            {
                data: 'Country',
                render: function (data, type, row, meta) {
                    return row.country
                }
            }
       ]
      
    });
}
