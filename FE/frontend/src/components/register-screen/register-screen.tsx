"use client"
import { ValidateEmail, ValidatePassword } from "@/utils/auth"
import { useState } from "react"

export default function RegisterScreen(){
    const [message, setMessage] = useState()

    function ValidateInput(email: string, password: string, passwordComfirmation: string){
        try{
            if(!ValidateEmail(email)){
                throw new ValidationError("Invalid Email")
            }
            if(!ValidatePassword(password)){
                throw new ValidationError("Invalid password")
            }
            if(password != passwordComfirmation){
                throw new ValidationError("Passwords don't match")
            }
            return true
        }
        catch(err){
            return err as ValidationError
        }
    }
    

    return (
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
    )
}