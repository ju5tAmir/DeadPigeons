import React from "react";

export interface UserTransactionsProps {
    transactionId: string;
    paymentMethod: string;
    amount: number;
    status: string;
    imagePath: string; // Path to the image from the backend
    transactionDate: string;
}

function TransactionsDetails({ transaction }: { transaction: UserTransactionsProps }) {
    return (
        <div className="max-w-md mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h2 className="text-xl font-bold text-gray-800 mb-4">Transaction Details</h2>
            <div className="space-y-4">

                <div>
                    <p className="text-gray-600 font-medium">Transaction ID:</p>
                    <p className="text-gray-800">{transaction.transactionId}</p>
                </div>
                <div>
                    <p className="text-gray-600 font-medium">Payment Method:</p>
                    <p className="text-gray-800">{transaction.paymentMethod}</p>
                </div>
                <div>
                    <p className="text-gray-600 font-medium">Amount:</p>
                    <p className="text-gray-800">${transaction.amount.toFixed (2)}</p>
                </div>
                <div>
                    <p className="text-gray-600 font-medium">Status:</p>
                    <p className="text-gray-800">{transaction.status}</p>
                </div>
                <div>
                    <p className="text-gray-600 font-medium">Transaction Date:</p>
                    <p className="text-gray-800">
                        {new Date (transaction.transactionDate).toLocaleString ()}
                    </p>
                </div>
                <div className="flex justify-center">
                    <img
                        src={transaction.imagePath}
                        alt={`Transaction ${transaction.transactionId}`}
                        className="object-cover rounded-md shadow-md"
                    />
                </div>
            </div>
        </div>
    );
}

export default TransactionsDetails;
