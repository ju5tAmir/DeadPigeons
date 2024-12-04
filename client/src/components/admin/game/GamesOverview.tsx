import React from "react";
import GamesTable from "./GamesTable.tsx";

function GamesOverview() {
    const years = [2020, 2021, 2022, 2023, 2024];
    const contentPlaceholder = 123; // Replace this later with your actual content


    const data = [
        {
            weekNumber: 1,
            timeFrame: {
                from: "2024-11-01",
                until: "2024-11-07",
                finishedAt: "2024-11-07 18:00"
            },
            players: {
                total: 150,
                winners: "Alice, Bob"
            },
            boards: {
                total: 60,
                winning: 20
            },
            revenue: {
                total: 5000,
                club: 3000,
                players: 2000
            },
            payout: 2000
        },
        {
            weekNumber: 2,
            timeFrame: {
                from: "2024-11-08",
                until: "2024-11-14",
                finishedAt: "2024-11-14 18:00"
            },
            players: {
                total: 175,
                winners: "Charlie, Dana"
            },
            boards: {
                total: 70,
                winning: 25
            },
            revenue: {
                total: 5000,
                club: 3000,
                players: 2000
            },
            payout: 2000
        },
        {
            weekNumber: 3,
            timeFrame: {
                from: "2024-11-15",
                until: "2024-11-21",
                finishedAt: "2024-11-21 18:00"
            },
            players: {
                total: 200,
                winners: "Eve, Frank"
            },
            boards: {
                total: 80,
                winning: 30
            },
            revenue: {
                total: 5000,
                club: 3000,
                players: 2000
            },
            payout: 2000
        },
    ];

    return (
        <div className="max-w-7xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h1 className="text-2xl font-bold text-gray-800 mb-4">Yearly Details</h1>
            <div className="space-y-4">
                {years.reverse().map ((year) => (
                    <details key={year} className="border border-gray-300 rounded-lg " open={year === new Date().getFullYear()}>
                        <summary
                            className="cursor-pointer px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200">
                            {year}
                        </summary>
                        <div className="px-6 py-4 bg-white border-t border-gray-300">
                            <GamesTable data={data}/>
                        </div>
                    </details>
                ))}
            </div>
        </div>
    );
}

export default GamesOverview;
