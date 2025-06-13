import ApiHandeler from "./data.js";
import {verify, setup} from "./2fa.js"

const forms = document.querySelectorAll("form");
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
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$/.test(password.value);
}
function EmailRegex(){
    return /^(?=.{6,128}$)[\w.-]+@([\w-]+\.)+[\w-]{2,6}$/.test(email.value);
}
function StringValidation(stringInput, maxlenght){
    let isSQLInjection = /(\b(SELECT|INSERT|DELETE|UPDATE|DROP|UNION|OR|AND)\b|--|;|'|"|\/\*|\*\/|xp_)/.test(stringInput);
    let isScriptInjection = /<\s*script\b|on\w+\s*=|javascript:|data:text\/html|<\s*iframe\b|<\s*img\b[^>]*on\w+\s*=|document\.|window\.|eval\(/.test(stringInput);
    return !(isSQLInjection || isScriptInjection || stringInput.lenght > maxlenght)
}

function ComparePasswords(){
    return password.value == passwordRe.value;
}
function ValidateUsername(){
    if(!StringValidation(userName, 50)){
        showError(userName, "Invalid username, check lenght")
        return false;
    }
    return true;
}

function ValidateEmail(){
    if(!EmailRegex()){
        showError(email, "nonvalid email")
        return false;
    }
    if(!StringValidation(email.value, 80)){
        showError(email, "No tempering or injection!")
        return false;
    }
    return true;
}

function ValidatePassword(){
    if(!PasswordRegex()) {
        showError(password, "nonvalid password")
        return false;
    }
    if(!StringValidation(password.value, 128)){
        showError(password,  "No tempering or injection!")
    }
    else if(!ComparePasswords()){
        showError(password, "")
        showError(passwordRe, "passwords don't match")
        return false;
    }
    return true;
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

forms[0].addEventListener("submit", async (event) => {
    event.preventDefault();
    inputs.forEach(input => {
        if (!input.checkVisibility() && input.innerText != "") {
            showError(input, "Not every field is filled in");
            return;
        }
    })
    if(!ValidateUsername()) return
    if(!ValidateEmail()) return
    if(!ValidatePassword()) return
    
    if (ApiHandeler.GetCaptchaResult()) {
        setTimeout(async () => {
            let response= await ApiHandeler.RegisterUser(email.value, userName.value, password.value, passwordRe.value);
            if(response.status === 200){
                forms[0].style.display = "none";
                forms[1].style.display = "inline";
                await ApiHandeler.LoginUser(email.value, password.value)
                setup()
            }
        }, 1000
        );
        
    }

});

forms[1].addEventListener("submit", async (event) => {
    event.preventDefault();
    await verify();
}
)

