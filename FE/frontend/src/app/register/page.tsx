import Body from "@/components/body/body";
import validationService from "@/services/validationService";

export default function Page() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [passwordComfirmation, setPasswordComfirmation] = useState("");
    let Message = "";
    function ValidateInput(){
        if(!validationService.ValidateEmail(email)){
            InvalidInput("email");
        }
        if(!validationService.ValidatePassWord(password))
            InvalidInput("password");
        if(password !== passwordComfirmation){
            InvalidInput("passwordComf");
        }
                
    }

    function InvalidInput(inputType: string){
        let element = document.querySelector(inputType);
    }

    return (
    <div className="w-full flex flex-col justify-center items-center">
      <h1>TicTacToe</h1>
      <Body>
        <form action="" className="flex flex-col">
            <label>Email</label>
            <input type="email" placeholder="email"></input>
            <label>Password</label>
            <input type="password" placeholder="password"></input>
            <label>Password comfirmation</label>
            <input type="password" placeholder="password comfirmation"></input>
            <div id="errorMessage" className="min-h-10 text-red-500"></div>
            <button disabled className="button">Next</button>
        </form>
      </Body>
    </div>
    )
}

function useState(arg0: string): [any, any] {
    throw new Error("Function not implemented.");
}
