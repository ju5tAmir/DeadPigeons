import {BoardResponse} from "../../../api.ts";
import {toDanishTimeFormat} from "../../../utils/TimeUtils.ts";

export default function BoardsTable({data}: {data: BoardResponse[]}){

    return (
        <table className="min-w-full border-collapse border border-gray-300">
            {/* Table Header */}
            <thead>
            <tr className="bg-gray-100">
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">WeekNumber</th>
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">Price</th>
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">PlaySequence</th>
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">PlayDate</th>
                <th rowSpan={2} className="border border-gray-300 px-4 py-2">Won</th>
            </tr>
            </thead>
            {/* Table Body */}
            <tbody>
            {data.map ((row, index) => (
                <tr key={index} className="hover:bg-gray-50">
                    <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                        {/* Center the Week Number */}
                        <span className="flex-1 text-center">{row.game?.weekNumber}</span>
                    </td>
                    <td className="border border-gray-300 px-4 py-2">{row.package?.price}</td>
                    <td className="border border-gray-300 px-4 py-2">{row.playSequence}</td>
                    <td className="border border-gray-300 px-4 py-2">{toDanishTimeFormat(row.playTime!)}</td>


                </tr>
            ))}
            </tbody>
        </table>
    );
}