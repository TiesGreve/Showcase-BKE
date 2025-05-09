export function getDataFromToken (token: string, key: string){
    let base64Url = token.split('.')[1];
    let base64 = base64Url.replace('-', '+').replace('_', '/');
    let jsonPayload = JSON.parse(window.atob(base64));
    const value = jsonPayload[key];
    return value;
}
export function ValidatePassword(password: string){
    let regexText = "/^"    +
    "(?=.*?[A-Z])"          +       // Uppercase
    "(?=.*?[a-z])"          +       // Lowercase
    "(?=.*?[0-9])"          +       // Number
    "(?=.*?[#?!@$%^&*-])."  +       // Special character
    "{11,126}"              +       // Lenght of 12
    "$/"
    
    const regex = new RegExp(regexText)

    return regex.test(password)
}
export function ValidateEmail(email: string){
    let regex = /e/
    return regex.test(email)
}