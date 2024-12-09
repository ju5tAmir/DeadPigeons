import DropdownMenu from "./DropdownMenu.tsx";
import Logo from "../../../public/logo.png";

const Navbar = () => {

    const menuItems = [
        {
            title: "Home",
            path: "/"
        },
        {
            title: "Games",
            path: "/game"
        },
        {
            title: "Packages",
            path: "/packages"
        },
        {
            title: "Users",
            path: "/users"
        },
        {
            title: "Transactions",
            path: "/transactions"
        }
    ]

    return (
        <>
            <div className="flex justify-between p-4 place-items-center bg-[#2b2d31] font-mono">
                {/* Logo */}
                <div className="flex flex-row pl-5">
                    <a href="/" className={"flex flex-row items-center gap-2"}>
                        <img src={Logo} width="50" height="50" />

                        <span className="font-extrabold text-4xl text-white">Jerne</span>
                        <span className="text-3xl text-slate-300 font-bold -ml-1.5   ">IF</span>
                    </a>
                </div>

                {/* Middle items */}
                <div className="flex w-auto text-lg pr-32">
                    {
                        menuItems.map ((item) => (
                            <>
                                <ul className="px-6 font-bold text-xl text-white hover:text-slate-100">
                                <li>
                                        <a href={item.path}>{item.title}</a>
                                    </li>
                                </ul>
                            </>
                        ))
                    }
                </div>

                <div
                    className="flex flex-row items-center h-8 gap-5">
                    {/*<ShoppingCardIcon/>*/}
                    <DropdownMenu/>
                </div>
                {/* Dropdown menu */}
            </div>
        </>
    )
}

export default Navbar;