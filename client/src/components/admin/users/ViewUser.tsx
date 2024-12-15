import React from "react";
import { UserInfo } from "../../../api.ts";
import {toDanishTimeFormat} from "../../../utils/TimeUtils.ts";

const ViewUser = ({ user }: { user: UserInfo }) => {
    return (
        <div className="max-w-2xl mx-auto mt-10 p-8 bg-white dark:bg-gray-900 shadow-lg rounded-xl">
            <h2 className="text-3xl font-semibold text-gray-800 dark:text-gray-200 mb-6 text-center">User Details</h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <ProfileField label="First Name" value={user?.firstName || "N/A"} />
                <ProfileField label="Last Name" value={user?.lastName || "N/A"} />
                <ProfileField label="Balance" value={`${user?.balance} kr` || "N/A"} />
                <ProfileField label="Username" value={user?.username || "N/A"} />
                <ProfileField label="Email" value={user?.email || "N/A"} />
                <ProfileField label="Phone Number" value={user?.phoneNumber || "N/A"} />
                <ProfileField label="Role" value={user?.role || "N/A"} />
                <ProfileField label="Status" value={user?.isActive ? "Active" : "Inactive"} status={user?.isActive} />
                <ProfileField label="Autoplay" value={user?.isAutoplay ? "Enabled" : "Disabled"} />
                <ProfileField label="Registration Date" value={toDanishTimeFormat(user?.registerationDate) || "N/A"} />
            </div>
        </div>
    );
};

interface ProfileFieldProps {
    label: string;
    value: string | number;
    status?: boolean;
}

const ProfileField: React.FC<ProfileFieldProps> = ({ label, value, status }) => (
    <div className="flex flex-col border-b border-gray-300 pb-2">
        <span className="text-sm text-gray-500 dark:text-gray-400 font-medium">{label}</span>
        <span
            className={`text-lg font-semibold ${
                status === undefined
                    ? "text-gray-800 dark:text-gray-200"
                    : status
                        ? "text-green-600"
                        : "text-red-600"
            }`}
        >
            {value}
        </span>
    </div>
);

export default ViewUser;
