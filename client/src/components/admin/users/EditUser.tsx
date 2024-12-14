import React, { useEffect, useState } from "react";
import { UserInfo } from "../../../api.ts";
import { useParams, useNavigate } from "react-router-dom";
import { http } from "../../../http.ts";
import toast from "react-hot-toast";

function EditUser() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [formData, setFormData] = useState<UserInfo>({
        firstName: "",
        lastName: "",
        username: "",
        email: "",
        phoneNumber: "",
        role: "",
        isActive: false,
        isAutoplay: false,
    });

    const [loading, setLoading] = useState(true);

    // Fetch user data
    const fetchUser = async (userId: string) => {
        try {
            const res = await http.userDetail(userId);
            setFormData(res.data);
        } catch (error) {
            toast.error("Failed to load user details.");
        } finally {
            setLoading(false);
        }
    };


    const handleUpdate = async () => {
        toast.promise(
            http.userUpdate(id!, formData),
            {
                loading: "Updating the user...",
                success: () => {
                    navigate(-1); // Go back to the previous page
                    return "User updated successfully!";
                },
                error: (err) => {
                    const message = err?.response?.data?.error || "Failed to update user.";
                    return `Error: ${message}`;
                },
            }
        );
    };

    // Handle form changes
    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
    ) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: name === "isActive" || name === "isAutoplay" ? value === "true" : value,
        }));
    };

    // Handle form submission
    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        handleUpdate();
    };

    useEffect(() => {
        if (id) fetchUser(id);
    }, [id]);

    if (loading) {
        return <p>Loading user details...</p>;
    }

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
                <div className="flex justify-end space-x-4 mt-6">
                    <button
                        type="button"
                        onClick={() => navigate(-1)}
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
