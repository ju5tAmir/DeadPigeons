import React from "react";
import { UserInfo } from "../../../api.ts";

function ViewUser({ user }: { user: UserInfo }) {
    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-50">
            <div className="max-w-2xl p-8 bg-white shadow-lg rounded-xl">
                <h2 className="text-2xl font-semibold text-gray-800 mb-6">User Details</h2>
                <div className="space-y-6">
                    <div className="flex items-center space-x-4">
                        <label className="w-1/3 text-gray-600 font-medium">First Name:</label>
                        <div className="flex-1 px-4 py-2 bg-gray-100 rounded-md text-gray-700">
                            {user?.firstName || "N/A"}
                        </div>
                    </div>
                    <div className="flex items-center space-x-4">
                        <label className="w-1/3 text-gray-600 font-medium">Last Name:</label>
                        <div className="flex-1 px-4 py-2 bg-gray-100 rounded-md text-gray-700">
                            {user?.lastName || "N/A"}
                        </div>
                    </div>
                    <div className="flex items-center space-x-4">
                        <label className="w-1/3 text-gray-600 font-medium">Username:</label>
                        <div className="flex-1 px-4 py-2 bg-gray-100 rounded-md text-gray-700">
                            {user?.username || "N/A"}
                        </div>
                    </div>
                    <div className="flex items-center space-x-4">
                        <label className="w-1/3 text-gray-600 font-medium">Email:</label>
                        <div className="flex-1 px-4 py-2 bg-gray-100 rounded-md text-gray-700">
                            {user?.email || "N/A"}
                        </div>
                    </div>
                    <div className="flex items-center space-x-4">
                        <label className="w-1/3 text-gray-600 font-medium">Phone Number:</label>
                        <div className="flex-1 px-4 py-2 bg-gray-100 rounded-md text-gray-700">
                            {user?.phoneNumber || "N/A"}
                        </div>
                    </div>
                    <div className="flex items-center space-x-4">
                        <label className="w-1/3 text-gray-600 font-medium">Role:</label>
                        <div className="flex-1 px-4 py-2 bg-gray-100 rounded-md text-gray-700">
                            {user?.role || "N/A"}
                        </div>
                    </div>
                    <div className="flex items-center space-x-4">
                        <label className="w-1/3 text-gray-600 font-medium">Status:</label>
                        <div
                            className={`flex-1 px-4 py-2 bg-gray-100 rounded-md ${
                                user?.isActive ? "text-green-600" : "text-red-600"
                            }`}
                        >
                            {user?.isActive ? "Active" : "Inactive"}
                        </div>
                    </div>
                    <div className="flex items-center space-x-4">
                        <label className="w-1/3 text-gray-600 font-medium">Autoplay:</label>
                        <div className="flex-1 px-4 py-2 bg-gray-100 rounded-md text-gray-700">
                            {user?.isAutoplay ? "Enabled" : "Disabled"}
                        </div>
                    </div>
                    <div className="flex items-center space-x-4">
                        <label className="w-1/3 text-gray-600 font-medium">Registration Date:</label>
                        <div className="flex-1 px-4 py-2 bg-gray-100 rounded-md text-gray-700">
                            {user?.registerationDate || "N/A"}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ViewUser;
