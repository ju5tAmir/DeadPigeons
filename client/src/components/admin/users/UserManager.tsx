import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { BoardResponse, UserInfo } from "../../../api.ts";
import { http } from "../../../http.ts";
import ViewUser from "./ViewUser.tsx";
import UserBoards from "./UserBoards.tsx";

function UserManager() {
    const { id } = useParams<{ id: string }>();
    const [user, setUser] = useState<UserInfo | null>(null);
    const [userBoards, setUserBoards] = useState<BoardResponse[] | null>(null);

    // Fetch user details
    const fetchUser = async (userId: string) => {
        try {
            const res = await http.userDetail(userId);
            setUser(res.data);
        } catch (error) {
            console.error("Failed to fetch user details:", error);
        }
    };

    // Fetch user boards
    const fetchUserBoards = async (userId: string) => {
        try {
            const res = await http.boardUserDetail(userId);
            setUserBoards(res.data);
        } catch (error) {
            console.error("Failed to fetch user boards:", error);
        }
    };

    useEffect(() => {
        if (id) {
            fetchUser(id);
        }
    }, [id]);

    return (
        <div className="max-w-7xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            {/* User Details */}
            <details
                key={user?.userId}
                className="border border-gray-300 rounded-lg"
                open={true}
            >
                <summary
                    className="cursor-pointer px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200"
                >
                    User Details
                </summary>
                <div className="px-6 py-4 bg-white border-t border-gray-300">
                    {user ? <ViewUser user={user} /> : <p>Loading User Details...</p>}
                </div>
            </details>

            {/* Boards Details */}
            <details
                className="border border-gray-300 rounded-lg mt-4"
                onToggle={(e) => {
                    if ((e.target as HTMLDetailsElement).open && id) {
                        fetchUserBoards(id);
                    }
                }}
            >
                <summary
                    className="cursor-pointer px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200"
                >
                    Boards Details
                </summary>
                <div className="px-6 py-4 bg-white border-t border-gray-300">
                    {userBoards ? (
                        <UserBoards boards={userBoards} />
                    ) : (
                        <p>Loading Boards...</p>
                    )}
                </div>
            </details>

            {/* Back Button */}
            <div className="mt-6">
                <button
                    className="px-4 py-2 bg-blue-500 text-white rounded shadow hover:bg-blue-600"
                    onClick={() => window.history.back()}
                >
                    Back
                </button>
            </div>
        </div>
    );
}

export default UserManager;
