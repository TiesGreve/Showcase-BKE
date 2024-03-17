import StartScreen from "../startScreen/startScreen.js";
const names = ["play", "login", "register"];
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
        names.forEach(element => {
            let button = document.getElementById(element);
            button.addEventListener("click", () =>{
                console.log("hallo")
                this.ChangePage(element);
            });
        })
        this.attachStyling()
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
    ChangePage(location){
        window.location.href = location + ".html";
    }
}
customElements.define("home-screen", HomeScreen);