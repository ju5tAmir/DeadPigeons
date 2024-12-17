import React, { Suspense } from "react";
import { useAtom } from "jotai";
import { checkAuth } from "./atoms/auth.ts";
import Loading from "./components/general/Loading.tsx";
import { Navigate, Route, Routes } from "react-router-dom";
import ResetPassword from "./pages/auth/ResetPassword.tsx";
import { RoutePath } from "./utils/user/RoutePath.ts";
import ChangePassword from "./pages/auth/ChangePassword.tsx";
import EmailConfirm from "./pages/auth/EmailConfirm.tsx"; // Assuming your RoutePath is exported here

// Lazy-load the components
const Login = React.lazy(() => import("./pages/auth/Login.tsx"));
const UserRoutes = React.lazy(() => import("./UserRoutes.tsx"));
const AdminRoutes = React.lazy(() => import("./AdminRoutes.tsx"));

function App() {
    const [auth] = useAtom(checkAuth);

    return (
        <Suspense fallback={<Loading />}>
            <Routes>
                {/* Reset Password Route */}
                <Route path={"/password/reset"} element={<ResetPassword />} />
                <Route path={"/password/change"} element={<ChangePassword />} />
                <Route path={"/email/confirm"} element={<EmailConfirm />} />

                {/* Auth-based Routes */}
                {auth === null && (
                    <Route path="*" element={<Login />} />
                )}
                {auth === "Player" && (
                    <Route path="*" element={<UserRoutes />} />
                )}
                {auth === "Admin" && (
                    <Route path="*" element={<AdminRoutes />} />
                )}

                {/* Fallback for any invalid route */}
                <Route path="*" element={<Navigate to={RoutePath.login} replace />} />
            </Routes>
        </Suspense>
    );
}

export default App;
