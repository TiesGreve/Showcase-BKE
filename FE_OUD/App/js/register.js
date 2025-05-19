import ApiHandeler from "./data.js";

const form = document.querySelector("form");
const userName = document.getElementById("UserName");
const email = document.getElementById("Email");
const password = document.getElementById("Password");
const passwordRe = document.getElementById("PasswordCheck");
const submit = document.getElementById("Submit");
const inputs = [userName, email, password, passwordRe];


inputs.forEach(input => {
    input.addEventListener("input", () => {CheckEvent()})
})


function CheckEvent(){
    if(CheckInputs()){
        submit.disabled = false;
    }
    else {
        submit.disabled = true;
    }
}

function CheckInputs(){
    let result = true;
    inputs.forEach(input => {
        if(!input.validity.valid){
            console.log(input.id)
            result = false;
            return;
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
function CheckOnScripting(input) {
    if(input.indexOf('<') != -1 && input.indexOf('>') != -1){
        return true;
    }
    return false;
}

form.addEventListener("submit", async (event) => {
    event.preventDefault();
    inputs.forEach(input => {
        if (!input.checkVisibility() && input.innerText != "") {
            // If it isn't, we display an appropriate error message
            showError(input);
            return;
        }
    })
    
    if (ApiHandeler.GetCaptchaResult()) {
        setTimeout(async () => {
            let response = await ApiHandeler.RegisterUser(email.value, userName.value, password.value, passwordRe.value);
            console.log(response)
            if(response.status === 200){
                window.location.href = "home.html";
            }
        }, 1000
        );
        
    }

});

async function SendData(){

}