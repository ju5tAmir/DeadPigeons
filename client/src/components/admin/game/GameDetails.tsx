interface GameDetails {
    weekNumber: number;
    timeFrame: { from: string; until: string; finishedAt: string };
    players: { online: number; offline: number; total: number };
    winners: { online: number; offline: number; total: number };
    boards: { online: number; offline: number; total: number };
    winningBoards: { online: number; offline: number; total: number };
    revenue: { online: number; offline: number; total: number };
    payouts: { online: number; offline: number; total: number };
}

function GameDetails({ game }: { game: GameDetails }) {
    return (
        <div className="max-w-4xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            {/* Header Section */}
            <div className="border-b pb-4 mb-6">
                <h1 className="text-2xl font-bold text-gray-800">Game Details</h1>
                <p className="text-sm text-gray-500">
                    Week {game.weekNumber} - From {game.timeFrame.from} to {game.timeFrame.until}
                </p>
            </div>

            {/* Game Details Section */}
            <div className="space-y-4">
                {/* TimeFrame Details */}
                <div>
                    <h2 className="text-lg font-semibold text-gray-700">TimeFrame</h2>
                    <ul className="mt-2 space-y-1 text-gray-600">
                        <li><strong>From:</strong> {game.timeFrame.from}</li>
                        <li><strong>Until:</strong> {game.timeFrame.until}</li>
                        <li><strong>Finished At:</strong> {game.timeFrame.finishedAt}</li>
                    </ul>
                </div>

                <div className="max-w-4xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
                    <h1 className="text-2xl font-bold text-gray-800 mb-4">Game Details</h1>
                    <table className="min-w-full border border-gray-300 text-gray-700">
                        {/* Table Header */}
                        <thead className="bg-gray-100">
                        <tr>
                            <th className="border border-gray-300 px-4 py-2 text-left">Category</th>
                            <th className="border border-gray-300 px-4 py-2 text-center">Online</th>
                            <th className="border border-gray-300 px-4 py-2 text-center">Offline</th>
                            <th className="border border-gray-300 px-4 py-2 text-center">Total</th>
                        </tr>
                        </thead>
                        {/* Table Body */}
                        <tbody>
                        {/* Players */}
                        <tr>
                            <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                                
                                <span className="flex-1 text-center">Players</span>
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
                            <td className="border border-gray-300 px-4 py-2 text-center">{game.players.online}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">{game.players.offline}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">{game.players.total}</td>
                        </tr>

                        {/* Winners */}
                        <tr>
                            <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                                
                                <span className="flex-1 text-center">Winners</span>
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
                            <td className="border border-gray-300 px-4 py-2 text-center">
                                {game.winners.online}
                            </td>
                            <td className="border border-gray-300 px-4 py-2 text-center">
                                {game.winners.offline}
                            </td>
                            <td className="border border-gray-300 px-4 py-2 text-center">{game.winners.total}</td>
                        </tr>

                        {/* Boards */}
                        <tr>
                            <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                                
                                <span className="flex-1 text-center">Boards</span>
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
                            <td className="border border-gray-300 px-4 py-2 text-center">{game.boards.online}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">{game.boards.offline}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">{game.boards.total}</td>
                        </tr>

                        {/* Winning Boards */}
                        <tr>
                            <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                                
                                <span className="flex-1 text-center">Winning Boards</span>
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
                            <td className="border border-gray-300 px-4 py-2 text-center">{game.winningBoards.online}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">{game.winningBoards.offline}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">{game.winningBoards.total}</td>
                        </tr>

                        {/* Revenue */}
                        <tr>
                            <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                                
                                <span className="flex-1 text-center">Net Revenue</span>
                            </td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${game.revenue.online.toFixed (2)}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${game.revenue.offline.toFixed (2)}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${game.revenue.total.toFixed (2)}</td>
                        </tr>

                        {/* Club Revenue */}
                        <tr>
                            <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                                
                                <span className="flex-1 text-center">Club (30%)</span>
                            </td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${(game.revenue.online * 0.30).toFixed (2)} </td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${(game.revenue.offline * 0.30).toFixed (2)}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${(game.revenue.total * 0.30).toFixed (2)}</td>
                        </tr>

                        {/* Players Revenue */}
                        <tr>
                            <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                                
                                <span className="flex-1 text-center">Game (70%)</span>
                            </td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${(game.revenue.online * 0.70).toFixed (2)} </td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${(game.revenue.offline * 0.70).toFixed (2)}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${(game.revenue.total * 0.70).toFixed (2)}</td>
                        </tr>

                        {/* Payouts */}
                        <tr>
                            <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                                
                                <span className="flex-1 text-center">Payouts</span>
                            </td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${game.payouts.online.toFixed (2)}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${game.payouts.offline.toFixed (2)}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${game.payouts.total.toFixed (2)}</td>
                        </tr>

                        {/* Players Revenue */}
                        <tr>
                            <td className="border border-gray-300 px-4 py-2 flex justify-between items-center">
                                
                                <span className="flex-1 text-center">Next Game Pot</span>
                            </td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${(game.revenue.online * 0.70).toFixed (2)} </td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${(game.revenue.offline * 0.70).toFixed (2)}</td>
                            <td className="border border-gray-300 px-4 py-2 text-center">${(game.revenue.total * 0.70).toFixed (2)}</td>
                        </tr>
                        </tbody>
                    </table>
                </div>

                {/* Back Button */}
                <div className="mt-6">
                    <button
                        className="px-4 py-2 bg-blue-500 text-white rounded shadow hover:bg-blue-600"
                        onClick={() => window.history.back ()}
                >
                    Back
                </button>
            </div>

            </div>
        </div>
    );
}

export default GameDetails;