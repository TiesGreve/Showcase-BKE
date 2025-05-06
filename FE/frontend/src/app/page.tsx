import Body from "@/components/body/body";
import Link from "next/link";

export default function Page() {
  
  return (
    <div className="w-full flex flex-col justify-center items-center">
      <h1>TicTacToe</h1>
      <Body>
      <Link href="/play" className="">Play</Link>
      <Link href="/login" className="">Login</Link>
      <Link href="/register" className="">Register</Link>
      </Body>
    </div>
    
  );
}
