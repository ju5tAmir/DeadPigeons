import GamesTable from "../components/admin/GamesTable.tsx";

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


    return(
        <>
            <div className="p-4">
                <h1 className="text-xl font-bold mb-4">Games</h1>
                <GamesTable data={data} />
            </div>

        </>
    )
}

export default Lab;