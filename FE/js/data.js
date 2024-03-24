/// To do:
// * Check capthca keys
// * join game
// * Make move
// * long polling


export default class ApiHandeler {
    constructor(){

    }
    static async GetRoomCode(userGuid){
        try{
            let response = fetch("https://localhost:7059/api/Game/Create", {
                method: "POST",
                body: JSON.stringify({
                    Guid : userGuid
                }),
                headers: {
                    'Accept' : 'application/json',
                    'Content-Type' : 'application/json'
                }
            });
            let result = await response.json();
            return result.parse();
        }
        catch(ex){
            return new Error(ex)
        }

    }
    static async JoinGame(Guid){
        try{

        }
        catch(e){
            
        }
    }


    static async GetCaptchaResult(){
        grecaptcha.ready(function() {
            grecaptcha.execute('6LfKon4pAAAAAMl9e47gJG3eOx7ePgZ_dJTmuOBF', {action: 'submit'}).then(async function(token) {
                try {
                    const response = await fetch('https://localhost:7060/api/Captcha', {
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