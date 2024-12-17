import React, { Suspense } from "react";
import { useAtom } from "jotai";
import { checkAuth } from "./atoms/auth.ts";
import Loading from "./components/general/Loading.tsx"; // Assuming your auth atom is exported here

// Lazy-load the components
const Login = React.lazy(() => import("./pages/auth/Login.tsx"));
const UserRoutes = React.lazy(() => import("./UserRoutes.tsx"));
const AdminRoutes = React.lazy(() => import("./AdminRoutes.tsx"));

function App() {
    const [auth] = useAtom(checkAuth);

    const renderContent = () => {
        if (auth == null) {
            return <Login />;
        }
        if (auth === "Player") {
            return <UserRoutes />;
        }
        if (auth === "Admin") {
            return <AdminRoutes />;
        }
    };

    return (

        <Suspense fallback={<Loading/>}>
            {renderContent()}
        </Suspense>
    );
}

export default App;
