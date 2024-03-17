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
    grecaptcha.ready(function() {
        grecaptcha.execute('6LfKon4pAAAAAMl9e47gJG3eOx7ePgZ_dJTmuOBF', {action: 'submit'}).then(async function(token) {
            try {
                console.log(token)
                const response = await fetch('https://localhost:7059/api/Captcha', {
                    method: "POST",
                    body: JSON.stringify({
                        Recaptcha: token
                    }),
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });
                let result = await response.json();
                console.log(result)

                result = JSON.parse(result)
                console.log(result)
                console.log(response)

                let humanFactor;
                let isHuman;
                if (result.score > 0.5) {
                    humanFactor = 'Het lijkt erop dat je een mens bent, je score is: ' + result.score;
                    isHuman = true;
                }
                else {
                    humanFactor = 'Het lijkt erop dat je geen mens bent, je score is: ' + result.score;
                    isHuman = false;
                }

                // Onderstaande code: pas aan naar je eigen smaak

                if (isHuman) {
                    // Wacht 3 seconden en verstuur dan het formulier alsnog
                    setTimeout(async () => {
                        await SendData();
                    }, 1000
                    );
                    window.location.href("home.html");
                }

            }
            catch (e) {
                loading.classList.remove("lds-ellipsis");
                error.textContent = 'Het bevestigen van de captcha is mislukt: ' + e.message;
                error.classList.add("error");
            }
        })
    })
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