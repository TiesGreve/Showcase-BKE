const temp = document.createElement("template");
temp.innerHTML = `
    <div class="game-board"></div>
`
class GameBoard extends HTMLElement {
    shadow
    constructor(){
        super();
        this.shadow = this.attachShadow({mode: "open"})
    }
    connectedCallback(){
        let content = temp.content.cloneNode(true)
        for(let i = 1; i < 9; i += 3){
            let row =  this.createRow(i);
            content.appendChild(row);
        }
        this.shadow.appendChild(content);
        this.attachStyling();
    }
    attachStyling(){
        const link = document.createElement("link");
        link.setAttribute("rel", "stylesheet");
        link.setAttribute("href", "./components/gameBoard/gameBoard.css")
        this.shadow.appendChild(link);
    }
    createRow(startnumb){
        let row  = document.createElement("div");
        row.setAttribute("class", "row")
        for(let i = startnumb; i < startnumb + 3; i++){
            row.appendChild(this.createCell(i));
        }
        return row;
    }
    createCell(numb){
        let cell = document.createElement("input");
        cell.setAttribute("type", "text")
        cell.setAttribute("class", "cell");
        cell.setAttribute("id", "c" + numb);
        cell.setAttribute("readonly", true);
        cell.addEventListener("click", () => {
        this.dispatchEvent( new CustomEvent("ButtonPressed", {
            bubbles: true,
            detail: {
                number: numb
            }}))
        })
        return cell;
    }
}
customElements.define("game-board", GameBoard);