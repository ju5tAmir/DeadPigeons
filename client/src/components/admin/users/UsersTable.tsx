// UsersTable.tsx
import React from 'react';
import { UserInfo } from "../../../api.ts";
import { toDanishTimeFormat } from "../../../utils/TimeUtils.ts";

interface UsersTableProps {
    userList: UserInfo[] | null;
    handleView: (userId: string) => void;
    handleEdit: (userId: string) => void;
    handleDelete: (userId: string) => void;
}

const UsersTable: React.FC<UsersTableProps> = ({ userList, handleView, handleEdit, handleDelete }) => {
    return (
        <>
        <table className="min-w-full border border-gray-300 text-gray-700">
            <thead className="bg-gray-100">
            <tr>
                <th className="border border-gray-300 px-4 py-2 text-left">Name</th>
                <th className="border border-gray-300 px-4 py-2 text-left">Username</th>
                <th className="border border-gray-300 px-4 py-2 text-left">Username</th>
                <th className="border border-gray-300 px-4 py-2 text-left">Email</th>
                <th className="border border-gray-300 px-4 py-2 text-left">Phone</th>
                <th className="border border-gray-300 px-4 py-2 text-center">Role</th>
                <th className="border border-gray-300 px-4 py-2 text-center">Active</th>
                <th className="border border-gray-300 px-4 py-2 text-center">Autoplay</th>
                <th className="border border-gray-300 px-4 py-2 text-left">Registration Date</th>
                <th className="border border-gray-300 px-4 py-2 text-center">Actions</th>
            </tr>
            </thead>
            <tbody>
            {userList?.map((player) => (
                <tr key={player.userId} className="hover:bg-gray-50">
                    <td className="border border-gray-300 px-4 py-2">{player.firstName} {player.lastName}</td>
                    <td className="border border-gray-300 px-4 py-2">{player.username}</td>
                    <td className="border border-gray-300 px-4 py-2 text-center">{player.balance} kr</td>
                    <td className="border border-gray-300 px-4 py-2">{player.email}</td>
                    <td className="border border-gray-300 px-4 py-2">{player.phoneNumber}</td>
                    <td className="border border-gray-300 px-4 py-2 text-center">{player.role}</td>
                    <td className="border border-gray-300 px-4 py-2 text-center">{player.isActive ? "✅" : "❌"}</td>
                    <td className="border border-gray-300 px-4 py-2 text-center">{player.isAutoplay ? "✅" : "❌"}</td>
                    <td className="border border-gray-300 px-4 py-2">{toDanishTimeFormat(player.registerationDate)}</td>
                    <td className="border border-gray-300 px-4 py-2 flex text-center align-middle">
                        <button onClick={() => handleView(player.userId)} className="bg-blue-500 text-white px-3 py-1 rounded hover:bg-blue-600 mr-2">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor" className="size-6">
                                <path strokeLinecap="round" strokeLinejoin="round" d="M2.036 12.322a1.012 1.012 0 0 1 0-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178Z"/>
                                <path strokeLinecap="round" strokeLinejoin="round" d="M15 12a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"/>
                            </svg>
                        </button>
                        <button onClick={() => handleEdit(player.userId)} className="bg-orange-400 text-white px-3 py-1 rounded hover:bg-orange-600 mr-2">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor" className="size-6">
                                <path strokeLinecap="round" strokeLinejoin="round" d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10"/>
                            </svg>
                        </button>
                        <button onClick={() => handleDelete(player.userId)} className="bg-red-500 text-white px-3 py-1 rounded hover:bg-red-600">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" className="size-6">
                                <path d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0"/>
                            </svg>
                        </button>
                    </td>
                </tr>
            ))}
            </tbody>
        </table>
            </>
    );
};

export default UsersTable;
