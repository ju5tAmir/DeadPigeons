import React, { useEffect, useState } from "react";
import { GameResponse, PackageResponse } from "../../../api.ts";
import { http } from "../../../http.ts";
import toast from "react-hot-toast";
import { useParams } from "react-router-dom";

const PlayGame = () => {
    const { id } = useParams();
    const [packages, setPackages] = useState<PackageResponse[] | null>([]);
    const [selectedTiles, setSelectedTiles] = useState<number[]>([]);
    const [currentPackage, setCurrentPackage] = useState<PackageResponse | null>(null);
    const [game, setGame] = useState<GameResponse | null>(null);
    const [playSequence, setPlaySequence] = useState<number[]>([]);

    const disableTile = selectedTiles.length >= 8;

    const fetchGame = async (gameId: string) => {
        const res = await http.gameDetail(gameId);
        setGame(res.data);
    };

    const getPackages = async () => {
        const res = await http.packageAllList();
        setPackages(res.data);
    };

    const handlePlay = async () => {
        if (!game || !currentPackage) return;

        toast.promise(
            http.boardPlayCreate({
                gameId: game.gameId,
                packageId: currentPackage.packageId,
                playSequence,
            }),
            {
                loading: "Sending data ...",
                success: () => {
                    setTimeout(() => {window.location.reload();}, 1000)
                    return "Play was successful!"},
                error: (err) => {
                    // Safely access the error message
                    const message = err?.response?.data?.error || "Failed to play.";
                    return `Error: ${message}`;
                },
            }
        );

    };

    const handleTileClick = (tileIndex: number) => {
        if (selectedTiles.includes(tileIndex)) {
            // Deselect the tile
            setSelectedTiles(selectedTiles.filter((tile) => tile !== tileIndex));

            // Remove the tile index from playSequence when deselected
            setPlaySequence(playSequence.filter((tile) => tile !== tileIndex));
        } else {
            // Select the tile, but only if the limit isn't reached
            if (selectedTiles.length < 8) {
                setSelectedTiles([...selectedTiles, tileIndex]);

                // Add the tile index to playSequence when selected
                setPlaySequence([...playSequence, tileIndex]);
            }
        }
    };


    const calculatePrice = () => {
        if (selectedTiles.length >= 5) {
            const packageFound = packages?.find(p => p.numberOfFields === selectedTiles.length);
            return packageFound ? packageFound.price : 0;
        }
        return 0;
    };

    useEffect(() => {
        if (selectedTiles.length >= 5) {
            const packageFound = packages?.find(p => p.numberOfFields === selectedTiles.length);
            setCurrentPackage(packageFound ?? null); // Only update when necessary
        } else {
            setCurrentPackage(null);
        }
    }, [selectedTiles, packages]); // Re-run when selectedTiles or packages changes

    useEffect(() => {
        if (id) fetchGame(id);
    }, [id]);

    useEffect(() => {
        getPackages();
    }, []);

    return (
        <div className="max-w-4xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h1 className="text-2xl font-bold text-gray-800 mb-4">Select Tiles</h1>

            <div className="grid grid-cols-4 gap-4">
                {Array.from({ length: 16 }, (_, index) => (
                    <div
                        key={index}
                        className={`p-4 border rounded-lg cursor-pointer transition-all ${
                            selectedTiles.includes(index+1) ? "bg-blue-500 text-white scale-110" : "bg-gray-200"
                        }`}
                        onClick={() => handleTileClick(index+1)}
                        style={{
                            pointerEvents: disableTile && !selectedTiles.includes(index+1) ? "none" : "auto",
                        }}
                    >
                        <div className="text-center">{index + 1}</div>
                    </div>
                ))}
            </div>

            <div className="mt-6">
                <p className="text-lg font-semibold">Price: ${calculatePrice()}</p>
                {selectedTiles.length >= 5 && (
                    <button
                        className="mt-4 px-6 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 transition duration-200"
                        disabled={selectedTiles.length > 8}
                        onClick={handlePlay}
                    >
                        Play
                    </button>
                )}
            </div>
        </div>
    );
};

export default PlayGame;
