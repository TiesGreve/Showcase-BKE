const temp = document.createElement("template");
temp.innerHTML = `
<div class="startScreen"></div>
`
class StartScreen extends HTMLElement {
    shadow;

    constructor(){
        super();
        //this.shadow = this.attachShadow({mode: "open"});
    }
    connectedCallback(){
        
        this.appendChild(temp.content.cloneNode(true));
        this.attachStyling();

    }
    attachStyling(){
        const link = document.createElement("link");
        link.setAttribute("rel", "stylesheet");
        link.setAttribute("href", "./components/startScreen/startScreen.css")
        this.appendChild(link);
    }
}
customElements.define('start-screen', StartScreen);
