import { useEffect } from "react";
import toast from "react-hot-toast";
import { useNavigate, useSearchParams } from "react-router-dom";
import { http } from "../../http.ts";

export default function EmailConfirm() {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    // Get token and email from the query params
    const token = searchParams.get("token");
    const email = searchParams.get("email");

    // If token or email is missing, redirect to home
    if (!token || !email) {
        navigate("/"); // Redirect to home
    }

    // Check the token and email when the component loads
    useEffect(() => {
        if (token && email) {
            // Make a GET request to the backend for email confirmation
            toast.promise(
                http.authConfirmCreate({ token, email }),
                {
                    success: "Email confirmed successfully",
                    error: () => {

                        navigate("/")
                        return "Unable to confirm email. Please check your link."},
                    loading: "Confirming email...",
                }
            ).then(() => {
                navigate("/login"); // Redirect to login after success
            });
        }
    }, [token, email, navigate]);

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
                <h2 className="text-2xl font-semibold text-center text-gray-700 mb-6">Confirming Email...</h2>
                <p className="text-center text-gray-600">Please wait while we confirm your email.</p>
            </div>
        </div>
    );
}
