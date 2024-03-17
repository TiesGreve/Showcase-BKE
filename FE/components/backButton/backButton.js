const temp = document.createElement("template");
temp.innerHTML = `
<button class="back-button">< Back</button>
`
class BackButton extends HTMLElement{
    constructor(){
        super();
    }
    connectedCallback(){
        this.appendChild(temp.content.cloneNode(true))
        let back = this.querySelector(".back-button");
        back.addEventListener("click", function() {
            console.log("Hallo")
            history.back();
        })
        this.attachStyling();
    }
    attachStyling(){
        const link = document.createElement("link");
        link.setAttribute("rel", "stylesheet");
        link.setAttribute("href", "./components/backButton/backButton.css")
        this.appendChild(link);
    }

}
customElements.define("back-button", BackButton)