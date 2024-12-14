import React from "react";
import {TransactionResponse} from "../../../api.ts";
import {useNavigate, useNavigation} from "react-router-dom";

const TransactionsTable = ({ transactions }: {transactions: TransactionResponse[]}) => {
    const navigate = useNavigate();

    function handleClick(transactionId: string) {
        navigate(transactionId);
    }

    return (
        <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
                <thead className="bg-gray-50 dark:bg-gray-800">
                <tr>
                    <th
                        scope="col"
                        className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider dark:text-gray-400"
                    >
                        Transaction ID
                    </th>
                    <th
                        scope="col"
                        className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider dark:text-gray-400"
                    >
                        Payment Method
                    </th>
                    <th
                        scope="col"
                        className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider dark:text-gray-400"
                    >
                        Amount
                    </th>
                    <th
                        scope="col"
                        className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider dark:text-gray-400"
                    >
                        Status
                    </th>
                    <th
                        scope="col"
                        className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider dark:text-gray-400"
                    >
                        Note
                    </th>
                    <th
                        scope="col"
                        className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider dark:text-gray-400"
                    >
                        Transaction Date
                    </th>
                </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200 dark:bg-gray-900 dark:divide-gray-700">
                {transactions.map((transaction) => (
                    <tr
                        key={transaction.transactionId}
                        onClick={() => handleClick(transaction.transactionId)}
                    >

                        <td className="px-6 py-4 text-sm text-gray-900 dark:text-white break-words">
                            {transaction.transactionId}
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-500 dark:text-gray-400">
                            {transaction.paymentMethod}
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-500 dark:text-gray-400">
                            ${transaction.amount.toFixed(2)}
                        </td>
                        <td className="px-6 py-4 text-sm">
                                <span
                                    className={`px-3 py-1 inline-flex text-xs font-semibold rounded-full ${
                                        transaction.status === "Confirmed"
                                            ? "bg-green-100 text-green-800"
                                            : transaction.status === "Pending"
                                                ? "bg-yellow-100 text-yellow-800"
                                                : "bg-red-100 text-red-800"
                                    }`}
                                >
                                    {transaction.status}
                                </span>
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-500 dark:text-gray-400">
                            {transaction.note || "-"}
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-500 dark:text-gray-400">
                            {new Date(transaction.transactionDate).toLocaleString()}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
};

export default TransactionsTable;
