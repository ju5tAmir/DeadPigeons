import { useState } from "react";
import { http } from "../../../http.ts";
import { useNavigate } from "react-router-dom";
import { GameLwResponse } from "../../../api.ts";
import toast from "react-hot-toast"; // Assuming you're using react-hot-toast for notifications

interface UpdateOfflineProps {
    game: GameLwResponse;
}

function UpdateOfflineProperties({ game }: UpdateOfflineProps) {
    // const navigate = useNavigate();
    //
    // // State for editable values
    // const [playersOffline, setPlayersOffline] = useState(game.players.offline);
    // const [winnersOffline, setWinnersOffline] = useState(game.winners.offline);
    // const [boardsOffline, setBoardsOffline] = useState(game.boards.offline);
    // const [winningBoardsOffline, setWinningBoardsOffline] = useState(game.winningBoards.offline);
    // const [revenueOffline, setRevenueOffline] = useState(game.revenue.offline);
    // const [payoutsOffline, setPayoutsOffline] = useState(game.payouts.offline);
    //
    // // Function to handle the form submission
    // const handleSubmit = async () => {
    //     try {
    //         // Update the offline properties with a PUT request (adjust based on your API)
    //         const response = await http.put("/update-offline", {
    //             playersOffline,
    //             winnersOffline,
    //             boardsOffline,
    //             winningBoardsOffline,
    //             revenueOffline,
    //             payoutsOffline
    //         });
    //
    //         // Assuming success, display a success toast
    //         if (response.status === 200) {
    //             toast.success("Offline properties updated successfully.");
    //         } else {
    //             throw new Error("Failed to update offline properties");
    //         }
    //     } catch (error) {
    //         toast.error("Error updating offline properties.");
    //     }
    // };
    //
    // return (
    //     <div className="max-w-4xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
    //         <h1 className="text-2xl font-bold text-gray-800 mb-4">Update Offline Properties</h1>
    //
    //         {/* Form */}
    //         <div className="space-y-4">
    //             <div className="flex flex-col space-y-2">
    //                 <label htmlFor="playersOffline" className="text-lg font-semibold text-gray-700">Players (Offline)</label>
    //                 <input
    //                     type="number"
    //                     id="playersOffline"
    //                     value={playersOffline}
    //                     onChange={(e) => setPlayersOffline(parseInt(e.target.value))}
    //                     className="px-4 py-2 border border-gray-300 rounded-md"
    //                 />
    //             </div>
    //
    //             <div className="flex flex-col space-y-2">
    //                 <label htmlFor="winnersOffline" className="text-lg font-semibold text-gray-700">Winners (Offline)</label>
    //                 <input
    //                     type="number"
    //                     id="winnersOffline"
    //                     value={winnersOffline}
    //                     onChange={(e) => setWinnersOffline(parseInt(e.target.value))}
    //                     className="px-4 py-2 border border-gray-300 rounded-md"
    //                 />
    //             </div>
    //
    //             <div className="flex flex-col space-y-2">
    //                 <label htmlFor="boardsOffline" className="text-lg font-semibold text-gray-700">Boards (Offline)</label>
    //                 <input
    //                     type="number"
    //                     id="boardsOffline"
    //                     value={boardsOffline}
    //                     onChange={(e) => setBoardsOffline(parseInt(e.target.value))}
    //                     className="px-4 py-2 border border-gray-300 rounded-md"
    //                 />
    //             </div>
    //
    //             <div className="flex flex-col space-y-2">
    //                 <label htmlFor="winningBoardsOffline" className="text-lg font-semibold text-gray-700">Winning Boards (Offline)</label>
    //                 <input
    //                     type="number"
    //                     id="winningBoardsOffline"
    //                     value={winningBoardsOffline}
    //                     onChange={(e) => setWinningBoardsOffline(parseInt(e.target.value))}
    //                     className="px-4 py-2 border border-gray-300 rounded-md"
    //                 />
    //             </div>
    //
    //             <div className="flex flex-col space-y-2">
    //                 <label htmlFor="revenueOffline" className="text-lg font-semibold text-gray-700">Revenue (Offline)</label>
    //                 <input
    //                     type="number"
    //                     id="revenueOffline"
    //                     value={revenueOffline}
    //                     onChange={(e) => setRevenueOffline(parseFloat(e.target.value))}
    //                     className="px-4 py-2 border border-gray-300 rounded-md"
    //                 />
    //             </div>
    //
    //             <div className="flex flex-col space-y-2">
    //                 <label htmlFor="payoutsOffline" className="text-lg font-semibold text-gray-700">Payouts (Offline)</label>
    //                 <input
    //                     type="number"
    //                     id="payoutsOffline"
    //                     value={payoutsOffline}
    //                     onChange={(e) => setPayoutsOffline(parseFloat(e.target.value))}
    //                     className="px-4 py-2 border border-gray-300 rounded-md"
    //                 />
    //             </div>
    //
    //             <div className="mt-6">
    //                 <button
    //                     onClick={handleSubmit}
    //                     className="w-full px-6 py-3 font-semibold rounded-lg shadow-md bg-blue-500 text-white hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-400 focus:ring-opacity-75"
    //                 >
    //                     Update Offline Properties
    //                 </button>
    //             </div>
    //         </div>
    //     </div>
    // );
}

export default UpdateOfflineProperties;
