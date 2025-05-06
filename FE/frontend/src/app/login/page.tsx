import Body from "@/components/body/body";

export default function Page() {

    return (
        <div className="w-full flex flex-col justify-center items-center">
              <h1>Login</h1>
              <Body>
                <form className="flex flex-col">
                    <label>Email</label>
                    <input type="email"></input>
                    <label>Password</label>
                    <input type="password"></input>
                    <button className="button text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"> login</button>
                </form>
              </Body>
            </div>
    )
}