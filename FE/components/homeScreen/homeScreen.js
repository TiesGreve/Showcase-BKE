import StartScreen from "../startScreen/startScreen.js";
import ApiHandeler from "../../js/data.js";

const temp = document.createElement("template");
temp.innerHTML = `                    
<form action="" method="post">
    <div>
        <label for="Email">Email</label>
        <input type="text" id="Email" required>
    </div>
    <div>
        <label for="Password">Password</label>
        <input type="password" id="Password" required>
    </div>
    <input class="home-button" type="submit" id="login-submit" disabled = true value="Login">
</form>`

const names = ["login", "register"];
class HomeScreen extends HTMLElement{
    shadow
    constructor(){
        super();
        //this.shadow = this.attachShadow({mode: "open"});
    }
    connectedCallback(){
        let startScreen = document.createElement("start-screen");
        names.forEach(element => {
            this.setupButton(element, startScreen);
        });
        this.appendChild(startScreen.cloneNode(true))
        this.attachEventListeners();
        this.attachStyling();
    }
    attachStyling(){
        const link = document.createElement("link");
        link.setAttribute("rel", "stylesheet");
        link.setAttribute("href", "./components/homeScreen/homeScreen.css")
        this.appendChild(link);
    }
    setupButton(name, startScreen){
        let button = document.createElement("button");
        button.innerText = name;
        button.setAttribute("id", name);
        button.setAttribute("class", "home-button");
        startScreen.appendChild(button);

    }
    attachEventListeners(){
        let register = document.getElementById("register");
        let login = document.getElementById("login");
        register.addEventListener("click", () =>{
            this.ChangePage("register");
        });
        login.addEventListener("click", (e) => {
            this.innerHTML = "";
            let startScreen = document.createElement("start-screen");
            startScreen.appendChild(temp.content.cloneNode(true))
            this.appendChild(startScreen.cloneNode(true))
            this.setLoginListeners();
            this.attachStyling();
        })
        
    }
    setLoginListeners(){
        let email = document.getElementById("Email");
        let password = document.getElementById("Password");
        let login = document.getElementById("login-submit");
        console.log(email + password + login)
        email.addEventListener("change", () => {
            if(email.value != "" && password.value != "")login.removeAttribute("disabled")
            else login.setAttribute("disabled", true);
        })
        password.addEventListener("change", (e) => {
            if(email.value != "" && password.value != "")login.removeAttribute("disabled")
            else login.setAttribute("disabled", true);
        })
        login.addEventListener("click", async (e) =>  {
            e.preventDefault();
            let result = await ApiHandeler.LoginUser(email.value, password.value);
            if(result){
                window.location.href = "home.html"
            }

        })

    }
    ChangePage(location){
        window.location.href = location + ".html";
    }
    ChangeLayout(){
        document.createElement("")
    }
}
customElements.define("home-screen", HomeScreen);