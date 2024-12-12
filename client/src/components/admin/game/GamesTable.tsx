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
                    key={index} className="hover:bg-gray-50 cursor-pointer"
                    onClick={() => handleClickOnGame (row.gameInfo?.gameId)}

                >
                    <td className="text-center border border-gray-300 px-4 py-2">{row.timeFrame?.weekNumber}</td>

                    <td className="border border-gray-300 px-4 py-2">{toDanishTimeFormat (row.timeFrame?.validFromDate)}</td>
                    <td className="border border-gray-300 px-4 py-2">{toDanishTimeFormat (row.timeFrame?.validUntilDate)}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.timeFrame?.finishedAt ? toDanishTimeFormat (row.timeFrame?.finishedAt) :
                        <p className={"text-center"}>-</p>}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.players?.totalPlayers}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.winningPlayers?.totalWinningPlayers}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.boards?.totalBoards}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.winningBoards?.totalWinningBoards}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.income?.totalIncome}</td>
                    <td className="border border-gray-300 px-4 py-2">
                        {(row.income?.totalIncome ? (row.income.totalIncome * 0.30) : 0).toFixed (2)}
                    </td>
                    <td className="border border-gray-300 px-4 py-2">
                        {(row.income?.totalIncome ? (row.income.totalIncome * 0.70) : 0).toFixed (2)}
                    </td>

                    <td className="border border-gray-300 px-4 py-2">{row.payouts?.totalPayouts}</td>

                </tr>
            ))}
            </tbody>
        </table>
    );
}

export default GamesTable;