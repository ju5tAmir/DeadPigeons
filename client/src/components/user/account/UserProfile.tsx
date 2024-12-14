import { useAtom } from "jotai";
import { userInfoAtom } from "../../../atoms/auth.ts";
import { toDanishTimeFormat } from "../../../utils/TimeUtils.ts";

const UserProfile = () => {
    const [user] = useAtom(userInfoAtom);

    if (!user) {
        return (
            <div className="flex h-screen items-center justify-center">
                <h1 className="text-4xl font-bold text-red-600">403 - Access Denied</h1>
            </div>
        );
    }

    return (
        <div className="max-w-2xl mx-auto mt-10 p-8 bg-white dark:bg-gray-900 shadow-lg rounded-xl">
            <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-200 mb-6 text-center">
                User Profile
            </h1>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <ProfileField label="First Name" value={user.firstName} />
                <ProfileField label="Last Name" value={user.lastName} />
                <ProfileField label="Username" value={user.username} />
                <ProfileField label="Email" value={user.email} />
                <ProfileField label="Phone Number" value={user.phoneNumber} />
                <ProfileField label="Balance" value={`$${user.balance.toFixed(2)}`} />
                <ProfileField label="Role" value={user.role} />
                <ProfileField label="Active" value={user.isActive ? "Yes" : "No"} />
                <ProfileField label="Autoplay" value={user.isAutoplay ? "Yes" : "No"} />
                <ProfileField
                    label="Registration Date"
                    value={toDanishTimeFormat(user.registerationDate)}
                />
            </div>
        </div>
    );
};

interface ProfileFieldProps {
    label: string;
    value: string | number;
}

const ProfileField: React.FC<ProfileFieldProps> = ({ label, value }) => (
    <div className="flex flex-col border-b border-gray-300 pb-2">
        <span className="text-sm text-gray-500 dark:text-gray-400 font-medium">
            {label}
        </span>
        <span className="text-lg text-gray-800 dark:text-gray-200 font-semibold truncate">
            {value}
        </span>
    </div>
);

export default UserProfile;
