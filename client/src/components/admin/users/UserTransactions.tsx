import React from "react";

export interface UserTransactionsProps {
    transactionId: string;
    paymentMethod: string;
    amount: number;
    status: string;
    transactionDate: string; // Keep as a string to parse/display later
}

// ToDo: Create another page when clicked on the transaction, shows the image

function UserTransactions({ data }: { data: UserTransactionsProps[] }) {
    return (
        <div className="max-w-3xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h2 className="text-xl font-bold text-gray-800 mb-4">User Transactions</h2>
            <table className="w-full border-collapse border border-gray-300">
                <thead>
                <tr className="bg-gray-100">
                    <th className="border border-gray-300 px-4 py-2 text-left">Payment Method</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Amount</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Status</th>
                    <th className="border border-gray-300 px-4 py-2 text-left">Transaction Date</th>
                </tr>
                </thead>
                <tbody>
                {data.map((transaction) => (
                    <tr key={transaction.transactionId} className="hover:bg-gray-50">
                        <td className="border border-gray-300 px-4 py-2">
                            {transaction.paymentMethod}
                        </td>
                        <td className="border border-gray-300 px-4 py-2">
                            ${transaction.amount.toFixed(2)}
                        </td>
                        <td className="border border-gray-300 px-4 py-2">{transaction.status}</td>
                        <td className="border border-gray-300 px-4 py-2">
                            {new Date(transaction.transactionDate).toLocaleString()}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}

export default UserTransactions;
