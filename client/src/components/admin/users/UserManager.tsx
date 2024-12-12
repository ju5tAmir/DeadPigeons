import GameDetails from "../game/GameDetails.tsx";
import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {UserInfo} from "../../../api.ts";
import {http} from "../../../http.ts";
import ViewUser from "./ViewUser.tsx";
import GameBoardDetails from "../game/GameBoardDetails.tsx";


function UserManager() {
    const { id } = useParams();
    const [user, setUser] = useState<UserInfo>();


    const fetchUser = async (id: string) => {
        const res = await http.userDetail(id);
        setUser(res.data);
    }

    useEffect (() => {
        if (id) fetchUser(id);
    }, []);

    async function fetchGameBoards() {
        const res = await http.
    }

    return (
        <>
            <ViewUser user={user} />
            <div className="max-w-7xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
                {/* Boards Details */}
                <details
                    className="border border-gray-300 rounded-lg"
                    open={false}
                    onToggle={(e) => {
                        if ((e.target as HTMLDetailsElement).open) {
                            fetchGameBoards();
                        }
                    }}
                >
                    <summary
                        className="cursor-pointer px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200">
                        Boards Details
                    </summary>
                    <div className="px-6 py-4 bg-white border-t border-gray-300">
                        {boards ? (
                            <GameBoardDetails boards={boards} />
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
        </>
    );
}

export default UserManager;