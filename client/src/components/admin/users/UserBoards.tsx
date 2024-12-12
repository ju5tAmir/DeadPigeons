import {BoardResponse} from "../../../api.ts";

function UserBoards({ boards }: { boards: BoardResponse[] }) {
    return (
        <div className="max-w-4xl mx-auto mt-8 p-6 bg-white shadow-lg rounded-lg">
            <h2 className="text-2xl font-semibold text-gray-800 mb-6">User Boards</h2>
            <table className="min-w-full border border-gray-300 text-gray-700">
                {/* Table Header */}
                <thead className="bg-gray-100">
                <tr>
                    <th className="border px-4 py-2 text-left">Game Year</th>
                    <th className="border px-4 py-2 text-left">Week Number</th>
                    <th className="border px-4 py-2 text-left">Package Price</th>
                    <th className="border px-4 py-2 text-left">Play Sequence</th>
                    <th className="border px-4 py-2 text-left">Play Date</th>
                </tr>
                </thead>
                {/* Table Body */}
                <tbody>
                {boards.map((board) => (
                    <tr key={board.boardId} className="hover:bg-gray-50">
                        <td className="border px-4 py-2">{board.game.year}</td>
                        <td className="border px-4 py-2">{board.game.weekNumber}</td>
                        <td className="border px-4 py-2">{board.package.price}</td>
                        <td className="border px-4 py-2">{board.playSequence.join(", ")}</td>
                        <td className="border px-4 py-2">
                            {new Date(board.playTime).toLocaleDateString()}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}

export default UserBoards;
