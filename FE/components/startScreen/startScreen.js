const temp = document.createElement("template");
temp.innerHTML = `
<div class="startScreen"></div>
`
class StartScreen extends HTMLElement {
    shadowRoot

    constructor(){
        super();
        this.shadowRoot = this.attachShadow({mode: "open"});
    }
    connectedCallback(){
        this.shadowRoot = temp.content.cloneNode(true);
        this.attachStyling();

    }
    attachStyling(){
        const link = document.createElement("link");
        link.setAttribute("rel", "stylesheet");
        link.setAttribute("href", "./components/startScreen/startScreen.css")
        this.shadowRoot.appendChild(link);
    }
}
customElements.define('start-screen', StartScreen);
