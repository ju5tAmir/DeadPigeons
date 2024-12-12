import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import toast from "react-hot-toast";
import { http } from "../../../http.ts";
import { GameLwResponse } from "../../../api.ts";

const UpdateOfflineProperties = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const [gameLw, setGameLw] = useState<GameLwResponse | null>(null);

    // State to hold the form values
    const [formData, setFormData] = useState({
        players: 0,
        winningPlayers: 0,
        boards: 0,
        winningBoards: 0,
        income: 0,
        payouts: 0,
    });

    // State to hold error messages
    const [errors, setErrors] = useState<{ [key: string]: string }>({});
    const [isSuccess, setIsSuccess] = useState(false);

    // Handle input changes
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };

    // Validation function
    const validate = () => {
        const newErrors: { [key: string]: string } = {};
        for (let field in formData) {
            if (formData[field as keyof typeof formData] < 0) {
                newErrors[field] = `${field} must be equal or greater than zero`;
            }
        }
        return newErrors;
    };

    // Handle form submission
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        // Validate form data
        const formErrors = validate();
        if (Object.keys(formErrors).length > 0) {
            setErrors(formErrors);
            return;
        }

        if (id) {
            toast.promise(
                http.gameUpdateOfflineUpdate(id, formData),
                {
                    loading: "Updating the game...",
                    success: () => {
                        setTimeout(() => {
                            navigate(-1);
                        }, 1000);
                        return "Updated successfully!";
                    },
                    error: (err) => {
                        const message = err?.response?.data?.error || "Failed to finish the game.";
                        return `Error: ${message}`;
                    }
                }
            );
        }
    };

    // Fetch game data
    const fetchGame = async (gameId: string) => {
        try {
            const res = await http.gameLwDetail(gameId);
            const gameData = res.data;

            // Set the game data
            setGameLw(gameData);

            // Set form data based on fetched game data
            setFormData({
                players: gameData?.players?.offlinePlayers || 0,
                winningPlayers: gameData?.winningPlayers?.offlineWinningPlayers || 0,
                boards: gameData?.boards?.offlineBoards || 0,
                winningBoards: gameData?.winningBoards?.offlineWinningBoards || 0,
                income: gameData?.income?.offlineIncome || 0,
                payouts: gameData?.payouts?.offlinePayouts || 0,
            });
        } catch (error) {
            toast.error("Failed to fetch game data.");
        }
    };

    // Fetch game data on component mount
    useEffect(() => {
        if (id) {
            fetchGame(id);
        }
    }, [id]);

    return (
        <div className="flex justify-center items-center min-h-screen bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-lg w-96">
                <h2 className="text-2xl font-semibold mb-6 text-center">Update Offline Properties</h2>

                {isSuccess && (
                    <div className="text-green-500 mb-4 text-center">
                        <p>Update Successful! Redirecting...</p>
                    </div>
                )}

                <form onSubmit={handleSubmit}>
                    <div className="mb-4">
                        <label htmlFor="players" className="block text-gray-700">Players</label>
                        <input
                            type="number"
                            id="players"
                            name="players"
                            value={formData.players}
                            onChange={handleChange}
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                        />
                        {errors.players && <p className="text-red-500 text-sm">{errors.players}</p>}
                    </div>

                    <div className="mb-4">
                        <label htmlFor="winningPlayers" className="block text-gray-700">Winning Players</label>
                        <input
                            type="number"
                            id="winningPlayers"
                            name="winningPlayers"
                            value={formData.winningPlayers}
                            onChange={handleChange}
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                        />
                        {errors.winningPlayers && <p className="text-red-500 text-sm">{errors.winningPlayers}</p>}
                    </div>

                    <div className="mb-4">
                        <label htmlFor="boards" className="block text-gray-700">Boards</label>
                        <input
                            type="number"
                            id="boards"
                            name="boards"
                            value={formData.boards}
                            onChange={handleChange}
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                        />
                        {errors.boards && <p className="text-red-500 text-sm">{errors.boards}</p>}
                    </div>

                    <div className="mb-4">
                        <label htmlFor="winningBoards" className="block text-gray-700">Winning Boards</label>
                        <input
                            type="number"
                            id="winningBoards"
                            name="winningBoards"
                            value={formData.winningBoards}
                            onChange={handleChange}
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                        />
                        {errors.winningBoards && <p className="text-red-500 text-sm">{errors.winningBoards}</p>}
                    </div>

                    <div className="mb-4">
                        <label htmlFor="income" className="block text-gray-700">Income</label>
                        <input
                            type="number"
                            id="income"
                            name="income"
                            value={formData.income}
                            onChange={handleChange}
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                        />
                        {errors.income && <p className="text-red-500 text-sm">{errors.income}</p>}
                    </div>

                    <div className="mb-4">
                        <label htmlFor="payouts" className="block text-gray-700">Payouts</label>
                        <input
                            type="number"
                            id="payouts"
                            name="payouts"
                            value={formData.payouts}
                            onChange={handleChange}
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                        />
                        {errors.payouts && <p className="text-red-500 text-sm">{errors.payouts}</p>}
                    </div>

                    <div className="flex justify-center">
                        <button
                            type="submit"
                            className="w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600"
                        >
                            Update
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default UpdateOfflineProperties;
