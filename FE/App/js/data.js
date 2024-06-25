/// To do:
// * Check capthca keys
// * join game
// * Make move
// * Check game
// * long polling

export default class ApiHandeler {
    static connectionString = "http://tiesshowcase.hbo-ict.org";
    constructor(){
        
    }
    

    static async LoginUser(email, password){
        try{
            let response = await fetch(this.connectionString + "/api/Auth/login", {
                method: "POST",
                headers: {
                    'accept' : 'application/json',
                    'Content-Type' : 'application/json'
                },
                body: JSON.stringify({
                    Email: email,
                    Password: password
                })
            })
            let result = await response.json();
            if (response.ok){
                sessionStorage.setItem("token", result)
            }
            return response.ok
        }
        catch(e){

        }
    }

    static async RegisterUser(email, username, password, passwordRe){
        try{
            let response = await fetch(this.connectionString + "/api/Auth/register", {
                method: "POST",
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json'
                },
                body: JSON.stringify({
                    Email: email,
                    UserName: username,
                    Password: password,
                    PasswordCheck: passwordRe
                })
            })
            return response;
        }
        catch(e){
    
        }
    }

    static async GetUserId(){
        try{
            const token = sessionStorage.getItem('token');
            const response = await fetch(this.connectionString + "/api/Auth/Id", {
                method: "GET",
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': 'Bearer ' + token
                }
            })
            if(response.status === 401) throw new Error(response.status);
            let result = await response.json();
            return result;
        }
        catch(ex){
            this.Unauthorized()
            return new Error(ex)
        }
    }
    static async GetOwnUsername(){
        try{
            const token = sessionStorage.getItem('token');
            const response = await fetch(this.connectionString + "/api/Auth/Name", {
                method: "GET",
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': 'Bearer ' + token
                }
            })
            if(response.status === 401) throw new Error(response.status);
            let result = await response.json();
            return result;
        }
        catch(ex){
            this.Unauthorized()
            return new Error(ex)
        }
    }
    static async GetUsername(id){
        try{
            const token = sessionStorage.getItem('token');
            const response = await fetch(this.connectionString + "/api/Auth/Name/" + id, {
                method: "GET",
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': 'Bearer ' + token
                }
            })
            if(response.status === 401) throw new Error(response.status);
            let result = await response.json();
            return result;
        }
        catch(ex){
            this.Unauthorized()
            return new Error(ex)
        }
    }
    static async CreateGame(){
        try{
            const token = sessionStorage.getItem('token');
            const response = await fetch(this.connectionString + "/api/Game/Create", {
                method: "POST",
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': 'Bearer ' + token
                }
            })
            if(response.status === 401) throw new Error(response.status);
            let result = await response.json();
            return result;
        }
        catch(ex){
            this.Unauthorized()
            return new Error(ex)
        }

    }
    static async JoinGame(guid){
        try{
            const token = sessionStorage.getItem('token');
            const response = await fetch(this.connectionString + "/api/Game/Join",{
                method: "POST",
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': 'Bearer ' + token
                },
                body: JSON.stringify({
                    GameId : guid
                })
            })
            if(response.status === 401) throw new Error(response.status);
            let result = await response.json()
            return result;
        }
        catch(ex){
            this.Unauthorized();
            return new Error(ex);
        }
    }

    static async MakeMove(cell){
        try{
            const token = sessionStorage.getItem('token');
            let game = JSON.parse(sessionStorage.getItem("Game"));
            let response = await fetch(this.connectionString + '/api/Game/Move',{
                method: "POST",
                body: JSON.stringify({
                    Cell : cell,
                    GameID : game.id
                }),
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': 'Bearer ' + token
                }
            })
            if(response.status === 401) throw new Error(response.status);
            let result = await response.text();
            return result;
        }
        catch(ex){
            this.Unauthorized()
            return new Error(ex)
        }
    }
    static async CheckGame(){
        try{
            const token = sessionStorage.getItem('token');
            let game = JSON.parse(sessionStorage.getItem("Game"));
            const response = await fetch(this.connectionString + '/api/Game/'+ game.id , {
                method: "GET",
                headers:  {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': 'Bearer ' + token
                }
            })
            if(response.status === 401 ) throw new Error(response.status);
            let result = await response.json();
            return result;
        }
        catch(ex){
            this.Unauthorized()
            return new Error(ex)
        }
    }

    static async GetCaptchaResult(){
        grecaptcha.ready(function() {
            grecaptcha.execute('6LfKon4pAAAAAMl9e47gJG3eOx7ePgZ_dJTmuOBF', {action: 'submit'}).then(async function(token) {
                try {
                    const response = await fetch(this.connectionString + '/api/Captcha', {
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
                    result = JSON.parse(result)
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
    static async GetRole(){
        try{
            const token = sessionStorage.getItem('token');
            let game = JSON.parse(sessionStorage.getItem("Game"));
            const response = await fetch(this.connectionString + '/api/Auth/Role' , {
                method: "GET",
                headers:  {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': 'Bearer ' + token
                }
            })
            if(response.status === 401 ) throw new Error(response.status);
            let result = await response.json();
            return result;
        }
        catch(ex){
            this.Unauthorized()
            return new Error(ex)
        }
    }

    static async GetUsers(){
        try{
            const token = sessionStorage.getItem('token');
                const response = await fetch(this.connectionString + "/api/Admin/", {
                method: "GET",
                headers:  {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': 'Bearer ' + token
                }
            })
            if(response.status === 401 ) throw new Error(response.status);
            let result = await response.json();
            return result;
        }
        catch(ex){
            this.Unauthorized()
            return new Error(ex)
        }
    }
    static async UpdateUserLockout(guid, lockout){
        try{
            const token = sessionStorage.getItem('token');
            const response = await fetch(this.connectionString + "/api/Admin/" + guid, {
                method: "GET",
                headers:  {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json',
                    'Authorization': 'Bearer ' + token
                },
                body: JSON.stringify({

                })
            })
            if(response.status === 401 ) throw new Error(response.status);
            let result = await response.json();
            return result;
        }
        catch(ex){
            this.Unauthorized()
            return new Error(ex)
        }
    }

    static Unauthorized(){
        sessionStorage.removeItem("token");
        window.location.href = "index.html"
    }
}