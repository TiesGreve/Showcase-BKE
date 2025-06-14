import ApiHandeler from "./data.js";

export async function verify() {
    const code = document.getElementById('code').value;
    const email = document.getElementById("Email").value;
    const password = document.getElementById("Password");
    fetch(ApiHandeler.connectionString + '/2fa/verify', {
    method: 'POST',
    headers: { 
        'Content-Type': 'application/json',
    },
    body: JSON.stringify({
        code: code,
        email: email
    })
    })
    .then(async res => {
    if (res.ok) {
        await ApiHandeler.LoginUser(email, password)
        window.location.href = "home.html";
    } else {
        alert('Invalid 2FA code.');
    }
});
}
export function setup(email) {
  fetch(ApiHandeler.connectionString +`/2fa/setup/${email}`,{
            method: "GET",
                headers:  {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',}}
                )
    .then(res => res.json())
    .then(data => {
        console.log(data)
        document.getElementById('qr').src = data;
        
    });
}

export default {verify, setup}