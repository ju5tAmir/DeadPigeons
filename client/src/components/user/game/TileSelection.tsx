import React, { useState } from "react";

const tilePrices = {
    5: 20,
    6: 70,
};

const TileSelection = () => {
    const [selectedTiles, setSelectedTiles] = useState<number[]>([]);
    const [maxTilesSelected, setMaxTilesSelected] = useState(false);

    const handleTileClick = (tileIndex: number) => {
        if (selectedTiles.includes(tileIndex)) {
            // Deselect the tile
            setSelectedTiles(selectedTiles.filter((tile) => tile !== tileIndex));
        } else {
            // Select the tile, but only if the limit isn't reached
            if (selectedTiles.length < 8) {
                setSelectedTiles([...selectedTiles, tileIndex]);
            }
        }
    };

    const calculatePrice = () => {
        if (selectedTiles.length >= 5 && selectedTiles.length <= 6) {
            return tilePrices[selectedTiles.length] || 0;
        }
        return 0;
    };

    const disableTile = selectedTiles.length >= 8;

    return (
        <div className="max-w-4xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h1 className="text-2xl font-bold text-gray-800 mb-4">Select Tiles</h1>

            <div className="grid grid-cols-4 gap-4">
                {Array.from({ length: 16 }, (_, index) => (
                    <div
                        key={index}
                        className={`p-4 border rounded-lg cursor-pointer transition-all ${
                            selectedTiles.includes(index) ? "bg-blue-500 text-white scale-110" : "bg-gray-200"
                        }`}
                        onClick={() => handleTileClick(index)}
                        style={{
                            pointerEvents: disableTile && !selectedTiles.includes(index) ? "none" : "auto",
                        }}
                    >
                        <div className="text-center">Tile {index + 1}</div>
                    </div>
                ))}
            </div>

            <div className="mt-6">
                <p className="text-lg font-semibold">Price: ${calculatePrice()}</p>
                {selectedTiles.length >= 5 && (
                    <button
                        className="mt-4 px-6 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 transition duration-200"
                        disabled={selectedTiles.length === 8}
                    >
                        Play
                    </button>
                )}
            </div>
        </div>
    );
};

export default TileSelection;
