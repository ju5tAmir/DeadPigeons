import Navbar from "./components/admin/general/Navbar.tsx";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {RoutePath} from "./utils/admin/RoutePath.ts";
import Lab from "./pages/Lab.tsx";
import {Toaster} from "react-hot-toast";
import Footer from "./components/general/Footer.tsx";
import GamesOverview from "./components/admin/game/GamesOverview.tsx";
import PlayGame from "./components/user/game/PlayGame.tsx";
import BoardsOverview from "./components/user/board/BoardsOverview.tsx";
import GameDetails from "./components/admin/game/GameDetails.tsx";
import GameManager from "./components/admin/game/GameManager.tsx";
import FinishGame from "./components/admin/game/FinishGame.tsx";

function AdminRoutes() {
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
                            <Route path={RoutePath.game} element={<GamesOverview/>}/>
                            <Route path={RoutePath.game}>
                                <Route path=":id" element={<GameManager/>}/>
                                <Route path=":id">
                                    <Route path="finish" element={<FinishGame/>}/>
                                </Route>
                            </Route>
                            <Route path={RoutePath.boards} element={<BoardsOverview/>}/>
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

export default AdminRoutes
