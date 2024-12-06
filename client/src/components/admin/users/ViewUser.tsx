import React from "react";
import { UserInfo } from "./UsersOverview.tsx";

interface ViewUserProps {
    user: UserInfo;
}

function ViewUser({ user }: ViewUserProps) {
    return (
        <div className="max-w-lg mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h2 className="text-xl font-bold text-gray-800 mb-6">View User</h2>
            <div className="space-y-4">
                <div>
                    <label className="block text-gray-600 font-medium mb-1">First Name</label>
                    <div className="w-full border border-gray-300 px-3 py-2 rounded-md bg-gray-100">
                        {user.firstName}
                    </div>
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Last Name</label>
                    <div className="w-full border border-gray-300 px-3 py-2 rounded-md bg-gray-100">
                        {user.lastName}
                    </div>
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Username</label>
                    <div className="w-full border border-gray-300 px-3 py-2 rounded-md bg-gray-100">
                        {user.username}
                    </div>
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Email</label>
                    <div className="w-full border border-gray-300 px-3 py-2 rounded-md bg-gray-100">
                        {user.email}
                    </div>
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Phone Number</label>
                    <div className="w-full border border-gray-300 px-3 py-2 rounded-md bg-gray-100">
                        {user.phoneNumber}
                    </div>
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Role</label>
                    <div className="w-full border border-gray-300 px-3 py-2 rounded-md bg-gray-100">
                        {user.role}
                    </div>
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Active</label>
                    <div className="w-full border border-gray-300 px-3 py-2 rounded-md bg-gray-100">
                        {user.isActive ? "Active" : "Inactive"}
                    </div>
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Autoplay</label>
                    <div className="w-full border border-gray-300 px-3 py-2 rounded-md bg-gray-100">
                        {user.isAutoplay ? "Enabled" : "Disabled"}
                    </div>
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Registration Date</label>
                    <div className="w-full border border-gray-300 px-3 py-2 rounded-md bg-gray-100">
                        {new Date(user.registrationDate).toLocaleDateString()}
                    </div>
                </div>
                <div className="flex justify-end mt-6">
                    <button
                        type="button"
                        className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400"
                    >
                        Close
                    </button>
                </div>
            </div>
        </div>
    );
}

export default ViewUser;
