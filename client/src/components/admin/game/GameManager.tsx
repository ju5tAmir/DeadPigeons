import GameDetails from "./GameDetails.tsx";
import GamePlayers from "./GamePlayers.tsx";
import {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import {
    BoardResponse,
    GameBoardsDetails,
    GameLwResponse,
    GamePlayerDetails,
    WinnersRequest,
    WinnersResponse
} from "../../../api.ts";
import {http} from "../../../http.ts";
import GameBoardDetails from "./GameBoardDetails.tsx";
import GameWinners from "./GameWinners.tsx";

function GameManager() {
    const { id } = useParams();
    const [gameLwDetails, setGameLwDetails] = useState<GameLwResponse | null>(null);
    const [players, setPlayers] = useState<GamePlayerDetails[] | null>(null);
    const [playersFetched, setPlayersFetched] = useState(false); // Track if players are fetched
    const [boards, setBoards] = useState<GameBoardsDetails[] | null>(null);
    const [boardsFetched, setBoardsFetched] = useState(false);
    const [winners, setWinners] = useState<WinnersResponse | null>(null);
    const [winnersFetched, setWinnersFetched] = useState(false);


    const fetchGameLightWeightDetails = async (gameId: string) => {
        const response = await http.gameLwDetail(gameId);
        setGameLwDetails(response.data);
    };

    const fetchGamePlayers = async () => {
        if (!playersFetched && id) {
            const response = await http.gamePlayersDetail(id);
            setPlayers(response.data);
            setPlayersFetched(true); // Mark players as fetched
        }
    };

    const fetchGameBoards = async () => {
        if (!boardsFetched && id) {
            const response = await http.gameBoardsDetail(id);
            setBoards(response.data);
            setBoardsFetched(true);
        }
    };

    const fetchGameWinners = async () => {
        if (!winnersFetched && id) {
            const response = await http.winnerDetail(id);
            setWinners(response.data);
            setWinnersFetched(true);
        }
    };

    useEffect(() => {
        if (id) fetchGameLightWeightDetails(id);
    }, [id]);

    return (
        <>
            <div className="max-w-7xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
                <h1 className="text-2xl font-bold text-gray-800 mb-4">Game Management</h1>
                <div className="space-y-4">
                    {/* Game Details */}
                    <details
                        key={gameLwDetails?.gameInfo?.gameId}
                        className="border border-gray-300 rounded-lg"
                        open={true}
                    >
                        <summary
                            className="cursor-pointer px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200">
                            Game Details
                        </summary>
                        <div className="px-6 py-4 bg-white border-t border-gray-300">
                            <GameDetails game={gameLwDetails} />
                        </div>
                    </details>

                    {/* Players Details */}
                    <details
                        className="border border-gray-300 rounded-lg"
                        open={false}
                        onToggle={(e) => {
                            if ((e.target as HTMLDetailsElement).open) {
                                fetchGamePlayers();
                            }
                        }}
                    >
                        <summary
                            className="cursor-pointer px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200">
                            Players Details
                        </summary>
                        <div className="px-6 py-4 bg-white border-t border-gray-300">
                            {players ? (
                                <GamePlayers players={players} />
                            ) : (
                                <p>Loading players...</p>
                            )}
                        </div>
                    </details>

                    {/* Boards Details */}
                    <details
                        className="border border-gray-300 rounded-lg"
                        open={false}
                        onToggle={(e) => {
                            if ((e.target as HTMLDetailsElement).open) {
                                fetchGameBoards();
                            }
                        }}
                    >
                        <summary
                            className="cursor-pointer px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200">
                            Boards Details
                        </summary>
                        <div className="px-6 py-4 bg-white border-t border-gray-300">
                            {boards ? (
                                <GameBoardDetails boards={boards} />
                            ) : (
                                <p>Loading Boards...</p>
                            )}
                        </div>
                    </details>


                    <details
                        className="border border-gray-300 rounded-lg"
                        open={false}
                        onToggle={(e) => {
                            if ((e.target as HTMLDetailsElement).open && gameLwDetails?.gameInfo?.status === 'finished') {
                                fetchGameWinners();
                            }
                        }}
                    >
                        <summary
                            className={` px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200 ${
                                gameLwDetails?.gameInfo?.status !== 'Finished' ? 'text-gray-400 cursor-not-allowed' : 'cursor-pointer'
                            }`}
                            onClick={(e) => {
                                // Prevent toggling if the game isn't finished
                                if (gameLwDetails?.gameInfo?.status !== 'Finished') {
                                    e.preventDefault();
                                } else {
                                    fetchGameWinners();
                                }
                            }}
                        >
                            Winners
                        </summary>
                        <div className="px-6 py-4 bg-white border-t border-gray-300">
                            {winners ? (
                                <GameWinners result={winners} />
                            ) : (
                                <p>Loading Winners...</p>
                            )}
                        </div>
                    </details>



                </div>

                {/* Back Button */}
                <div className="mt-6">
                    <button
                        className="px-4 py-2 bg-blue-500 text-white rounded shadow hover:bg-blue-600"
                        onClick={() => window.history.back()}
                    >
                        Back
                    </button>
                </div>
            </div>
        </>
    );
}

export default GameManager;
