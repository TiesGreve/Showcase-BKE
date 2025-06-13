import ApiHandeler from "./data.js";

export async function verify() {
    const code = document.getElementById('code').value;
    const token = sessionStorage.getItem('token');
    fetch(ApiHandeler.connectionString + '/2fa/verify', {
    method: 'POST',
    headers: { 
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify({code})
    })
    .then(res => {
    if (res.ok) {
        window.location.href = "home.html";
    } else {
        alert('Invalid 2FA code.');
    }
});
}
export function setup() {
  fetch(ApiHandeler.connectionString +`/2fa/setup/${email.value}`,{
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