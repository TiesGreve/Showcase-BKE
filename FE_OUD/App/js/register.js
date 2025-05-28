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

function ValidatePassword(){
    if(!PasswordRegex()) {
        showError(password, "check password")
        return false;
    }
    else if(!ComparePasswords()){
        showError(password, "")
        showError(passwordRe, "niet goed")
        return false;
    }
    return true;
}
function CheckOnScripting(input) {
    if(input.indexOf('<') != -1 && input.indexOf('>') != -1){
        return true;
    }
    return false;
}
function showError(element, message){
    element.classList.add("Error-Input");
    let errorBox = document.querySelector("#Error-Message")
    if(errorBox.innerText == ""){
        errorBox.innerText = message;
    }
    console.log(message)
}
function removeError(){
    inputs.forEach(element => {
        element.classList.remove("Error-Input")
    });
    document.querySelector("#Error-Message").innerText = "";
}

form.addEventListener("submit", async (event) => {
    
    event.preventDefault();
    inputs.forEach(input => {
        if (!input.checkVisibility() && input.innerText != "") {
            // If it isn't, we display an appropriate error message
            showError(input, "Niet alles is ingevuld");
            return;
        }
        if(CheckOnScripting(input.innerText)){
            showError(input, `'<' en  '>' zijn niet toegestaan`)
        }
    })
    if(!ValidatePassword()) return
    
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