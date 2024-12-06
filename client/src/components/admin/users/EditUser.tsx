import React, { useState } from "react";
import {UserInfo} from "./UsersOverview.tsx";

interface EditUserProps {
    user: UserInfo;
}

function EditUser({ user}: EditUserProps) {
    const [formData, setFormData] = useState(user);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: name === "isActive" || name === "isAutoplay" ? value === "true" : value,
        }));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
    };

    return (
        <div className="max-w-lg mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h2 className="text-xl font-bold text-gray-800 mb-6">Edit User</h2>
            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label className="block text-gray-600 font-medium mb-1">First Name</label>
                    <input
                        type="text"
                        name="firstName"
                        value={formData.firstName}
                        onChange={handleChange}
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                    />
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Last Name</label>
                    <input
                        type="text"
                        name="lastName"
                        value={formData.lastName}
                        onChange={handleChange}
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                    />
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Username</label>
                    <input
                        type="text"
                        name="username"
                        value={formData.username}
                        onChange={handleChange}
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                    />
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Email</label>
                    <input
                        type="email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                    />
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Phone Number</label>
                    <input
                        type="text"
                        name="phoneNumber"
                        value={formData.phoneNumber}
                        onChange={handleChange}
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                    />
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Role</label>
                    <input
                        type="text"
                        name="role"
                        value={formData.role}
                        onChange={handleChange}
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                    />
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Active</label>
                    <select
                        name="isActive"
                        value={String(formData.isActive)}
                        onChange={handleChange}
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                    >
                        <option value="true">Active</option>
                        <option value="false">Inactive</option>
                    </select>
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Autoplay</label>
                    <select
                        name="isAutoplay"
                        value={String(formData.isAutoplay)}
                        onChange={handleChange}
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                    >
                        <option value="true">Enabled</option>
                        <option value="false">Disabled</option>
                    </select>
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Registration Date</label>
                    <input
                        type="date"
                        name="registrationDate"
                        value={new Date(formData.registrationDate).toISOString().split("T")[0]}
                        onChange={(e) =>
                            setFormData((prev) => ({
                                ...prev,
                                registrationDate: new Date(e.target.value),
                            }))
                        }
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                        disabled
                    />
                </div>
                <div className="flex justify-end space-x-4 mt-6">
                    <button
                        type="button"
                        // onClick={onCancel}
                        className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400"
                    >
                        Cancel
                    </button>
                    <button
                        type="submit"
                        className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                    >
                        Update
                    </button>
                </div>
            </form>
        </div>
    );
}

export default EditUser;
