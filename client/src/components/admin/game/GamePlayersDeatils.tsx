export interface UserInfo {
    userId: string;
    firstName: string;
    lastName: string;
    username: string;
    email: string;
    phoneNumber: string;
    role: string;
    isActive: boolean;
    isAutoplay: boolean;
    registrationDate: Date;
}

function GamePlayersDetails({ players }: { players: UserInfo[] }) {
    return (
        <div className="max-w-6xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h1 className="text-2xl font-bold text-gray-800 mb-4">Game Players Details</h1>
            <table className="min-w-full border border-gray-300 text-gray-700">
                {/* Table Header */}
                <thead className="bg-gray-100">
                <tr>
                    <th className="border border-gray-300 px-4 py-2 text-left">Name</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Username</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Email</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Phone</th>
                    <th className="border border-gray-300 px-4 py-2 text-center">Role</th>
                    <th className="border border-gray-300 px-4 py-2 text-center">Active</th>
                    <th className="border border-gray-300 px-4 py-2 text-center">Autoplay</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Registration Date</th>
                </tr>
                </thead>
                {/* Table Body */}
                <tbody>
                {players.map((player) => (
                    <tr key={player.userId} className="hover:bg-gray-50">
                        <td className="border border-gray-300 px-4 py-2">
                            {player.firstName} {player.lastName}
                        </td>
                        <td className="border border-gray-300 px-4 py-2">{player.username}</td>
                        <td className="border border-gray-300 px-4 py-2">{player.email}</td>
                        <td className="border border-gray-300 px-4 py-2">{player.phoneNumber}</td>
                        <td className="border border-gray-300 px-4 py-2 text-center">{player.role}</td>
                        <td className="border border-gray-300 px-4 py-2 text-center">
                            {player.isActive ? "✅" : "❌"}
                        </td>
                        <td className="border border-gray-300 px-4 py-2 text-center">
                            {player.isAutoplay ? "✅" : "❌"}
                        </td>
                        <td className="border border-gray-300 px-4 py-2">
                            {new Date(player.registrationDate).toLocaleDateString()}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}

export default GamePlayersDetails;
