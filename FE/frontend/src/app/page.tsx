"use client"
import Body from "@/components/body/body";
import { useRouter } from "next/navigation";

export default function Page() {
  const router = useRouter();
  return (
    <div className="w-full flex flex-col justify-center items-center">
      <h1>TicTacToe</h1>
      <Body>
      <button className="" onClick={() => { router.push("/play") }}>Play</button>
      <button className="" onClick={() => { router.push("/login") }}>Login</button>
      <button className="" onClick={() => { router.push("/register") }}>Register</button>
      </Body>
    </div>
    
  );
}
