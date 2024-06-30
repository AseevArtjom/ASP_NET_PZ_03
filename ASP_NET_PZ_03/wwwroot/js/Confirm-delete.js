$(document).ready(function () {
    $(".confirm-action").on("click", function (e) {
        e.preventDefault();
        var confirm_msg = $(this).data("confirm-message");
        var success_msg = $(this).data("success-message");
        var error_msg = $(this).data("error-message");
        var url = $(this).attr("href");

        const CustomSwal = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-success",
                cancelButton: "btn btn-danger"
            },
            buttonsStyling: false
        });

        console.log(url);

        CustomSwal.fire({
            title: confirm_msg,
            showCancelButton: true,
            confirmButtonText: "Ok",
            cancelButtonText: "Cancel",
            didOpen: () => {
                document.querySelector('.swal2-confirm').style.marginRight = '10px';
                document.querySelector('.swal2-actions').style.justifyContent = 'space-between';
            }
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    }
                })
                    .then(r => r.json())
                    .then(data => {
                        if (data.ok) {
                            CustomSwal.fire(success_msg, "", "success")
                                .then(() => {
                                    window.location.reload();
                                });
                        } else {
                            CustomSwal.fire(error_msg, "", "error");
                        }
                    })
                    .catch(error => {
                        console.log(error);
                        CustomSwal.fire(error_msg, "", "error");
                    });
            }
        });
    });
});
