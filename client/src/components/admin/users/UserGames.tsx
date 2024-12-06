import React from "react";

export interface UserGamesProp {
    weekNumber: number;
    boards: number;
    isWinner: boolean;
}

function UserGames({ data }: { data: UserGamesProp[] }) {
    return (
        <div className="max-w-2xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h2 className="text-xl font-bold text-gray-800 mb-4">User Games</h2>
            <table className="w-full border-collapse border border-gray-300">
                <thead>
                <tr className="bg-gray-100">
                    <th className="border border-gray-300 px-4 py-2 text-left">Week Number</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Boards</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Winner</th>
                </tr>
                </thead>
                <tbody>
                {data.map((game, index) => (
                    <tr key={index} className="hover:bg-gray-50">
                        <td className="border border-gray-300 px-4 py-2">{game.weekNumber}</td>
                        <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                            <span className="flex-1 text-center">{game.boards}</span>
                            {/* Position the Icon to the Right */}
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                strokeWidth={1.5}
                                stroke="currentColor"
                                className="w-4 h-4 text-blue-500 cursor-pointer hover:text-blue-700"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    d="M15.75 9V5.25A2.25 2.25 0 0013.5 3h-3a2.25 2.25 0 00-2.25 2.25V9m-1.5 0h9m-10.5 0A2.25 2.25 0 003 11.25v6A2.25 2.25 0 005.25 19.5h13.5A2.25 2.25 0 0021 17.25v-6A2.25 2.25 0 0018.75 9m-10.5 0V5.25m0 13.5h6"
                                />
                            </svg>
                        </td>
                        <td className="border border-gray-300 px-4 py-2">
                            {game.isWinner ? "Yes" : "No"}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}

export default UserGames;
