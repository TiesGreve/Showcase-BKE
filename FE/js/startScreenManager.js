class StartScreenManager{
    play
    login
    register
    constructor(){
        this.play = document.getElementById("Play");
        this.login = document.getElementById("Login");
        this.register = document.getElementById("Register");
        this.RegisterButtonEvents();

    }
    RegisterButtonEvents(){
        this.play.addEventListener("click", () =>{
            this.ChangePage("");                        //Dit moet nog toegevoegd worden
        })
        this.login.addEventListener("click", () =>{
            this.ChangePage("");                        //Dit moet nog toegevoegd worden
        })
        this.register.addEventListener("click", () =>{
            console.log("hallo")
            this.ChangePage("register");
        })
    }
    ChangePage(location){
        window.location.href = location + ".html";
    }
}
let startScreenManager = new StartScreenManager();