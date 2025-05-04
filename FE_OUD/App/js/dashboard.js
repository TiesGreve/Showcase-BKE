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
                <th>Is locked out</th>
                <th>toggle lockout</th>
            </tr>
        </thead>
        <tbody>

        </tbody>
    </table>
`

if(sessionStorage.getItem("token") == null){
    ApiHandeler.Unauthorized();
}

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
    FillTable();
}

async function FillTable(){
    let users = await ApiHandeler.GetUsers();
    console.log(users)
    users.forEach(element => {
        let row = document.createElement("tr");
        row.setAttribute("class", "row")
        let id = document.createElement("td");
        id.appendChild(CreateText(element.id))
        row.appendChild(id);
        let Name = document.createElement("td");
        Name.appendChild(CreateText(element.userName))
        row.appendChild(Name);
        let Email = document.createElement("td");
        Email.appendChild(CreateText(element.email))
        row.appendChild(Email);
        let IsLockout = document.createElement("td");
        IsLockout.appendChild(CreateText(!!element.LockoutButton))
        row.appendChild(IsLockout);
        let LockoutButton = document.createElement("td");
        row.appendChild(LockoutButton);
        
        document.querySelector("tbody").appendChild(row);
        
    });
}

function CreateText(string){
    let p = document.createElement("p");
    p.innerText = string;
    return p;
}