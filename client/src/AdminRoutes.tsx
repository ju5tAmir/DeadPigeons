import React, { Suspense, lazy } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import Navbar from "./components/admin/general/Navbar.tsx";
import Footer from "./components/general/Footer.tsx";
import { RoutePath } from "./utils/admin/RoutePath.ts";

// Lazy-loaded components
const Lab = lazy(() => import("./pages/Lab.tsx"));
const GamesOverview = lazy(() => import("./components/admin/game/GamesOverview.tsx"));
const GameDetails = lazy(() => import("./components/admin/game/GameDetails.tsx"));
const GameManager = lazy(() => import("./components/admin/game/GameManager.tsx"));
const FinishGame = lazy(() => import("./components/admin/game/FinishGame.tsx"));
const UpdateOfflineProperties = lazy(() => import("./components/admin/game/UpdateOfflineProperties.tsx"));
const UsersOverview = lazy(() => import("./components/admin/users/UsersOverview.tsx"));
const UserManager = lazy(() => import("./components/admin/users/UserManager.tsx"));
const CreateUser = lazy(() => import("./components/admin/users/CreateUser.tsx"));
const EditUser = lazy(() => import("./components/admin/users/EditUser.tsx"));
const BoardsOverview = lazy(() => import("./components/user/board/BoardsOverview.tsx"));
const Login = lazy(() => import("./pages/auth/Login.tsx"));
const UserProfile = lazy(() => import("./components/user/account/UserProfile.tsx"));
const Logout = lazy(() => import("./components/user/account/Logout.tsx"));
const Transactions = lazy(() => import("./components/admin/transactions/Transactions.tsx"));
const TransactionDetails = lazy(() => import("./components/admin/transactions/TransactionDetails.tsx"));
const TransferMoney = lazy(() => import("./components/admin/transactions/TransferMoney.tsx"));

function AdminRoutes() {
    return (
        <>
            <div className="flex flex-col min-h-screen">
                {/* Navbar stays at the top */}
                <Navbar />

                <main className="flex-grow">
                    <Suspense fallback={<div className="flex justify-center items-center h-screen">Loading...</div>}>
                        <Routes>
                            {/* Redirect root path ("/") to "/game" */}
                            <Route path="/" element={<Navigate to={RoutePath.game} replace />} />

                            {/* Catch-all route for invalid paths */}
                            <Route path="*" element={<Navigate to={RoutePath.game} replace />} />

                            {/* Main routes */}
                            <Route path={RoutePath.lab} element={<Lab />} />
                            <Route path={RoutePath.game} element={<GamesOverview />} />
                            <Route path={RoutePath.game}>
                                <Route path=":id" element={<GameManager />} />
                                <Route path=":id">
                                    <Route path="finish" element={<FinishGame />} />
                                    <Route path="update" element={<UpdateOfflineProperties />} />
                                </Route>
                            </Route>
                            <Route path={RoutePath.users} element={<UsersOverview />} />
                            <Route path={RoutePath.users}>
                                <Route path="create" element={<CreateUser />} />
                                <Route path=":id" element={<UserManager />} />
                                <Route path=":id/update" element={<EditUser />} />
                            </Route>
                            <Route path={RoutePath.boards} element={<BoardsOverview />} />
                            <Route path={RoutePath.login} element={<Login />} />
                            <Route path={RoutePath.profile} element={<UserProfile />} />
                            <Route path={RoutePath.logout} element={<Logout />} />
                            <Route path={RoutePath.transactions} element={<Transactions />} />
                            <Route path={RoutePath.transactions}>
                                <Route path=":id" element={<TransactionDetails />} />
                                <Route path="transfer" element={<TransferMoney />} />
                            </Route>
                        </Routes>
                    </Suspense>
                </main>

                {/* Footer stays at the bottom */}
                <Footer />
            </div>
        </>
    );
}

export default AdminRoutes;
