import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { TransactionResponse } from "../../../api.ts";
import { http } from "../../../http.ts";
import toast from "react-hot-toast";
import { toDanishTimeFormat } from "../../../utils/TimeUtils.ts";

function TransactionDetails() {
    const { id } = useParams<{ id: string }>();
    const [transaction, setTransaction] = useState<TransactionResponse | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const navigate = useNavigate();

    const fetchTransactionDetails = async () => {
        try {
            if (!id) {
                toast.error("Transaction ID is missing.");
                return;
            }
            const res = await http.transactionDetail(id);
            setTransaction(res.data);
        } catch (error) {
            console.error("Error fetching transaction details:", error);
            toast.error("Failed to fetch transaction details.");
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        fetchTransactionDetails();
    }, [id]);

    return (
        <div className="min-h-screen bg-gradient-to-r from-indigo-500 to-purple-600 flex items-center justify-center">
            <div className="w-full max-w-4xl bg-white dark:bg-gray-800 p-8 rounded-xl shadow-lg overflow-hidden">
                <button
                    onClick={() => navigate(-1)}
                    className="mb-6 px-6 py-2 bg-gray-200 text-gray-800 rounded-lg hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2"
                >
                    &larr; Back
                </button>

                {isLoading ? (
                    <div className="flex justify-center items-center h-48">
                        <div className="animate-spin rounded-full h-12 w-12 border-t-4 border-t-indigo-600 border-b-4 border-gray-400"></div>
                    </div>
                ) : transaction ? (
                    <div className="space-y-8">
                        <h1 className="text-3xl font-bold text-gray-900 dark:text-white text-center">
                            Transaction Details
                        </h1>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 text-lg">
                            <div className="text-gray-700 dark:text-gray-300">
                                <span className="font-semibold text-indigo-700 dark:text-indigo-400">
                                    Transaction ID:
                                </span>
                                <p className="mt-1">{transaction.transactionId}</p>
                            </div>

                            <div className="text-gray-700 dark:text-gray-300">
                                <span className="font-semibold text-indigo-700 dark:text-indigo-400">
                                    Payment Method:
                                </span>
                                <p className="mt-1">{transaction.paymentMethod}</p>
                            </div>

                            <div className="text-gray-700 dark:text-gray-300">
                                <span className="font-semibold text-indigo-700 dark:text-indigo-400">
                                    Amount:
                                </span>
                                <p className="mt-1 text-xl font-semibold text-indigo-600 dark:text-indigo-300">
                                    ${transaction.amount.toFixed(2)}
                                </p>
                            </div>

                            <div className="text-gray-700 dark:text-gray-300">
                                <span className="font-semibold text-indigo-700 dark:text-indigo-400">
                                    Status:
                                </span>
                                <p className="mt-1">{transaction.status}</p>
                            </div>

                            <div className="text-gray-700 dark:text-gray-300">
                                <span className="font-semibold text-indigo-700 dark:text-indigo-400">
                                    Transaction Date:
                                </span>
                                <p className="mt-1">{toDanishTimeFormat(transaction.transactionDate)}</p>
                            </div>

                            <div className="col-span-2 text-gray-700 dark:text-gray-300">
                                <span className="font-semibold text-indigo-700 dark:text-indigo-400">
                                    Note:
                                </span>
                                <p className="mt-1">{transaction.note || "No note provided."}</p>
                            </div>
                        </div>

                        {transaction.imageUrl && (
                            <div className="mt-8">
                                <span className="font-semibold text-indigo-700 dark:text-indigo-400">
                                    Transaction Image:
                                </span>
                                <div className="mt-2 flex justify-center">
                                    <img
                                        src={transaction.imageUrl}
                                        alt="Transaction"
                                        className="w-96 h-auto rounded-lg shadow-md" // Set a fixed width here
                                    />
                                </div>
                            </div>
                        )}
                    </div>
                ) : (
                    <div className="text-center text-gray-500 dark:text-gray-400">
                        Transaction not found.
                    </div>
                )}
            </div>
        </div>
    );
}

export default TransactionDetails;
