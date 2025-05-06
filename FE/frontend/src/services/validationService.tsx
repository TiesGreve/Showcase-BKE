
export default class validationService{
     static ValidatePassWord(password: string){
        let regexText = "/^"    +
        "(?=.*?[A-Z])"          +       // Uppercase
        "(?=.*?[a-z])"          +       // Lowercase
        "(?=.*?[0-9])"          +       // Number
        "(?=.*?[#?!@$%^&*-])."  +       // Special character
        "{11,}"                 +       // Lenght of 12
        "$/"
        
        const regex = new RegExp(regexText)

        return regex.test(password)
    }
    static ValidateEmail(email: string){
        let regex = /e/

        return regex.test(email)
    }
}