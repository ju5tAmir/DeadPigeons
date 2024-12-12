import { toDanishTimeFormat } from "../../../utils/TimeUtils.ts";
import { WinnersResponse } from "../../../api.ts";

function GameWinners({ result }: { result: WinnersResponse }) {
    return (
        <div className="max-w-6xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
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
                {result.winners?.map((winner, index) => (
                    winner.boardDetails?.map((board, boardIndex) => (
                        <tr key={`${index}-${boardIndex}`} className="hover:bg-gray-50">
                            <td className="border border-gray-300 px-4 py-2">
                                {winner.playerDetails?.firstName + " " + winner.playerDetails?.lastName}
                            </td>
                            <td className="border border-gray-300 px-4 py-2">{winner.playerDetails?.username}</td>
                            <td className="border border-gray-300 px-4 py-2">
                                {board.packageDetails?.numberOfFields}
                            </td>
                            <td className="border border-gray-300 px-4 py-2">{board.packageDetails?.price}</td>
                            <td className="border border-gray-300 px-4 py-2">
                                {board.playSequence?.join(", ")}
                            </td>
                            <td className="border border-gray-300 px-4 py-2">
                                {toDanishTimeFormat(board.)}
                            </td>
                        </tr>
                    ))
                ))}
                </tbody>
            </table>
        </div>
    );
}

export default GameWinners;
