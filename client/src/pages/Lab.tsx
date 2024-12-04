import GamesTable from "../components/admin/game/GamesTable.tsx";
import GameDetails from "../components/admin/game/GameDetails.tsx";

function Lab() {

    const data = [
        {
            weekNumber: 1,
            timeFrame: {
                from: "2024-11-01",
                until: "2024-11-07",
                finishedAt: "2024-11-07 18:00"
            },
            players: {
                total: 150,
                winners: "Alice, Bob"
            },
            boards: {
                total: 60,
                winning: 20
            },
            revenue: {
                total: 5000,
                club: 3000,
                players: 2000
            },
            payout: 2000
        },
        {
            weekNumber: 2,
            timeFrame: {
                from: "2024-11-08",
                until: "2024-11-14",
                finishedAt: "2024-11-14 18:00"
            },
            players: {
                total: 175,
                winners: "Charlie, Dana"
            },
            boards: {
                total: 70,
                winning: 25
            },
            revenue: {
                total: 5000,
                club: 3000,
                players: 2000
            },
            payout: 2000
        },
        {
            weekNumber: 3,
            timeFrame: {
                from: "2024-11-15",
                until: "2024-11-21",
                finishedAt: "2024-11-21 18:00"
            },
            players: {
                total: 200,
                winners: "Eve, Frank"
            },
            boards: {
                total: 80,
                winning: 30
            },
            revenue: {
                total: 5000,
                club: 3000,
                players: 2000
            },
            payout: 2000
        },
    ];

    const exampleGame: GameDetails = {
        weekNumber: 42,
        timeFrame: { from: "2024-11-01", until: "2024-11-07", finishedAt: "2024-11-07 18:00" },
        players: { online: 80, offline: 40, total: 120 },
        winners: { online: 12, offline: 2, total: 3 },
        boards: { online: 30, offline: 20, total: 50 },
        winningBoards: { online: 15, offline: 5, total: 20 },
        revenue: { online: 1000, offline: 500, total: 1500 },
        payouts: { online: 400, offline: 200, total: 600 },
    };

    return(
        <>
            {/*Games Table*/}
            {/*<div className="p-4">*/}
            {/*    <h1 className="text-xl font-bold mb-4">Games</h1>*/}
            {/*    <GamesTable data={data} />*/}
            {/*</div>*/}

            <GameDetails game={exampleGame} />
        </>
    )
}

export default Lab;