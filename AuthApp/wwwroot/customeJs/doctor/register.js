var Register = {

    clearValidationerror: function (ErrorId) {
        $(`#${ErrorId}`).text("");
    },
    NextForm: function () {
        var email = $('input[name="Email"]').val();
        var password = $('input[name="Password"]').val();
        var isValid = true;
        var confirmedPassword = $('input[name="ConfirmedPassword"]').val();
        var stepper = new Stepper(document.querySelector('.bs-stepper'));
        $("#emailError").text("");
        $("#passwordError").text("");
        $("#confirmedPasswordError").text("");
        if (email === '' || email === "" || email === null) {
            $("#emailError").text("email is required");
            isValid = false;
        }
        if (password === undefined || password === "" || password=== '') {
            $("#passwordError").text("password is required");
            isValid = false;
        }
        if (confirmedPassword === undefined || confirmedPassword === "" || confirmedPassword === '') {
            $("#confirmedPasswordError").text("confirm password is required");
            isValid = false;
        }else if (password != confirmedPassword ) {
            $("#confirmedPasswordError").text("The password and confirmation password do not match.");
            isValid = false;
        }
        if (isValid) { stepper.next(); /*$("#personal-info").show(); $("#account-details").hide(); */ } else { return; }
    },
    SubmitForm: function () {
        var form1Data = $("#AccountForm").serialize();
        var form2Data = $("#PersonalForm").serialize();
        var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();
        general.callAjax("/Doctor/Auth/Register", form1Data + '&' + form2Data + '&__RequestVerificationToken=' + antiForgeryToken,
            function (response) {
                console.log(response);
                if (response.status == 1) {
                    toastr['info']('the register is success !! 😀', 'success', {
                        positionClass: 'toast-top-right',
                        rtl: true,
                        timeOut: 1000,
                        onHidden: function () {
                            window.location.href = '/Doctor/Dashboard/Index';
                        }
                    });
                } else {
                    console.log(response);
                    //alert(response.serverInfo.message);
                }
            }
            , true);
      
    }

};