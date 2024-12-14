import React, {useEffect, useState} from "react";
import GamesTable from "./GamesTable.tsx";
import {GameLwResponse, GameResponse} from "../../../api.ts";
import {http} from "../../../http.ts";

function GamesOverview() {
    const [years, setYears] = useState<number[] | null>(null);
    const [games, setGames] = useState<GameLwResponse[]>([]);

    async function yearsLoader() {
        const response = await http.gameYearsList();
        setYears(response.data);
    }

    async function loadGamesForYear(year: number) {
        // If games for this year are already loaded, skip the request
        if (games.some((g) => g.timeFrame?.year === year)) return;

        const newGames = await http.gameYearLwDetail(year).then((res) => res.data);
        if (newGames) {
            setGames((prevGames) => [...prevGames, ...newGames]);
        }
    }

    async function loadMostRecentYear(){
        if (years && years.length > 0) {
            const mostRecentYear = Math.max(...years);
            await loadGamesForYear(mostRecentYear);
        }
    }

    // Get the most recent year
    function mostRecentYear() {
        if (years && years.length > 0) {
            return Math.max(...years);
        }
    }

    // Load the years when the component mounts
    useEffect(() => {
        yearsLoader();
    }, []);

    useEffect(() => {
        loadMostRecentYear();
    }, [years]);


    return (
        <div className="max-w-7xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h1 className="text-2xl font-bold text-gray-800 mb-4">Yearly Details</h1>
            <div className="space-y-4">
                {years?.map((year) => (
                    <details
                        key={year}
                        className="border border-gray-300 rounded-lg"
                        open={year === mostRecentYear()}
                        onClick={() => loadGamesForYear(year)} // Handle click for year-specific games
                    >
                        <summary className="cursor-pointer px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200">
                            {year}
                        </summary>
                        <div className="px-6 py-4 bg-white border-t border-gray-300">
                            <GamesTable data={games.filter((g) => g.timeFrame?.year === year)} />
                        </div>
                    </details>
                ))}
            </div>
        </div>
    );
}

export default GamesOverview;
