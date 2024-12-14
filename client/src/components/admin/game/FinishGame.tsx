import React, { useState, useEffect } from "react";
import { GameResponse } from "../../../api.ts";
import { http } from "../../../http.ts";
import toast from "react-hot-toast";
import { useNavigate, useParams } from "react-router-dom";
import {RoutePath} from "../../../utils/admin/RoutePath.ts";

const FinishGame = () => {
    const { id } = useParams();
    const [game, setGame] = useState<GameResponse | null>(null);
    const [selectedTiles, setSelectedTiles] = useState<number[]>([]);
    const [isRequesting, setIsRequesting] = useState(false); // To manage request status
    const navigate = useNavigate();

    const disableTile = selectedTiles.length >= 3;

    // Fetch game details
    const fetchGame = async (gameId: string) => {
        const res = await http.gameDetail(gameId);
        setGame(res.data);
    };

    const handleRedirect = () => {
        navigate(`${RoutePath.game}/${game?.gameId}`); // Navigate to the game detail page
    };

    // Handle finish game request
    const handleFinishGame = async () => {
        if (selectedTiles.length === 3) {
            setIsRequesting(true);
            toast.promise(
                http.gameFinishCreate({
                    gameId: game?.gameId, // Send the game ID with selected tiles
                    winningSequence: selectedTiles
                }),
                {
                    loading: "Finishing the game...",
                    success: () => {
                        // Call handleRedirect inside the success callback
                        handleRedirect();
                        return "Game finished successfully!";
                    },
                    error: (err) => {
                        const message = err?.response?.data?.error || "Failed to finish the game.";
                        return `Error: ${message}`;
                    },
                }
            ).finally(() => {
                setIsRequesting(false);
            });
        } else {
            toast.error("Please select exactly 3 tiles to finish the game.");
        }
    };

    // Handle tile selection
    const handleTileClick = (tileIndex: number) => {
        if (selectedTiles.includes(tileIndex)) {
            // Deselect the tile
            setSelectedTiles(selectedTiles.filter((tile) => tile !== tileIndex));
        } else {
            // Select the tile, but only if the limit isn't reached
            if (selectedTiles.length < 3) {
                setSelectedTiles([...selectedTiles, tileIndex]);
            }
        }
    };

    // Fetch game data when component mounts
    useEffect(() => {
        if (id) fetchGame(id);
    }, [id]);

    return (
        <div className="max-w-4xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h1 className="text-2xl font-bold text-gray-800 mb-4">Admin: Finish Game</h1>

            {/* Tile Selection */}
            <div className="grid grid-cols-4 gap-4">
                {Array.from({ length: 16 }, (_, index) => (
                    <div
                        key={index}
                        className={`p-4 border rounded-lg cursor-pointer transition-all ${
                            selectedTiles.includes(index + 1) ? "bg-blue-500 text-white scale-110" : "bg-gray-200"
                        }`}
                        onClick={() => handleTileClick(index + 1)}
                        style={{
                            pointerEvents: disableTile && !selectedTiles.includes(index + 1) ? "none" : "auto",
                        }}
                    >
                        <div className="text-center">{index + 1}</div>
                    </div>
                ))}
            </div>

            {/* Message for tile count */}
            <div className="mt-4">
                {selectedTiles.length === 3 ? (
                    <p className="text-lg font-semibold text-green-600">You have selected 3 tiles.</p>
                ) : (
                    <p className="text-lg font-semibold text-red-600">Please select exactly 3 tiles.</p>
                )}
            </div>

            {/* Finish Button */}
            <div className="mt-6">
                <button
                    className="mt-4 px-6 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 transition duration-200"
                    disabled={selectedTiles.length !== 3 || isRequesting}
                    onClick={handleFinishGame}
                >
                    {isRequesting ? "Finishing..." : "Finish Game"}
                </button>
            </div>
        </div>
    );
};

export default FinishGame;
