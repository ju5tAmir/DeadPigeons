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
import UsersOverview from "./components/admin/users/UsersOverview.tsx";
import ViewUser from "./components/admin/users/ViewUser.tsx";
import EditUser from "./components/admin/users/EditUser.tsx";
import UserManager from "./components/admin/users/UserManager.tsx";
import CreateUser from "./components/admin/users/CreateUser.tsx";
import UpdateOfflineProperties from "./components/admin/game/UpdateOfflineProperties.tsx";
import Login from "./pages/auth/Login.tsx";
import UserProfile from "./components/user/account/UserProfile.tsx";
import Logout from "./components/user/account/Logout.tsx";
import Transactions from "./components/admin/transactions/Transactions.tsx";
import TransactionDetails from "./components/admin/transactions/TransactionDetails.tsx";

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
                                    <Route path="update" element={<UpdateOfflineProperties/>}/>
                                </Route>
                            </Route>
                            <Route path={RoutePath.users} element={<UsersOverview/>}/>
                            <Route path={RoutePath.users}>
                                <Route path="create" element={<CreateUser/>} />
                                <Route path=":id" element={<UserManager />}></Route>
                                    <Route path=":id">
                                        <Route path="update" element={<EditUser/>}/>
                                    </Route>
                                </Route>
                            <Route path={RoutePath.boards} element={<BoardsOverview/>}/>
                            <Route path={RoutePath.login} element={<Login/>}/>
                            <Route path={RoutePath.profile} element={<UserProfile/>}/>
                            <Route path={RoutePath.logout} element={<Logout/>}/>
                            <Route path={RoutePath.transactions} element={<Transactions/>}/>
                            <Route path={RoutePath.transactions}>
                                <Route path={":id"} element={<TransactionDetails/>}/>
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

export default AdminRoutes
