import ApiHandeler from "./data.js";

const form = document.querySelector("form");
const userName = document.getElementById("UserName");
const email = document.getElementById("Email");
const password = document.getElementById("Password");
const passwordRe = document.getElementById("PasswordCheck");
const submit = document.getElementById("Submit");
const inputs = [userName, email, password, passwordRe];


inputs.forEach(input => {
    input.addEventListener("input", () => {CheckEvent(input)})
})


function CheckEvent(element){
    if(element.validity.valid){
        if(element.id == "Password" && !PasswordRegex){
            submit.disabled = true;
            return;
        }
        if(CheckInputs){
            submit.disabled = false;
        }
    }
    else {
        submit.disabled = true;
    }
}

function CheckInputs(){
    result = true;
    inputs.forEach(input => {
        if(!input.validity.valid){
            result = false;
        }
    })
    return result;
}

function PasswordRegex(){
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,10}$/.test(password.value);
}

function ComparePasswords(){
    return password.value == passwordRe.value;
}

form.addEventListener("submit", async (event) => {
    // Then we prevent the form from being sent by canceling the event

    event.preventDefault();

    // if the email field is valid, we let the form submit
    inputs.forEach(input => {
        if (!input.validity.valid) {
            // If it isn't, we display an appropriate error message
            //showError(input);
            return;
        }
    })
    
    if (ApiHandeler.GetCaptchaResult()) {
        setTimeout(async () => {
            let response = await ApiHandeler.RegisterUser(email.value, userName.value, password.value, passwordRe.value);
            if(response.status === 200){
                window.location.href = "home.html";
            }
        }, 1000
        );
        
    }

});

async function SendData(){

}