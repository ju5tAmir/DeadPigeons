import { http } from "../../http.ts";
import {useEffect, useState} from "react";
import toast from "react-hot-toast";
import { useNavigate } from "react-router-dom";
import {GameResponse} from "../../api.ts";

const WeeksOfYear = () => {
    const weeks = Array.from({ length: 52 }, (_, i) => i + 1);
    const [game, setGame] = useState<GameResponse>();
    // const navigate = useNavigate();

    // Get the current week of the year
    const currentWeek = Math.ceil(
        (new Date().getTime() - new Date(new Date().getFullYear(), 0, 1).getTime()) /
        (7 * 24 * 60 * 60 * 1000)
    );

    async function gameLoader() {
        const response = await http.gameList();
        return response.data;
    }

    // Function to trigger toast on loading the games
    const loadGames = async () => {
        // Using toast.promise to show success, error, and loading states
        const data = await toast.promise (gameLoader (), {
            loading: "Loading games...",
            success: "Games loaded successfully!",
            error: "Failed to load games. Please try again.",
        });

        setGame(data);
    };

    // Ensure the request only happens once when the component is first mounted
    useEffect(() => {
        loadGames();
    }, []); // Empty dependency array ensures this runs only once when the component is mounted

    return (
        <div className="p-6 bg-gray-100 min-h-screen flex flex-col items-center justify-center">
            <div className="grid grid-cols-10 gap-3">
                {weeks.map((week) => (
                    <div
                        key={week}
                        className={`w-20 h-20 flex items-center justify-center text-sm font-semibold rounded shadow-md ${
                            week === currentWeek
                                ? "bg-green-500 text-white"
                                : "bg-blue-500 text-white hover:bg-blue-600"
                        } cursor-pointer`}
                        title={`Week ${week}`}
                        onClick={
                            week === currentWeek && game?.gameId
                                ? () => {
                                    window.location.href = `/game/${game.gameId}`;
                                }
                                : undefined // If the condition is false, do nothing
                        }
                    >
                        {week}
                    </div>
                ))}
            </div>
        </div>
    );
};

export default WeeksOfYear;
