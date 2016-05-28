/// <reference path="External/jquery-2.1.0.min.js" />
/// <reference path="External/jquery-2.1.0-vsdoc.js" />
/// <reference path="External/jquery.validate.min.js" />
/// <reference path="External/jquery.validate-vsdoc.js" />

//if valid  loginFormContainer=190px;
//if !valid loginFormContainer=220px;

function UserNamePasswordValidationChecker() {
        var loginForm = $("#LoginForm");
        var loginFormContainer = $("#LoginFormInnerContainer");
        var usernameTextBox = $('#UserName');
        var passwordTextBox = $('#Password');
        var isUserNameValid = usernameTextBox.valid();
        var isPassworValid = passwordTextBox.valid();

        if (isUserNameValid && isPassworValid) {
            loginFormContainer.animate({
                height: "179px"
            }, "fast");
        };

        if (!isUserNameValid || !isPassworValid) {
            $(".loginErrors").show()
            loginFormContainer.animate({
                height: "204px"
            }, "fast");
        };

        if (!isUserNameValid && !isPassworValid) {
            $(".loginErrors").show()
            loginFormContainer.animate({
                height: "220px"
            }, "fast");
        };
    };

function UserNamePasswordValidationCheckerAfterAjax() {
    var loginForm = $("#LoginForm");
    var loginFormContainer = $("#LoginFormInnerContainer");
    var usernameTextBox = $('#UserName');
    var passwordTextBox = $('#Password');
    var isUserNameValid = usernameTextBox.valid();
    var isPassworValid = passwordTextBox.valid();

    if (isUserNameValid && isPassworValid) {
        loginFormContainer.animate({
            height: "219px"
        }, "fast");
    };

    if (!isUserNameValid || !isPassworValid) {
        $(".loginErrors").show()
        loginFormContainer.animate({
            height: "244px"
        }, "fast");
    };

    if (!isUserNameValid && !isPassworValid) {
        $(".loginErrors").show()
        loginFormContainer.animate({
            height: "260px"
        }, "fast");
    };
};

function ShowLoginForm() {
    var loginForm = $("#LoginForm");
    var loginFormContainer = $("#LoginFormInnerContainer");

    loginFormContainer.animate({
        height: "250px"
    })
    //UserNamePasswordValidationChecker();
	.animate({
		height: "179px"
	}, "fast");


    loginForm.animate({
        'margin-top': "40px"
    })
	.animate({
		'margin-top': "5px"
	}, "fast");

    $(".login_button").toggle();
};

function HideLoginForm() {
    $(".validation-summary-errors").hide();
    $(".loginErrors").hide();
    $(':input', '#LoginForm')
    .not(':button, :submit, :reset, :hidden')
    .val('')
    .removeAttr('checked')
    .removeAttr('selected');

    $("#LoginFormInnerContainer").animate({
        height: "0px"
    }, "fast");

    $('#UserName').on('input', function () {
        UserNamePasswordValidationChecker();
    });

    $('#Password').on('input', function () {
        UserNamePasswordValidationChecker();
    });

    $('#loginBtn').on('click', function () {
        UserNamePasswordValidationChecker();
    });
}

function ShowLoginFormAfterFailureAuth() {
    var loginForm = $("#LoginForm");
    var loginFormContainer = $("#LoginFormInnerContainer");

    loginFormContainer.animate({
        height: "300px"
    })
	.animate({
	    height: "219px"
	}, "fast");


    loginForm.animate({
        'margin-top': "40px"
    })
	.animate({
	    'margin-top': "5px"
	}, "fast");

    $(".login_button").toggle();
};

function RegistrationOfLoginFormEvents() {
    var isAuthError = 'none';

    if ($(".validation-summary-errors").css('display') != undefined) {
        isAuthError = $(".validation-summary-errors").css('display');
    }

    $(".login_button").on('click', ShowLoginForm);

    $("#hide_button").on('click', HideLoginForm);
    

    $('#UserName').on('input', function () {
        if (isAuthError == 'none') {
            UserNamePasswordValidationChecker();
        }
        else {
            UserNamePasswordValidationCheckerAfterAjax();
        }
    });

    $('#Password').on('input', function () {
        if (isAuthError == 'none') {
            UserNamePasswordValidationChecker();
        }
        else {
            UserNamePasswordValidationCheckerAfterAjax();
        }
    });

    $('#loginBtn').on('click', function () {
        if (isAuthError == 'none') {
            UserNamePasswordValidationChecker();
        }
        else {
            UserNamePasswordValidationCheckerAfterAjax();
        }
    });
};

$(document).ready(function () {
    RegistrationOfLoginFormEvents();
});

function successLogin() {
    RegistrationOfLoginFormEvents();
    $.validator.unobtrusive.parse("#LoginForm");
    $("#LoginForm").validate();

    ShowLoginFormAfterFailureAuth();
};

