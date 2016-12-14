$(document).ready(function() {
	// $("#btn").click(function(){
	// 	$(selector).post(URL,data,function(data,status,xhr),dataType)
	// 	$.post(getRootPath() + "/user/login",{
	// 		username:$("#uname").val(),
	// 		password: $("#upwd").val()
	// 	})
	// },

	// )	console.log(88)
	// $("body").on("focus",".form-control",function(){
	// 	$(this).closest(".fg-line").addClass(".fp")
	// })

	$("#btn").click(function(){
		$.ajax({
		method: 'post',
		url: getRootPath() + "/user/login",
		dataType: 'json',
		// 用户名和密码
		data: {
			username: $("#uname").val(),
			password: $("#upwd").val()
		},
		success: function(data) {
			console.log(data);
			// 假设后端传回的 1 表示成功， 0 表示用户名或密码错误
			if (data) {
				alert("验证通过");
			} else {
				alert("用户名或密码错误");
			}
		}
	});
	})
	
})