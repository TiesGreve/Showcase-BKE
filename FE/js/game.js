import ApiHandeler from "./data";

const joinButton = document.querySelector("#JoinGame");
const keyInput = document.querySelector("#GameID");
var pattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;

keyInput.addEventListener("change", (e) => {
    if(pattern.test(keyInput.value)){
        joinButton.disabled = false;
    }
    else joinButton.disabled = true;
})
joinButton.addEventListener("click", async () => {
    let result = await ApiHandeler.JoinGame(keyInput.value)
})