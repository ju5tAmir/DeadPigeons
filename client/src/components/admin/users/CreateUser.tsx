import React, { useState } from 'react';
import toast from "react-hot-toast";
import { http } from "../../../http.ts";
import {useNavigate} from "react-router-dom";

const CreateUser = () => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [email, setEmail] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const navigate = useNavigate();

    const [errorMessages, setErrorMessages] = useState<string[]>([]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        handleCreate({ firstName, lastName, email, phoneNumber });
    };

    const handleCreate = async (userData: { firstName: string; lastName: string; email: string; phoneNumber: string }) => {
        toast.promise(
            http.authRegisterCreate(userData),
            {
                loading: 'Creating user...',
                success: () => {
                    setTimeout(() => {
                        navigate(-1);
                    }, 1000);

                    return 'User created successfully and the activation link has been sent to the user email.';
                },
                error: (err) => {
                    const response = err?.response?.data;
                    if (response?.errors) {
                        // If there are validation errors, extract them and set them in the state
                        const validationErrors = Object.values(response.errors)
                            .flat() // Flatten the array of error messages
                            .map((error) => `${error}`); // Map each error message with a prefix

                        setErrorMessages(validationErrors);
                        return validationErrors.join("\n"); // Join error messages for the toast
                    }

                    // If no specific validation errors, show a general error message
                    const message = response?.message || "Failed to create the user.";
                    setErrorMessages([message]);
                    return message;
                }
            }
        );
    };

    function handleCancel() {
        navigate(-1);
    }

    return (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
            <div className="bg-white p-6 rounded-lg shadow-lg w-96">
                <h3 className="text-lg font-medium text-gray-700 mb-4">Create New User</h3>
                <form onSubmit={handleSubmit}>
                    <div className="mb-4">
                        <label htmlFor="firstName" className="block text-sm font-medium text-gray-700">First Name</label>
                        <input
                            type="text"
                            id="firstName"
                            value={firstName}
                            onChange={(e) => setFirstName(e.target.value)}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                            required
                        />
                    </div>
                    <div className="mb-4">
                        <label htmlFor="lastName" className="block text-sm font-medium text-gray-700">Last Name</label>
                        <input
                            type="text"
                            id="lastName"
                            value={lastName}
                            onChange={(e) => setLastName(e.target.value)}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                            required
                        />
                    </div>
                    <div className="mb-4">
                        <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
                        <input
                            type="email"
                            id="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                            required
                        />
                    </div>
                    <div className="mb-4">
                        <label htmlFor="phoneNumber" className="block text-sm font-medium text-gray-700">Phone Number</label>
                        <input
                            type="text"
                            id="phoneNumber"
                            value={phoneNumber}
                            onChange={(e) => setPhoneNumber(e.target.value)}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                            required
                        />
                    </div>
                    {errorMessages.length > 0 && (
                        <div className="mb-4">
                            <ul className="text-sm text-red-600">
                                {errorMessages.map((error, index) => (
                                    <li key={index} className="text-red-500">• {error}</li>
                                ))}
                            </ul>
                        </div>
                    )}
                    <div className="flex justify-end gap-4">
                        <button
                            type="button"
                            className="bg-gray-300 px-4 py-2 rounded-md hover:bg-gray-400"
                            onClick={handleCancel}
                        >
                            Cancel
                        </button>
                        <button
                            type="submit"
                            className="bg-green-500 text-white px-4 py-2 rounded-md hover:bg-green-600"
                        >
                            Create User
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default CreateUser;
