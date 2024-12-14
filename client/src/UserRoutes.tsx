import Navbar from "./components/user/general/Navbar.tsx";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {RoutePath} from "./utils/user/RoutePath.ts";
import Lab from "./pages/Lab.tsx";
import {Toaster} from "react-hot-toast";
import Footer from "./components/general/Footer.tsx";
import GamesOverview from "./components/user/game/GamesOverview.tsx";
import PlayGame from "./components/user/game/PlayGame.tsx";
import BoardsOverview from "./components/user/board/BoardsOverview.tsx";
import Login from "./pages/auth/Login.tsx";
import GameDetails from "./components/user/game/GameDetails.tsx";
import UserProfile from "./components/user/account/UserProfile.tsx";
import Logout from "./components/user/account/Logout.tsx";
import Transactions from "./components/user/transactions/Transactions.tsx";
import CreateTransaction from "./components/user/transactions/CreateTransaction.tsx";

function UserRoutes() {
    return (
        <>
            <div className="flex flex-col min-h-screen">
                {/* Navbar stays at the top */}
                <Navbar/>

                {/* Main content stretches to fill the remaining space */}
                <main className="flex-grow">
                    <BrowserRouter>
                        <Routes>
                            <Route path={RoutePath.lab} element={<Lab/>}/>
                            <Route path={RoutePath.games} element={<GamesOverview/>}/>
                            <Route path={RoutePath.games}>
                                <Route path=":id" element={<GameDetails />} />
                            </Route>
                            <Route path={RoutePath.boards} element={<BoardsOverview/>}/>
                            <Route path={RoutePath.login} element={<Login/>}/>
                            <Route path={RoutePath.profile} element={<UserProfile/>}/>
                            <Route path={RoutePath.logout} element={<Logout/>}/>
                            <Route path={RoutePath.transactions} element={<Transactions/>}/>
                            <Route path={RoutePath.transactions}>
                                <Route path={"create"} element={<CreateTransaction/>}/>
                                <Route path={":id"} element={<CreateTransaction/>}/>
                            </Route>
                        </Routes>
                    </BrowserRouter>
                    <Toaster
                        position="top-center"
                        reverseOrder={false}
                    />
                </main>

                {/* Footer stays at the bottom */}
                <Footer/>
            </div>
        </>
    )
}

export default UserRoutes
