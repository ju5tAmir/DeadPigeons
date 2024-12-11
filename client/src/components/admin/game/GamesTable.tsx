import {GameLwResponse} from "../../../api.ts";
import {toDanishTimeFormat} from "../../../utils/TimeUtils.ts";
import {useNavigate} from "react-router-dom";

function GamesTable({ data }: { data: GameLwResponse[] }) {

    const navigate = useNavigate();

    const handleClickOnGame = (gameId: string) => {
        navigate(`${gameId}`)
    }

    return (
        <table className="min-w-full border-collapse border border-gray-300">
            {/* Table Header */}
            <thead>
            <tr className="bg-gray-100">
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">WeekNumber</th>
                <th colSpan={3} className="border border-gray-300 px-4 py-2">TimeFrame</th>
                <th colSpan={2} className="border border-gray-300 px-4 py-2">Players</th>
                <th colSpan={2} className="border border-gray-300 px-4 py-2">Boards</th>
                <th colSpan={3} className="border border-gray-300 px-4 py-2">Revenue</th>
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">Payouts</th>
            </tr>
            <tr className="bg-gray-200">
                <th className="border border-gray-300 px-4 py-2">From</th>
                <th className="border border-gray-300 px-4 py-2">Until</th>
                <th className="border border-gray-300 px-4 py-2">Finish</th>
                <th className="border border-gray-300 px-4 py-2">Total</th>
                <th className="border border-gray-300 px-4 py-2">Winner</th>
                <th className="border border-gray-300 px-4 py-2">Total</th>
                <th className="border border-gray-300 px-4 py-2">Winning</th>
                <th className="border border-gray-300 px-4 py-2">Total</th>
                <th className="border border-gray-300 px-4 py-2">Club</th>
                <th className="border border-gray-300 px-4 py-2">Players</th>
            </tr>
            </thead>
            {/* Table Body */}
            <tbody>
            {data.map ((row, index) => (
                <tr
                    key={index} className="hover:bg-gray-50"
                    onClick={() => handleClickOnGame(row.gameInfo?.gameId)}

                >
                    <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                        {/* Center the Week Number */}
                        <span className="flex-1 text-center">{row.timeFrame?.weekNumber}</span>
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
                    <td className="border border-gray-300 px-4 py-2">{toDanishTimeFormat(row.timeFrame?.validFromDate)}</td>
                    <td className="border border-gray-300 px-4 py-2">{toDanishTimeFormat(row.timeFrame?.validUntilDate)}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.timeFrame?.finishedAt ? toDanishTimeFormat(row.timeFrame?.finishedAt) : <p className={"text-center"}>-</p>}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.players?.totalPlayers}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.winningPlayers?.totalWinningPlayers}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.boards?.totalBoards}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.winningBoards?.totalWinningBoards}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.income?.totalIncome}</td>
                    <td className="border border-gray-300 px-4 py-2">{(row.income?.totalIncome ?? 0) * 30}</td>
                    <td className="border border-gray-300 px-4 py-2">{(row.income?.totalIncome ?? 0) * 70}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.payouts?.totalPayouts}</td>

                </tr>
            ))}
            </tbody>
        </table>
    );
}

export default GamesTable;