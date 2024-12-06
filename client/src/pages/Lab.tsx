import GamesTable from "../components/admin/game/GamesTable.tsx";
import GameDetails from "../components/admin/game/GameDetails.tsx";
import React from "react";
import GamePlayersDetails, {UserInfo} from "../components/admin/game/GamePlayersDeatils.tsx";
import GameBoardsDetails, {Boards} from "../components/admin/game/GameBoardsDetails.tsx";
import GamesOverview from "../components/admin/game/GamesOverview.tsx";
import UsersOverview from "../components/admin/users/UsersOverview.tsx";
import EditUser from "../components/admin/users/EditUser.tsx";
import ViewUser from "../components/admin/users/ViewUser.tsx";
import CreateUser from "../components/admin/users/CreateUser.tsx";
import UserGames from "../components/admin/users/UserGames.tsx";
import userGames from "../components/admin/users/UserGames.tsx";
import UserBoards from "../components/admin/users/UserBoards.tsx";
import UserTransactions from "../components/admin/users/UserTransactions.tsx";

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

    const mockPlayers: UserInfo[] = [
        {
            userId: "1",
            firstName: "John",
            lastName: "Doe",
            username: "johndoe123",
            email: "johndoe@example.com",
            phoneNumber: "+1234567890",
            role: "Player",
            isActive: true,
            isAutoplay: false,
            registrationDate: new Date("2023-01-15"),
        },
        {
            userId: "2",
            firstName: "Jane",
            lastName: "Smith",
            username: "janesmith456",
            email: "janesmith@example.com",
            phoneNumber: "+9876543210",
            role: "Admin",
            isActive: true,
            isAutoplay: true,
            registrationDate: new Date("2023-02-20"),
        },
        {
            userId: "3",
            firstName: "Alice",
            lastName: "Johnson",
            username: "alicej",
            email: "alicej@example.com",
            phoneNumber: "+1122334455",
            role: "Player",
            isActive: false,
            isAutoplay: false,
            registrationDate: new Date("2023-03-10"),
        },
    ];

    const mockBoards: Boards[] = [
        {
            player: {
                fullName: "JerneIF",
                email: "dot@dot.com"
            },
            package: {
                numberOfFields: 20,
                price: 50.5,
            },
            playSequence: new Set([1, 2, 3, 4]),
            playTime: new Date("2023-04-15T10:30:00"),
        },
        {
            player: {
                fullName: "JerneIF",
                email: "dot@dot.com"
            },
            package: {
                numberOfFields: 30,
                price: 75.0,
            },
            playSequence: new Set([5, 6, 7, 8]),
            playTime: new Date("2023-05-10T14:00:00"),
        },
        {
            player: {
                fullName: "JerneIF",
                email: "dot@dot.com"
            },
            package: {
                numberOfFields: 25,
                price: 60.25,
            },
            playSequence: new Set([9, 10, 11]),
            playTime: new Date("2023-06-01T09:45:00"),
        },
    ];


    const mockUser = {
        userId: "3",
        firstName: "Alice",
        lastName: "Johnson",
        username: "alicej",
        email: "alicej@example.com",
        phoneNumber: "+1122334455",
        role: "Player",
        isActive: false,
        isAutoplay: false,
        registrationDate: new Date("2023-03-10"),
    };

    const userGamesData = [
        {
            weekNumber: 43,
            boards: 3,
        },
        {
            weekNumber: 44,
            boards: 1,
        }
    ]

    const userBoardsData = [
        {
            weekNumber: 43,
            packagePrice: 80,
            playSequence: [1,2,3,4,5],
            playDate: new Date().getDate(),
            isWinner: false
        },
        {
            weekNumber: 43,
            packagePrice: 80,
            playSequence: [5,6,7,8,9],
            playDate: new Date().getDate(),
            isWinner: true
        },
        {
            weekNumber: 44,
            packagePrice: 100,
            playSequence: [1,2,3,4,5,12],
            playDate: new Date().getDate(),
            isWinner: false
        }
    ]

    const transactionsData = [
        {
            transactionId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            paymentMethod: "Credit Card",
            amount: 120.5,
            status: "Completed",
            transactionDate: "2024-12-06T18:21:59.874Z",
        },
        {
            transactionId: "a12b34c56d78e90f12g34h56",
            paymentMethod: "PayPal",
            amount: 45.99,
            status: "Pending",
            transactionDate: "2024-12-05T14:11:22.000Z",
        },
    ]

    return(
        <>
            {/*Games Table*/}
            {/*<div className="p-4">*/}
            {/*    <h1 className="text-xl font-bold mb-4">Games</h1>*/}
            {/*    <GamesTable data={data} />*/}
            {/*</div>*/}

            {/*<GameDetails game={exampleGame} />*/}

            {/*<GamePlayersDetails players={mockPlayers} />*/}

            {/*<GameBoardsDetails boards={mockBoards} />*/}

            {/*<GamesOverview/>*/}

            {/*<UsersOverview players={mockPlayers}/>*/}

            {/*<EditUser user={mockUser}/>*/}

            {/*<ViewUser user={mockUser} />*/}

            {/*<CreateUser/>*/}

            {/*<UserGames data={userGamesData} />*/}

            {/*<UserBoards data={userBoardsData}/>*/}

            <UserTransactions data={transactionsData} />

        </>
    )
}

export default Lab;