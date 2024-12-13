import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../atoms/auth.ts";

function Logout() {
    const user = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (user) {
            // Perform logout
            user.logout();
        }
        // Redirect to login page after logout or if not logged in
        navigate("/login");
    }, [user, navigate]);

    return (
        <div className="flex h-screen items-center justify-center">
            <p className="text-lg text-gray-600 dark:text-gray-400">Redirecting to login...</p>
        </div>
    );
}

export default Logout;
