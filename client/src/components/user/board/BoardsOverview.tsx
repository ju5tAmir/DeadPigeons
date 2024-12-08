import { http } from "../../../http.ts";
import React, { useEffect, useState } from "react";
import { BoardResponse } from "../../../api.ts";
import BoardsTable from "./BoardsTable.tsx";

export default function BoardsOverview() {
    const [years, setYears] = useState<number[] | null>(null);
    const [boards, setBoards] = useState<BoardResponse[] | null>([]);
    const [loading, setLoading] = useState(true); // Loading state

    // Fetch all boards
    async function getAllBoards() {
        setLoading(true);
        await http.boardAllList().then((res) => {
            setBoards(res.data);
        });
        setLoading(false);
    }

    // Load years from the boards
    function yearsLoader() {
        if (boards) {
            const yearsSet = new Set<number>();
            boards.forEach((b) => {
                const year = b.game?.year;
                if (year) {
                    yearsSet.add(year); // Use a Set to ensure unique years
                }
            });
            setYears(Array.from(yearsSet)); // Convert the Set back to an array
        }
    }

    function mostRecentYear() {
        if (years && years.length > 0) {
            return Math.max(...years);
        }
    }

    function loadBoardForYear(year: number) {
        if (boards && boards.length > 0) {
            return boards.filter((board) => board.game?.year === year);
        }
    }

    useEffect(() => {
        if (boards) {
            yearsLoader();
        }
    }, [boards]);

    useEffect(() => {
        getAllBoards();
    }, []);

    if (loading) {
        return <div>Loading...</div>;
    }


    return (
        <div className="max-w-7xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h1 className="text-2xl font-bold text-gray-800 mb-4">Boards Overview</h1>
            <div className="space-y-4">
                {years?.map((year) => (
                    <details
                        key={year}
                        className="border border-gray-300 rounded-lg"
                        open={year === mostRecentYear()}
                        onClick={() => loadBoardForYear(year)} // Handle click for year-specific boards
                    >
                        <summary className="cursor-pointer px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200">
                            {year}
                        </summary>
                        <div className="px-6 py-4 bg-white border-t border-gray-300">
                            <BoardsTable data={boards!.filter((b) => b.game?.year === year)} />
                        </div>
                    </details>
                ))}
            </div>
        </div>
    );
}