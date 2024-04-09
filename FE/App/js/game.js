import ApiHandeler from "./data.js";

const createGame = document.getElementById("CreateGame");
const joinButton = document.getElementById("JoinGame");
const keyInput = document.getElementById("GameID");
const copyId = document.getElementById("game-id-copy");

const gameActive = false;

let Usernames = []

let pattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;

if(sessionStorage.getItem("Game") != null) sessionStorage.removeItem("Game");
if(sessionStorage.getItem("Id") != null) sessionStorage.removeItem("Id");
if(sessionStorage.getItem("token") == null) window.location.href = "index.html"

keyInput.addEventListener("change", (e) => {
    if(pattern.test(keyInput.value)){
        joinButton.disabled = false;
        //console.log(keyInput.value)
    }
    else joinButton.disabled = true;
})

joinButton.addEventListener("click", async (e) => {
    e.preventDefault();
    let result = await ApiHandeler.JoinGame(keyInput.value);
    if(result != null){
        let resultJSON = JSON.stringify(result);
        document.getElementById("unblurred").remove();
        Usernames = [await ApiHandeler.GetUsername(result.user1), await ApiHandeler.GetUsername(result.user2)]
        document.getElementById("player1-name").innerText = Usernames[0];
        document.getElementById("player2-name").innerText = Usernames[1];
        document.getElementById("game-id").innerText = result.id
        sessionStorage.setItem("Game", resultJSON);
        setUpGame();
    }
})
createGame.addEventListener("click", async () => {
    let result = await ApiHandeler.CreateGame();
    if(result != null){
        let resultJSON = JSON.stringify(result);
        document.getElementById("unblurred").remove();
        document.getElementById("player1-name").innerText = await ApiHandeler.GetUsername(result.user1);
        document.getElementById("game-id").innerText = result.id
        sessionStorage.setItem("Game", resultJSON);
        setUpGame();
    }
})
copyId.addEventListener("click", () => {
    let game = JSON.parse(sessionStorage.getItem("Game"));
    navigator.clipboard.writeText(game.id);
})

async function setUpGame(){
    let buttons = document.querySelector("game-board").shadow.querySelectorAll(".cell");
    let id = await ApiHandeler.GetUserId();
    sessionStorage.setItem("Id", id.value);
    buttons.forEach(element => {
        element.removeAttribute("readonly");
        element.addEventListener("click", (e) => {
            let game = JSON.parse(sessionStorage.getItem("Game"));
            if(game.currentTurn == sessionStorage.getItem("Id")){
                CheckTurn(element)
            }
        })
    });
}
async function CheckTurn(e){
    let game = JSON.parse(sessionStorage.getItem("Game"));
    if(game.currentTurn == sessionStorage.getItem("Id") && !e.innerText){
        //console.log((e.getAttribute("id").substring(1)))
        let result = await ApiHandeler.MakeMove(parseInt(e.getAttribute("id").substring(1))-1)
        if(result.status === 400){
            return;
        }
        let id = sessionStorage.getItem("Id");
        if(id == game.user1){
            e.value = "X"
        }
        else if(id == game.user2){
            e.value = "O"
        }
        //console.log(e.getAttribute("id"))
    }
}
async function Game(){

    if(sessionStorage.getItem("Game") == null){
        //console.log("No game set")
        await new Promise(resolve => setTimeout(resolve, 1000))
        await Game();
    }
    let game = await ApiHandeler.CheckGame(); 
    if(game.user2 == null){
        //console.log("No user 2")
        await new Promise(resolve => setTimeout(resolve, 1000))
        await Game();
    }
    if(document.getElementById("player2-name").innerText == "name...") {
        Usernames = [await ApiHandeler.GetUsername(game.user1), await ApiHandeler.GetUsername(game.user2)]
        document.getElementById("player2-name").innerText = Usernames[1];
    }
    if(game.gameFinish == null){    
        sessionStorage.setItem("Game", JSON.stringify(game))
        UpdateBoard(game.boardState);
        ShowTurn();
        //console.log("game bussy")
        await new Promise(resolve => setTimeout(resolve, 1000))
        await Game();
    }
}
function UpdateBoard(board){
    let buttons = document.querySelector("game-board").shadow.querySelectorAll(".cell");
    //console.log(buttons)
    //console.log(board)
    for(let i = 0; i < buttons.length; i++){
        buttons[i].value = board[i];
    }
    //console.log(buttons.length)
}
function ShowTurn(){
    let game = JSON.parse(sessionStorage.getItem("Game"));
    let turnText = document.getElementById("Current-Turn")
    if(game.currentTurn == game.user1){
        turnText.innerText = Usernames[0]
    }
    else if(game.currentTurn == game.user2){
        turnText.innerText = Usernames[1]
    }
}
Game();