var dtble;
$(document).ready(function () {
    loadadata();

});
function loadadata() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Order/GetData"
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
             { "data": "phoneNumber" },
            { "data": "applicationUser.email" },
            { "data": "orderSatus" },
            { "data": "totalPrice" },
            {
                "data": "id",
                "render": function (data) {
                    return  ` 
                
                       <a href="/Admin/Order/Details?orderid=${data}" class="btn btn-warning">Details</a>
                      
                       `
                }
            }
        ]

    });
}

function DeleteItem(url) {
    
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "Delete",
                success:function(data){
                if(data.succes){
                   
                    toastr.success(data.message);
                    dtble.ajax.reload();
            }else {
                toastr.error(data.message);
            }
                }
});
            Swal.fire({
                title: "Deleted!",
                text: "Your file has been deleted.",
                icon: "success"
            });
        }
    });
}