import { Children } from "react";

export default function Body({
    children,
  }: Readonly<{
    children: React.ReactNode;
  }>){
    return (
        <div className="w-full max-w-3xl bg-sky-500 h-fit rounded-xl justify-center items-center flex flex-col p-4 drop-shadow-xl">
            {children}
        </div>
    )
}