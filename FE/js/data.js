/// To do:
// * Check capthca keys
// * join game
// * Make move
// * Check game
// * long polling

export default class ApiHandeler {
    static connectionString = "https://localhost:7059";
    constructor(){

    }
    static async GetUserId(){
        try{
            const token = localStorage.getItem('token');
            const response = fetch(connectionString + "/api/Auth/Id", {
                method: "GET",
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': token
                }
            })
            let result = await response.json();
            return result.parse();
        }
        catch(ex){
            return new Error(ex)
        }
    }
    static async GetUserName(){
        try{
            const token = localStorage.getItem('token');
            const response = fetch(connectionString + "/api/Auth/Name", {
                method: "GET",
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': token
                }
            })
            let result = (await response).json();
            return result.parse();
        }
        catch(ex){
            return new Error(ex)
        }
    }
    static async CreateGame(){
        try{
            const token = localStorage.getItem('token');
            const response = fetch(connectionString + "/api/Game/Create", {
                method: "POST",
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': token
                }
            });
            let result = await response.json();
            return result.parse();
        }
        catch(ex){
            return new Error(ex)
        }

    }
    static async JoinGame(guid){
        try{
            const token = localStorage.getItem('token');
            const response = fetch(connectionString + "/api/Game/Join",{
                method: "POST",
                body: JSON.stringify({
                    Guid: guid
                }),
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': token
                }
            })
            let result = await response.json()
            return result.parse();
        }
        catch(ex){
            return new Error(ex);
        }
    }

    static async MakeMove(cell){
        try{
            const token = localStorage.getItem('token');
            let gameID = localStorage.getItem("GameId");
            let response = fetch(connectionString + '/api/Game/Move',{
                method: "POST",
                body: JSON.stringify({
                    Cell : cell,
                    GameID : gameID
                }),
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': token
                }
            })
            let result = await response.json()
        }
        catch(e){

        }
    }
    static async CheckGame(){
        try{
            const token = localStorage.getItem('token');
            const response = await fetch(connectionString + '/api/Game/id?guid='+localStorage.getItem("GameId", {
                method: "GET",
                headers:  {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': token
                }
            }))
        }
        catch(e){

        }
    }

    static async GetCaptchaResult(){
        grecaptcha.ready(function() {
            grecaptcha.execute('6LfKon4pAAAAAMl9e47gJG3eOx7ePgZ_dJTmuOBF', {action: 'submit'}).then(async function(token) {
                try {
                    const response = await fetch(connectionString + '/api/Captcha', {
                        method: "POST",
                        body: JSON.stringify({
                            Recaptcha: token
                        }),
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        }
                    });
                    let result = await response.json();
                    console.log(result)
    
                    result = JSON.parse(result)
                    console.log(result)
                    console.log(response)
                    if(result == undefined){
                        throw Error("result undefined");
                    }
                    if (result.score > 0.5) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                catch (e) {
                    return false;
                }
            })
        })
    }
}