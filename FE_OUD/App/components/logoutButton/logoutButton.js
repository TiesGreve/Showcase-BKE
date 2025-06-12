import ApiHandeler from "../../js/data.js";

const temp = document.createElement("template");
temp.innerHTML = `
<button class="logout-button">Logout</button>
`
export default class LogoutButton extends HTMLElement {
    shadow;

    constructor(){
        super();
        //his.shadow = this.attachShadow({mode: "open"});
    }
    connectedCallback(){
        
        this.appendChild(temp.content.cloneNode(true));
        this.attachStyling();
        let button = this.querySelector(".logout-button");
        button.addEventListener("click", function() {
            ApiHandeler.Unauthorized();
        })

    }
    attachStyling(){
        const link = document.createElement("link");
        link.setAttribute("rel", "stylesheet");
        link.setAttribute("href", "./components/logoutButton/logoutButton.css")
        this.appendChild(link);
    }
}
customElements.define("logout-button", LogoutButton)