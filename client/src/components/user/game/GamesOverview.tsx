import React from "react";
import GamesTable from "./GamesTable.tsx";

function GamesOverview() {
    const years = [2020, 2021, 2022, 2023, 2024];
    const contentPlaceholder = 123; // Replace this later with your actual content


    const data = [
        {
            weekNumber: 1,
            timeFrame: {
                from: "2024-12-01 08:00",
                until: "2024-12-01 20:00",
                finishedAt: "2024-12-01 19:45",
            },
            boards: 5,
            status: "Completed",
        },
        {
            weekNumber: 2,
            timeFrame: {
                from: "2024-12-02 09:00",
                until: "2024-12-02 21:00",
                finishedAt: "2024-12-02 20:50",
            },
            boards: 8,
            status: "Completed",
        },
        {
            weekNumber: 3,
            timeFrame: {
                from: "2024-12-03 10:00",
                until: "2024-12-03 22:00",
                finishedAt: "2024-12-03 21:55",
            },
            boards: 7,
            status: "In Progress",
        },
        {
            weekNumber: 4,
            timeFrame: {
                from: "2024-12-04 11:00",
                until: "2024-12-04 23:00",
                finishedAt: "2024-12-04 22:30",
            },
            boards: 6,
            status: "Completed",
        },
        {
            weekNumber: 5,
            timeFrame: {
                from: "2024-12-05 12:00",
                until: "2024-12-05 18:00",
                finishedAt: "2024-12-05 17:45",
            },
            boards: 9,
            status: "Pending",
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
