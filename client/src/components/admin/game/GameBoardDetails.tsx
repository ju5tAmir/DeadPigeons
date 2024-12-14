import {BoardResponse, GameBoardsDetails} from "../../../api.ts";
import {useState} from "react";
import {toDanishTimeFormat} from "../../../utils/TimeUtils.ts";

function GameBoardDetails({ boards }: { boards: GameBoardsDetails[] }) {
    const [searchTerm, setSearchTerm] = useState("");

    return (
        <div className="max-w-6xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            {/* Header with Search Bar */}
            <div className="flex items-center justify-between mb-4">
                <h1 className="text-2xl font-bold text-gray-800">Game Boards Details</h1>
                <input
                    type="text"
                    placeholder="Search..."
                    className="border border-gray-300 rounded-lg px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                />
            </div>

            {/* Table */}
            <table className="min-w-full border border-gray-300 text-gray-700">
                {/* Table Header */}
                <thead className="bg-gray-100">
                <tr>
                    <th className="border border-gray-300 px-4 py-2 text-left">Full Name</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Username</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Package Fields</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Package Price</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Play Sequence</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Play Date</th>
                </tr>
                </thead>
                {/* Table Body */}
                <tbody>
                {boards.map((board, index) => (
                    <tr key={index} className="hover:bg-gray-50">
                        <td className="border border-gray-300 px-4 py-2">{board.player?.firstName + " " + board.player?.lastName}</td>
                        <td className="border border-gray-300 px-4 py-2">{board.player?.username}</td>
                        <td className="border border-gray-300 px-4 py-2">{board.packageDetails?.numberOfFields}</td>
                        <td className="border border-gray-300 px-4 py-2">{board.packageDetails?.price}</td>
                        <td className="border border-gray-300 px-4 py-2">
                            {board.playSequence?.join (", ")}
                        </td>
                        <td className="border border-gray-300 px-4 py-2">
                            {toDanishTimeFormat(board.playDate)}
                        </td>
                    </tr>
                ))
                }
                </tbody>
            </table>
        </div>
    );
}

export default GameBoardDetails;
