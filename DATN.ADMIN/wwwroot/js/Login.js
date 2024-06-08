function Login() {
    var emailElement = document.getElementById("email");
    var passwordElement = document.getElementById("password");
    if (!trimmedEmail || trimmedEmail.trim() === "") {
        common.showNotification("Bạn chưa nhập địa chỉ Email/tên đăng nhập nhận", "error");
        emailElement.style.border = "2px solid red";
        return;
    }
    // không quá 50 ký tự
    if (trimmedEmail.length > 50) {
        common.showNotification("Email/tên đăng nhập không được quá 50 ký tự", "error");
        emailElement.style.border = "2px solid red";
        return;
    }
    // check password
    if (!passwordElement.value || passwordElement.value.trim() === "") {
        common.showNotification("Bạn chưa nhập mật khẩu", "error");
        passwordElement.style.border = "2px solid red";
        return;
    }
    // không quá 50 ký tự
    if (passwordElement.value.length > 50) {
        common.showNotification("Mật khẩu không được quá 50 ký tự", "error");
        passwordElement.style.border = "2px solid red";
        return;
    }

}