import ApiHandeler from "./data.js"

const tempGebruiker = document.createElement("template");
tempGebruiker.innerHTML = `
    <section class="dashboard-data">
    </section>
    <hr>
    <section class="dashboard-button">
        <button id="play-button" class="home-button">Play</button>
    </section>
`
const tempAdmin = document.createElement("template");
tempAdmin.innerHTML = `
    <table class="table">
        <thead>
            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Email</th>
                <th>block</th>
            </tr>
        </thead>
        <tbody>

        </tbody>
    </table>
`



var role = await ApiHandeler.GetRole();
console.log(role)
if(role.value == "Gebruiker"){
    let screen = document.querySelector("start-screen");
    screen.appendChild(tempGebruiker.content.cloneNode(true))
    document.getElementById("play-button").addEventListener("click", () => {
        window.location.href = "play.html";
    })
}
else{
    let screen = document.querySelector("start-screen");
    screen.appendChild(tempAdmin.content.cloneNode(true));
}