var dtble;
$(document).ready(function () {
    loadadata();

});
function loadadata() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetData"
        },
        "columns": [
            { "data": "name" },
            { "data": "description" },
             { "data": "price" },
            { "data": "category.name" },
            {
                "data": "id",
                "render": function (data) {
                    return  ` 
                
                       <a href="/Admin/Product/Edit/${data}" class="btn btn-success">Edit</a>
                       <a onClick=DeleteItem("/Admin/Product/Delete/${data}") class="btn btn-danger">Delete</a>
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