import Body from "@/components/body/body";
import RegisterScreen from "@/components/register-screen/register-screen";

export default function Page() {
  
    return (
    <div className="w-full flex flex-col justify-center items-center">
      <h1>TicTacToe</h1>
      <Body>
        <RegisterScreen/>
      </Body>
    </div>
    )
}
