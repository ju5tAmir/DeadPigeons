import {GameResponse} from "../../../api.ts";
import {toDanishTimeFormat} from "../../../utils/TimeUtils.ts";
import {useNavigate} from "react-router-dom";
import {RoutePath} from "../../../utils/user/RoutePath.ts";

function    GamesTable({ data }: { data: GameResponse[] }) {
    const navigate = useNavigate();

    const handleClickOnGame = (gameId: string) => {
        navigate(`${RoutePath.games}/${gameId}`)
    }

    return (
        <table className="min-w-full border-collapse border border-gray-300">
            {/* Table Header */}
            <thead>
            <tr className="bg-gray-100">
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">WeekNumber</th>
                <th colSpan={3} className="border border-gray-300 px-4 py-2">TimeFrame</th>
                {/*<th rowSpan={2} className="border border-gray-300 px-4 py-2">Your Boards</th>*/}
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">Game Status</th>

            </tr>
            <tr className="bg-gray-200">
                <th className="border border-gray-300 px-4 py-2">From</th>
                <th className="border border-gray-300 px-4 py-2">Until</th>
                <th className="border border-gray-300 px-4 py-2">Finish</th>

            </tr>
            </thead>
            {/* Table Body */}
            <tbody>
            {data.map ((row, index) => (
                <tr
                    key={index}
                    className="hover:bg-gray-50 cursor-pointer"
                    onClick={() => handleClickOnGame (row.gameId!)}
                >
                    <td className="border border-gray-300 px-4 py-2 text-center">
                        <span>{row.weekNumber}</span>
                    </td>
                    <td className="border border-gray-300 px-4 py-2 text-center">
                        {toDanishTimeFormat (row.validFromDate!)}
                    </td>
                    <td className="border border-gray-300 px-4 py-2 text-center">
                        {toDanishTimeFormat (row.validUntilDate!)}
                    </td>
                    <td className="border border-gray-300 px-4 py-2 text-center">
                        {row.finishedAt == null ? "Ongoing" : toDanishTimeFormat (row.finishedAt)}
                    </td>
                    <td className="border border-gray-300 px-4 py-2 text-center">
                        {row.status}
                    </td>
                </tr>
            ))}
            </tbody>

        </table>
    );
}

export default GamesTable;