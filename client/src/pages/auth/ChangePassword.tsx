import { useForm, SubmitHandler } from "react-hook-form";
import toast from "react-hot-toast";
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useNavigate } from "react-router-dom";
import { http } from "../../http.ts";
import { useEffect, useState } from "react";


// Validation schema for password reset
const schema = yup
    .object({
        password: yup
            .string()
            .min(8, "Password must be at least 8 characters long")
            .matches(/[A-Z]/, "Password must contain at least one uppercase letter")
            .matches(/\d/, "Password must contain at least one digit")
            .matches(/[^A-Za-z0-9]/, "Password must contain at least one special character")
            .required("Password is required"),
        confirmPassword: yup
            .string()
            .oneOf([yup.ref("password")], "Passwords must match") // Removed the null part
            .required("Confirm password is required"),
    })
    .required();

interface ResetPasswordForm {
    password: string;
    confirmPassword: string;
}

export default function ResetPassword() {
    const navigate = useNavigate();
    const [token, setToken] = useState<string | null>(null);
    const [email, setEmail] = useState<string | null>(null);

    // Retrieve token and email from the query string
    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const tokenFromUrl = queryParams.get("token");
        const emailFromUrl = queryParams.get("email");

        if (tokenFromUrl && emailFromUrl) {
            setToken(tokenFromUrl);
            setEmail(emailFromUrl);
        } else {
            navigate("/"); // Redirect if either token or email is missing
        }
    }, [navigate]);

    // Use react-hook-form for form handling
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<ResetPasswordForm>({
        resolver: yupResolver(schema),
    });

    const onSubmit: SubmitHandler<ResetPasswordForm> = (data) => {
        if (token && email) {
            // Make API call for password reset
            toast.promise(
                http.authActivateCreate(
                    {
                        email: email,
                        token: token,
                        password: data.password,
                    }
                ),
                {
                    success: "Password changed successfully",
                    error: (err) => {
                        const message = err?.response?.data?.errors?.password
                            ? "" + err.response.data.errors.password.join("\n• ")
                            : err?.response?.data?.error || "General failure.";

                        return `Error: ${message}`;
                    },
                    loading: "Resetting password...",
                }
            ).then(() => {
                navigate("/login"); // Redirect to login after success
            });

        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
                <h2 className="text-2xl font-semibold text-center text-gray-700 mb-6">Reset Password</h2>

                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="mb-6">
                        <label htmlFor="password" className="block text-sm font-medium text-gray-600">
                            New Password
                        </label>
                        <input
                            type="password"
                            id="password"
                            className="w-full mt-2 px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                            placeholder="Enter your new password"
                            {...register("password")}
                        />
                        {errors.password && (
                            <p className="text-red-500 text-sm">{errors.password.message}</p>
                        )}
                    </div>

                    <div className="mb-6">
                        <label htmlFor="confirmPassword" className="block text-sm font-medium text-gray-600">
                            Confirm Password
                        </label>
                        <input
                            type="password"
                            id="confirmPassword"
                            className="w-full mt-2 px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                            placeholder="Confirm your new password"
                            {...register("confirmPassword")}
                        />
                        {errors.confirmPassword && (
                            <p className="text-red-500 text-sm">{errors.confirmPassword.message}</p>
                        )}
                    </div>

                    <button
                        type="submit"
                        className="w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600 transition duration-200"
                    >
                        Reset Password
                    </button>
                </form>

                <div className="mt-4 text-center">
                    <p className="text-sm text-gray-600">
                        Want to go back?{" "}
                        <a href="/login" className="text-blue-500 hover:underline">
                            Go back to login
                        </a>
                    </p>
                </div>
            </div>
        </div>
    );
}
