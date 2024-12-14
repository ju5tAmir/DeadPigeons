import PlayGame from "./PlayGame.tsx";
import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {BoardResponse, GameResponse, PackageResponse} from "../../../api.ts";
import {http} from "../../../http.ts";
import BoardsTable from "../board/BoardsTable.tsx";

function GameDetails() {
    const { id } = useParams();
    const [game, setGame] = useState<GameResponse | null>(null);
    const [boards, setBoards] = useState<BoardResponse[] | null>(null);

    const fetchGame = async (gameId: string) => {
        const res = await http.gameDetail(gameId);
        setGame(res.data);
    };

    const fetchBoards = async (gameId: string) => {
        const res = await http.boardGameDetail(gameId);

        setBoards(res.data)
    }

    useEffect(() => {
        if (id) fetchGame(id);
    }, [id]);



    const isActiveGame = (): boolean => {
        return game?.status === "Active";
    };

    return (
        <>
        <div className="max-w-7xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            {isActiveGame() ? <PlayGame/> : ""}
            <h1 className="text-2xl font-bold text-gray-800 mb-4">Played Boards</h1>
            <div className="space-y-4">
            {/* Played Boards */}
            <details
                className="border border-gray-300 rounded-lg"
                open={false}
                onToggle={(e) => {
                    if ((e.target as HTMLDetailsElement).open) {
                        fetchBoards(game?.gameId);
                    }
                }}
            >
                <summary
                    className="cursor-pointer px-6 py-4 text-gray-800 font-semibold bg-gray-100 rounded-t-lg hover:bg-gray-200">
                    Players Details
                </summary>
                <div className="px-6 py-4 bg-white border-t border-gray-300">
                    {boards ? (
                        <BoardsTable data={boards} />
                    ) : (
                        <p>Loading boards...</p>
                    )}
                </div>
            </details>
                </div>
            </div>
        </>
    )
}

export default GameDetails;