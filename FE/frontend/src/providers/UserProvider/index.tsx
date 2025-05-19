'use client'
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

    async function LoginUser(email: string, password: string){
        try{
            let response = await fetch(process.env.API_URL + "/Auth/login", {
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


    return (
        <UserContext.Provider value={{}}>
            {children}
        </UserContext.Provider>
    )
})

export default UserProvider