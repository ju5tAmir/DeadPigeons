import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { TransactionResponse, UserInfo } from "../../../api.ts";
import { http } from "../../../http.ts";
import toast from "react-hot-toast";
import { toDanishTimeFormat } from "../../../utils/TimeUtils.ts";

function TransactionDetails() {
    const { id } = useParams<{ id: string }>();
    const [transaction, setTransaction] = useState<TransactionResponse | null>(null);
    const [user, setUser] = useState<UserInfo | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [newBalance, setNewBalance] = useState<number | string>(0);
    const [transactionStatusUpdated, setTransactionStatusUpdated] = useState<boolean>(false); // Track if status has been updated
    const navigate = useNavigate();

    const fetchTransactionDetails = async () => {
        try {
            if (!id) {
                toast.error("Transaction ID is missing.");
                return;
            }
            const res = await http.transactionDetail(id);
            setTransaction(res.data);
            setNewBalance(res.data.amount); // Set the initial balance to the fetched amount
            setTransactionStatusUpdated(false); // Reset status update flag
        } catch (error) {
            console.error("Error fetching transaction details:", error);
            toast.error("Failed to fetch transaction details.");
        } finally {
            setIsLoading(false);
        }
    };

    const fetchUser = async (userId: string) => {
        try {
            const res = await http.userDetail(userId);
            setUser(res.data);
        } catch (error) {
            console.error("Error fetching user details:", error);
            toast.error("Failed to fetch user details.");
        }
    };

    const handleApprove = async () => {
        toast.promise(http.transactionApproveCreate(transaction.transactionId, { amount: newBalance }), {
            success: "Transaction approved.",
            loading: "Approving...",
            error: "Failed to approve transaction."
        });

        // After approve, re-fetch transaction details
        await fetchTransactionDetails();
        setTransactionStatusUpdated(true); // Flag that status has been updated
    };

    const handleDecline = async () => {
        toast.promise(http.transactionDeclineDetail(transaction.transactionId), {
            success: "Transaction declined.",
            loading: "Declining...",
            error: "Failed to decline transaction."
        });

        // After decline, re-fetch transaction details
        await fetchTransactionDetails();
        setTransactionStatusUpdated(true); // Flag that status has been updated
    };

    useEffect(() => {
        fetchTransactionDetails();
    }, [id]);

    useEffect(() => {
        if (transaction?.userId) {
            fetchUser(transaction.userId);
        }
    }, [transaction]);

    return (
        <div className="min-h-screen bg-gradient-to-r from-indigo-500 to-purple-600 flex justify-center py-12">
            <div className="w-full max-w-7xl bg-white dark:bg-gray-800 p-8 rounded-xl shadow-lg">
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
                    <div className="flex flex-col md:flex-row gap-8">
                        {/* Left Section - User Details */}
                        <div className="w-full md:w-1/2 space-y-6">
                            <h2 className="text-2xl font-semibold text-gray-900 dark:text-white">User Details</h2>
                            {user ? (
                                <div className="space-y-4">
                                    <div className="text-gray-700 dark:text-gray-300">
                                        <span className="font-semibold text-indigo-700 dark:text-indigo-400">User ID:</span>
                                        <p className="mt-1">{user.userId}</p>
                                    </div>
                                    <div className="text-gray-700 dark:text-gray-300">
                                        <span className="font-semibold text-indigo-700 dark:text-indigo-400">Name:</span>
                                        <p className="mt-1">{user.firstName} {user.lastName}</p>
                                    </div>
                                    <div className="text-gray-700 dark:text-gray-300">
                                        <span className="font-semibold text-indigo-700 dark:text-indigo-400">Email:</span>
                                        <p className="mt-1">{user.email}</p>
                                    </div>
                                    <div className="text-gray-700 dark:text-gray-300">
                                        <span className="font-semibold text-indigo-700 dark:text-indigo-400">Role:</span>
                                        <p className="mt-1">{user.role}</p>
                                    </div>
                                </div>
                            ) : (
                                <div className="text-center text-gray-500 dark:text-gray-400">User not found.</div>
                            )}
                        </div>

                        {/* Right Section - Transaction Details */}
                        <div className="w-full md:w-1/2 space-y-6">
                            <h2 className="text-2xl font-semibold text-gray-900 dark:text-white">Transaction Details</h2>
                            {transaction ? (
                                <div className="space-y-4">
                                    <div className="text-gray-700 dark:text-gray-300">
                                        <span className="font-semibold text-indigo-700 dark:text-indigo-400">Transaction ID:</span>
                                        <p className="mt-1">{transaction.transactionId}</p>
                                    </div>
                                    <div className="text-gray-700 dark:text-gray-300">
                                        <span className="font-semibold text-indigo-700 dark:text-indigo-400">Payment Method:</span>
                                        <p className="mt-1">{transaction.paymentMethod}</p>
                                    </div>
                                    <div className="text-gray-700 dark:text-gray-300">
                                        <span className="font-semibold text-indigo-700 dark:text-indigo-400">Amount:</span>
                                        <div className="mt-1 flex items-center space-x-2">
                                            {transaction.status === "Pending" ? (
                                                <input
                                                    type="number"
                                                    value={newBalance}
                                                    onChange={(e) => setNewBalance(e.target.value)}
                                                    className="w-32 p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-indigo-500"
                                                />
                                            ) : (
                                                <span className="text-xl font-semibold text-indigo-600 dark:text-indigo-300">
                                                    ${transaction.amount}
                                                </span>
                                            )}
                                        </div>
                                    </div>
                                    <div className="text-gray-700 dark:text-gray-300">
                                        <span className="font-semibold text-indigo-700 dark:text-indigo-400">Status:</span>
                                        <p className="mt-1">{transaction.status}</p>
                                    </div>
                                    <div className="text-gray-700 dark:text-gray-300">
                                        <span className="font-semibold text-indigo-700 dark:text-indigo-400">Transaction Date:</span>
                                        <p className="mt-1">{toDanishTimeFormat(transaction.transactionDate)}</p>
                                    </div>
                                    <div className="text-gray-700 dark:text-gray-300">
                                        <span className="font-semibold text-indigo-700 dark:text-indigo-400">Note:</span>
                                        <p className="mt-1">{transaction.note || "No note provided."}</p>
                                    </div>

                                    {/* Hide buttons if status is not 'Pending' */}
                                    {transaction.status === "Pending" && !transactionStatusUpdated && (
                                        <div className="flex space-x-4 mt-6">
                                            <button
                                                onClick={handleApprove}
                                                className="w-full py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500"
                                            >
                                                Approve
                                            </button>
                                            <button
                                                onClick={handleDecline}
                                                className="w-full py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500"
                                            >
                                                Decline
                                            </button>
                                        </div>
                                    )}
                                </div>
                            ) : (
                                <div className="text-center text-gray-500 dark:text-gray-400">Transaction not found.</div>
                            )}
                        </div>
                    </div>
                ) : (
                    <div className="text-center text-gray-500 dark:text-gray-400">Transaction not found.</div>
                )}

                {/* Transaction Image Section */}
                {transaction?.imageUrl && (
                    <div className="mt-8">
                        <h3 className="text-xl font-semibold text-gray-900 dark:text-white">Transaction Image</h3>
                        <div className="mt-2 flex justify-center">
                            <img
                                src={transaction.imageUrl}
                                alt="Transaction"
                                className="w-full max-w-2xl h-auto rounded-lg shadow-md"
                            />
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}

export default TransactionDetails;
