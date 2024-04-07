import ApiHandeler from "./data";

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
            await SendData();
        }, 1000
        );
        window.location.href("home.html");
    }

});

async function SendData(){
    try{
        let response = await fetch("https://localhost:7059/api/Auth/register", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json' 
            },
            body: JSON.stringify({
                Email: email.value,
                UserName: userName.value,
                Password: password.value,
                PasswordCheck: passwordRe.value
            })
        }).then(res => {
            if(!res.ok){
                
            }
            else{

            }
        })
    }
    catch(e){

    }
}