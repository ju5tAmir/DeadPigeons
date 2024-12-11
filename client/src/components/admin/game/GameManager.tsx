import {useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import {http} from "../../../http.ts";
import {GameLwResponse} from "../../../api.ts";
import GameDetails from "./GameDetails.tsx";

function GameManager(){
    const { id } = useParams();
    const [gameLwDetails, setGameLwDetails] = useState<GameLwResponse | null>(null);

    const fetchGameLightWeightDetails = async (gameId: string) => await http.gameLwDetail(gameId).then(res => setGameLwDetails(res.data));


    useEffect (() => {
        if (id) fetchGameLightWeightDetails(id);
    }, []);
    return(
        <>
            <div className="max-w-7xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
                <h1 className="text-2xl font-bold text-gray-800 mb-4">Game Management</h1>
                <div className="space-y-4">
                    <details
                        key={gameLwDetails?.gameInfo?.gameId}
                        className="border border-gray-300 rounded-lg"
                        open={false}
                        // onClick={() => loadGamesForYear (year)} // Handle click for year-specific games
                    >
                        <summary
                            className="cursor-pointer px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200">
                            Game Details
                        </summary>
                        <div className="px-6 py-4 bg-white border-t border-gray-300">
                            <GameDetails game={gameLwDetails}/>
                        </div>
                    </details>

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
        </>
    )
}

export default GameManager;