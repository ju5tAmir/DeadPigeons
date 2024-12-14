import {toDanishTimeFormat} from "../../../utils/TimeUtils.ts";
import {GamePlayerDetails} from "../../../api.ts";

function GamePlayers({ players }: {players: GamePlayerDetails[]}){
    return (
        <table className="min-w-full border-collapse border border-gray-300">
            {/* Table Header */}
            <thead>
            <tr className="bg-gray-100">
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">FirstName</th>
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">LastName</th>
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">Username</th>
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">IsAutoPlay</th>
            </tr>
            </thead>
            {/* Table Body */}
            <tbody>
            {players.map ((row, index) => (
                <tr
                    key={index} className="hover:bg-gray-50"
                >
                    <td className="border border-gray-300 px-4 py-2">{row.firstName}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.lastName}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.username}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.autoPlay}</td>
                </tr>
            ))}
            </tbody>
        </table>
    );
}

export default GamePlayers;