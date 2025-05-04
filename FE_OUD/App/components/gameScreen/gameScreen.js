import ApiHandeler from "../../js/data";
let temp = document.createElement("template");
temp.innerHTML = `
<div class="game-screen"></div>
`
class GameScreen extends HTMLElement {
    shadowRoot
    constructor(){
        super()
        this.shadowRoot = this.attachShadow({mode: "open"})
    }
    connectedCallback(){

    }
    attachStyling(){

    }
    getRoomKey(){
        
    }
}