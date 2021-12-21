$("#facebook-button").on("click", function () {
    document.getElementById('fb-form-button').click();
});
$("#google-button").on("click", function () {
    document.getElementById('google-form-button').click();
});

//<script src="https://www.google.com/recaptcha/api.js?render=@this.Configuration[" GoogleReCaptcha:Key"]" ></script >
grecaptcha.ready(function () {
    grecaptcha.execute('@this.Configuration["GoogleReCaptcha:Key"]', { action: '/Identity/Account/Login' }).then(function (token) {
        document.getElementById("RecaptchaValue").value = token;
    });
});
