'use client'
import axios from "axios"
import { useRouter } from "next/router"
import { ReactNode, createContext, useState } from "react"
import Cookies from 'js-cookie'
import { getDataFromToken, ValidateEmail, ValidatePassword} from "@/utils/auth"

interface UserProviderProps {
    children: ReactNode,
}

interface UserContextProps {
    
}

const UserContext = createContext<UserContextProps>({
    
})

const UserProvider = (({children} : UserProviderProps) => {
    const router = useRouter();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [twoFaCode, setTwoFaCode] = useState('');

    let accessToken = Cookies.get("accessToken")
    let role;
    if(accessToken){
        role = getDataFromToken(accessToken, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
        if(role === "Player") router.push("/account/dashboard");
        else if(role === "Admin") router.push("/admin");
    }

    function LoginUser(email: string, password: string){
        axios.post('/api/auth/login',{
            Email: email,
            Password: password,
        })
    }
    
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
        <UserContext.Provider value={{}}>
            {children}
        </UserContext.Provider>
    )
})

export default UserProvider