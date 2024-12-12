import { useState } from "react";
import {http} from "../../../http.ts";
import {UserInfo} from "../../../api.ts";

function UsersOverview() {
    const [players, setPlayers] = useState<UserInfo[] | null>(null);
    const [userList, setUserList] = useState(players);

    const fetchUsers = async () => {
        const res = await http.authUsersList();
        setPlayers(res.data);
    }

    const handleEdit = (userId: string) => {
        console.log(`Edit user with ID: ${userId}`);
        // Add your edit functionality here
    };

    const handleDelete = (userId: string) => {
        console.log(`Delete user with ID: ${userId}`);
        // Add your delete functionality here
    };

    const handleCreatePlayer = () => {
        console.log("Create a new player");
    };


    return (
        <div className="max-w-6xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">

            <div className="flex justify-between items-center mb-4">
                <h1 className="text-2xl font-bold text-gray-800">Game Players Details</h1>
                <button
                    onClick={handleCreatePlayer}
                    className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600 flex gap-2"
                >
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                         stroke="currentColor" className="size-6">
                        <path stroke-linecap="round" stroke-linejoin="round"
                              d="M18 7.5v3m0 0v3m0-3h3m-3 0h-3m-2.25-4.125a3.375 3.375 0 1 1-6.75 0 3.375 3.375 0 0 1 6.75 0ZM3 19.235v-.11a6.375 6.375 0 0 1 12.75 0v.109A12.318 12.318 0 0 1 9.374 21c-2.331 0-4.512-.645-6.374-1.766Z"/>
                    </svg>

                    <p>Create User</p>
                </button>
            </div>
            <table className="min-w-full border border-gray-300 text-gray-700">
                {/* Table Header */}
                <thead className="bg-gray-100">
                <tr>
                    <th className="border border-gray-300 px-4 py-2 text-left">Name</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Username</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Email</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Phone</th>
                    <th className="border border-gray-300 px-4 py-2 text-center">Role</th>
                    <th className="border border-gray-300 px-4 py-2 text-center">Active</th>
                    <th className="border border-gray-300 px-4 py-2 text-center">Autoplay</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Registration Date</th>
                    <th className="border border-gray-300 px-4 py-2 text-center">Actions</th>
                </tr>
                </thead>
                {/* Table Body */}
                <tbody>
                {userList.map ((player) => (
                    <tr key={player.userId} className="hover:bg-gray-50">
                        <td className="border border-gray-300 px-4 py-2">
                            {player.firstName} {player.lastName}
                        </td>
                        <td className="border border-gray-300 px-4 py-2">{player.username}</td>
                        <td className="border border-gray-300 px-4 py-2">{player.email}</td>
                        <td className="border border-gray-300 px-4 py-2">{player.phoneNumber}</td>
                        <td className="border border-gray-300 px-4 py-2 text-center">{player.role}</td>
                        <td className="border border-gray-300 px-4 py-2 text-center">
                            <select
                                value={player.isActive ? "active" : "inactive"}
                                onChange={() => toggleActiveStatus (player.userId, player.isActive)}
                                className="bg-white border border-gray-300 rounded px-2 py-1"
                            >
                                <option value="active">Active</option>
                                <option value="inactive">Inactive</option>
                            </select>
                        </td>
                        <td className="border border-gray-300 px-4 py-2 text-center">
                            {player.isAutoplay ? "✅" : "❌"}
                        </td>
                        <td className="border border-gray-300 px-4 py-2">
                            {new Date (player.registrationDate).toLocaleDateString ()}
                        </td>
                        <td className="border border-gray-300 px-4 py-2 flex text-center align-middle">
                            <button
                                onClick={() => handleEdit (player.userId)}
                                className="bg-blue-500 text-white px-3 py-1 rounded hover:bg-blue-600 mr-2"
                            >
                                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"
                                     stroke-width="1.5" stroke="currentColor" className="size-6">
                                    <path stroke-linecap="round" stroke-linejoin="round"
                                          d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10"/>
                                </svg>

                            </button>
                            <button
                                onClick={() => handleDelete (player.userId)}
                                className="bg-red-500 text-white px-3 py-1 rounded hover:bg-red-600"
                            >
                                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"
                                     stroke-width="1.5" stroke="currentColor" className="size-6">
                                    <path stroke-linecap="round" stroke-linejoin="round"
                                          d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0"/>
                                </svg>

                            </button>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}

export default UsersOverview;
