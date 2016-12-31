$(function() {
    var root = getRootPath();
    var currentPasswordBox = $("#current_password");
    var currentAckPasswordBox = $("#current_ack_password");
    $("#current_user_save").on("click", function() {
        if(!currentPasswordBox.val() || !currentAckPasswordBox.val()) {
            alert("密码不能为空！");
            return;
        }
        if(currentPasswordBox.val() != currentAckPasswordBox.val()) {
            alert("密码不一致！");
        } else {
            $.ajax({
                url: root + "/user/UpdateCurrentUserPassword",
                type: "post",
                dataType: "json",
                data: {
                    password: currentPasswordBox.val()
                },
                success: function(result) {
                    if(result.status) {
                        alert("修改成功！");
                        $("#current_user_modal").modal("hide");
                        window.location.href = root + "/user/logout";
                    } else {
                        alert(result.message);
                    }
                }
            });
        }
    });

    $("btn_current_pwd").on("click", function() {
        currentPasswordBox.val("");
        currentAckPasswordBox.val("");
    });
})