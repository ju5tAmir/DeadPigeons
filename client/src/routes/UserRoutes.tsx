import React, { Suspense, lazy } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import Navbar from "../components/user/general/Navbar.tsx";
import Footer from "../components/general/Footer.tsx";
import { RoutePath } from "../utils/user/RoutePath.ts";

// Lazy-loaded components
const GamesOverview = lazy(() => import("../components/user/game/GamesOverview.tsx"));
const BoardsOverview = lazy(() => import("../components/user/board/BoardsOverview.tsx"));
const GameDetails = lazy(() => import("../components/user/game/GameDetails.tsx"));
const UserProfile = lazy(() => import("../components/user/account/UserProfile.tsx"));
const Logout = lazy(() => import("../components/user/account/Logout.tsx"));
const Transactions = lazy(() => import("../components/user/transactions/Transactions.tsx"));
const CreateTransaction = lazy(() => import("../components/user/transactions/CreateTransaction.tsx"));
const TransactionDetails = lazy(() => import("../components/user/transactions/TransactionDetails.tsx"));

function UserRoutes() {
    return (
        <>
            <div className="flex flex-col min-h-screen">
                {/* Navbar stays at the top */}
                <Navbar />
                <main className="flex-grow">
                    <Suspense fallback={<div className="flex justify-center items-center h-screen">Loading...</div>}>
                        <Routes>
                            {/* Redirect root path ("/") to "/games" */}
                            <Route path="/" element={<Navigate to={RoutePath.games} replace />} />

                            {/* Catch-all route for invalid paths */}
                            <Route path="*" element={<Navigate to={RoutePath.games} replace />} />

                            {/* Main routes */}
                            <Route path={RoutePath.games} element={<GamesOverview />} />
                            <Route path={RoutePath.games}>
                                <Route path={RoutePath.id} element={<GameDetails />} />
                            </Route>
                            <Route path={RoutePath.boards} element={<BoardsOverview />} />
                            <Route path={RoutePath.profile} element={<UserProfile />} />
                            <Route path={RoutePath.logout} element={<Logout />} />
                            <Route path={RoutePath.transactions} element={<Transactions />} />
                            <Route path={RoutePath.transactions}>
                                <Route path={RoutePath.create} element={<CreateTransaction />} />
                                <Route path={RoutePath.id} element={<TransactionDetails />} />
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

export default UserRoutes;
