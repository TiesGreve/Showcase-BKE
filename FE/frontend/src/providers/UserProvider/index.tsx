'use client'
import { ReactNode } from "react"
import { createContext } from "vm"

interface UserProviderProps {
    children: ReactNode,
}

interface UserContextProps {
    name: string,
}

const UserContext = createContext<UserContextProps>({
    name: ""
})

const UserProvider = (({children} : UserProviderProps) => {
    return (
        <UserContext.Provider>
            {children}
        </UserContext.Provider>
    )
})

export default UserProvider