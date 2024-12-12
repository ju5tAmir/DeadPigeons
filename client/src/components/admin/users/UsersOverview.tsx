import { useEffect, useState } from "react";
import { http } from "../../../http.ts";
import { UserInfo } from "../../../api.ts";
import { toDanishTimeFormat } from "../../../utils/TimeUtils.ts";
import { useNavigate } from "react-router-dom";
import { RoutePath } from "../../../utils/admin/RoutePath.ts";
import EditUser from "./EditUser.tsx";
import toast from "react-hot-toast";
import DeleteUserModal from "./DeleteUserModal.tsx";
import UsersTable from "./UsersTable.tsx";
import UsersOverviewHeader from "./UsersOverviewHeader.tsx";

function UsersOverview() {
    const [userList, setUserList] = useState<UserInfo[] | null>(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [userIdToDelete, setUserIdToDelete] = useState<string | null>(null);
    const navigate = useNavigate();

    const fetchUsers = async () => {
        const res = await http.userAllList();
        setUserList(res.data);
    };

    const handleEdit = (userId: string) => {
        navigate(`${userId}/update`);
    };

    // Trigger the modal and set the user ID for deletion
    const handleDelete = (userId: string) => {
        setUserIdToDelete(userId);
        setIsModalOpen(true);
    };

    // Confirm the deletion
    const confirmDelete = async () => {
        if (userIdToDelete) {
            toast.promise (http.userDelete(userIdToDelete),
                {
                    loading: "Deleting the user...",
                    success: () => {
                        return "User deleted successfully!";
                    },
                    error: (err) => {
                        const message = err?.response?.data?.error || "Failed to delete user.";
                        return `Error: ${message}`;
                    },
                })
            setIsModalOpen(false);
        }
        window.location.reload();
    };

    // Cancel the delete operation
    const cancelDelete = () => {
        setIsModalOpen(false);
        setUserIdToDelete(null); // Reset the user ID
    };

    const handleCreatePlayer = () => {
        navigate("create");
    };

    useEffect(() => {
        fetchUsers();
    }, []);

    function handleView(userId: string) {
        navigate(`${RoutePath.users}/${userId}`);
    }

    return (
        <div className="max-w-6xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <UsersOverviewHeader
                handleCreatePlayer={handleCreatePlayer}
            />

            <UsersTable
                userList={userList}
                handleView={handleView}
                handleEdit={handleEdit}
                handleDelete={handleDelete}
            />

            <DeleteUserModal
                isModalOpen={isModalOpen}
                cancelDelete={cancelDelete}
                confirmDelete={confirmDelete}
            />
        </div>
    );
}

export default UsersOverview;
