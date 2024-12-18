import { useForm, SubmitHandler } from "react-hook-form";
import toast from "react-hot-toast";
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useNavigate } from "react-router-dom";
import {http} from "../../http.ts";

// Validation schema for password reset
const schema = yup
    .object({
        email: yup
            .string()
            .email("Invalid email format")
            .required("Email is required"),
    })
    .required();

interface ResetPasswordForm {
    email: string;
}

export default function ResetPassword() {
    const navigate = useNavigate();

    // Use react-hook-form for form handling
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<ResetPasswordForm>({
        resolver: yupResolver(schema),
    });

    const onSubmit: SubmitHandler<ResetPasswordForm> = (data) => {
        toast.promise(http.authResetPasswordCreate({
            email: data.email
        }),
            {
                success: "Password reset email sent successfully",
                error: "Unable to send reset email",
                loading: "Sending password reset email...",
            }
        ).then(() => {
            navigate("/login");
        });
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
                <h2 className="text-2xl font-semibold text-center text-gray-700 mb-6">Reset Password</h2>

                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="mb-6">
                        <label htmlFor="email" className="block text-sm font-medium text-gray-600">
                            Email
                        </label>
                        <input
                            type="email"
                            id="email"
                            className="w-full mt-2 px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                            placeholder="Enter your email"
                            {...register("email")}
                        />
                        {errors.email && (
                            <p className="text-red-500 text-sm">{errors.email.message}</p>
                        )}
                    </div>

                    <button
                        type="submit"
                        className="w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600 transition duration-200"
                    >
                        Send Reset Email
                    </button>
                </form>

                <div className="mt-4 text-center">
                    <p className="text-sm text-gray-600">
                        Remember your password?{" "}
                        <a href="/login" className="text-blue-500 hover:underline">
                            Go back to login
                        </a>
                    </p>
                </div>
            </div>
        </div>
    );
}
