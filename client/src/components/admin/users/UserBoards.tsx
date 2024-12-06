import React from "react";

export interface UserBoardsProps {
    weekNumber: number;
    packagePrice: number;
    playSequence: number[];
    playDate: Date;
    isWinner: boolean
}

function UserBoards({ data }: { data: UserBoardsProps[] }) {
    return (
        <div className="max-w-3xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h2 className="text-xl font-bold text-gray-800 mb-4">User Boards</h2>
            <table className="w-full border-collapse border border-gray-300">
                <thead>
                <tr className="bg-gray-100">
                    <th className="border border-gray-300 px-4 py-2 text-left">Week Number</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Package Price</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Play Sequence</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Play Date</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Is Winner?</th>
                </tr>
                </thead>
                <tbody>
                {data.map((board, index) => (
                    <tr key={index} className="hover:bg-gray-50">
                        <td className="border border-gray-300 px-4 py-2">{board.weekNumber}</td>
                        <td className="border border-gray-300 px-4 py-2">${board.packagePrice.toFixed (2)}</td>
                        <td className="border border-gray-300 px-4 py-2">
                            {board.playSequence.join (", ")}
                        </td>
                        <td className="border border-gray-300 px-4 py-2">
                            {new Date (board.playDate).toLocaleDateString ()}
                        </td>
                        <td className="border border-gray-300 px-4 py-2">
                            {board.isWinner ? "Yes" : "No"}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}

export default UserBoards;
