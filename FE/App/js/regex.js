export function PasswordRegex(pssw){
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$/.test(pssw);
}
export function EmailRegex(email){
    return /^(?=.{6,128}$)[\w.-]+@([\w-]+\.)+[\w-]{2,6}$/.test(email);
}
export function StringValidation(stringInput, maxlenght){
    let isSQLInjection = /(\b(SELECT|INSERT|DELETE|UPDATE|DROP|UNION|OR|AND)\b|--|;|'|"|\/\*|\*\/|xp_)/.test(stringInput);
    let isScriptInjection = /<\s*script\b|on\w+\s*=|javascript:|data:text\/html|<\s*iframe\b|<\s*img\b[^>]*on\w+\s*=|document\.|window\.|eval\(/.test(stringInput);
    return !(isSQLInjection || isScriptInjection || stringInput.lenght > maxlenght)
}
export default {EmailRegex, PasswordRegex, StringValidation}