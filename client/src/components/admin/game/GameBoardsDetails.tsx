import React, { useState } from "react";

export interface PackageResponse {
    numberOfFields: number;
    price: number;
}

export interface Player {
    fullName: string;
    email: string;
}

export interface Boards {
    player: Player;
    package: PackageResponse;
    playSequence: Set<number>;
    playTime: Date;
}

function GameBoardsDetails({ boards }: { boards: Boards[] }) {
    const [searchTerm, setSearchTerm] = useState("");

    // Filter boards based on the search term
    const filteredBoards = boards.filter((board) => {
        const search = searchTerm.toLowerCase();
        return (
            board.player.fullName.toLowerCase().includes(search) ||
            board.player.email.toLowerCase().includes(search) ||
            board.package.numberOfFields.toString().includes(search) ||
            board.package.price.toString().includes(search) ||
            Array.from(board.playSequence).join(", ").includes(search) ||
            new Date(board.playTime).toLocaleString().toLowerCase().includes(search)
        );
    });

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
                    <th className="border border-gray-300 px-4 py-2 text-left">Email</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Package Fields</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Package Price</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Play Sequence</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Play Time</th>
                </tr>
                </thead>
                {/* Table Body */}
                <tbody>
                {filteredBoards.length > 0 ? (
                    filteredBoards.map((board, index) => (
                        <tr key={index} className="hover:bg-gray-50">
                            <td className="border border-gray-300 px-4 py-2">{board.player.fullName}</td>
                            <td className="border border-gray-300 px-4 py-2">{board.player.email}</td>
                            <td className="border border-gray-300 px-4 py-2">
                                {board.package.numberOfFields} Fields
                            </td>
                            <td className="border border-gray-300 px-4 py-2">
                                ${board.package.price.toFixed(2)}
                            </td>
                            <td className="border border-gray-300 px-4 py-2">
                                {Array.from(board.playSequence).join(", ")}
                            </td>
                            <td className="border border-gray-300 px-4 py-2">
                                {new Date(board.playTime).toLocaleString()}
                            </td>
                        </tr>
                    ))
                ) : (
                    <tr>
                        <td colSpan={6} className="border border-gray-300 px-4 py-2 text-center">
                            No matching records found.
                        </td>
                    </tr>
                )}
                </tbody>
            </table>
        </div>
    );
}

export default GameBoardsDetails;
