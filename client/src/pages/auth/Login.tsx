import {checkAuth, Credentials, useAuth} from "../../atoms/auth.ts";
import { useForm, SubmitHandler } from 'react-hook-form';
import toast from "react-hot-toast";
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import {useAtom} from "jotai";
import {useNavigate, useNavigation} from "react-router-dom";

// Validation schema using Yup
const schema: yup.ObjectSchema<Credentials> = yup
    .object({
        email: yup.string().email("Invalid email format").required("Email is required"),
        password: yup.string().min(6, "Password must be at least 6 characters").required("Password is required"),
    })
    .required();

export default function Login() {
    const { login } = useAuth();
    const [ isAuth ] = useAtom(checkAuth)
    const navigate = useNavigate();

    if (isAuth != null) {
        navigate("/");
    }

    // Use react-hook-form for form handling
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<Credentials>({
        resolver: yupResolver(schema),
    });

    const onSubmit: SubmitHandler<Credentials> = (data) => {
        toast.promise(login(data), {
            success: "Logged in successfully",
            error: "Invalid credentials",
            loading: "Logging in...",
        });
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
                <h2 className="text-2xl font-semibold text-center text-gray-700 mb-6">Login</h2>

                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="mb-4">
                        <label htmlFor="email" className="block text-sm font-medium text-gray-600">Email</label>
                        <input
                            type="email"
                            id="email"
                            className="w-full mt-2 px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                            placeholder="Enter your email"
                            {...register('email')}
                        />
                        {errors.email && (
                            <p className="text-red-500 text-sm">{errors.email.message}</p>
                        )}
                    </div>

                    <div className="mb-6">
                        <label htmlFor="password" className="block text-sm font-medium text-gray-600">Password</label>
                        <input
                            type="password"
                            id="password"
                            className="w-full mt-2 px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                            placeholder="Enter your password"
                            {...register('password')}
                        />
                        {errors.password && (
                            <p className="text-red-500 text-sm">{errors.password.message}</p>
                        )}
                    </div>

                    <button
                        type="submit"
                        className="w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600 transition duration-200"
                    >
                        Login
                    </button>
                </form>

                <div className="mt-4 text-center">
                    <p className="text-sm text-gray-600">
                        Forgot your password? <a href="/reset-password" className="text-blue-500 hover:underline">Click here</a>
                    </p>
                </div>
            </div>
        </div>
    );
}
