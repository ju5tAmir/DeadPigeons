import { useEffect, useState } from "react";
import { TransactionResponse } from "../../../api.ts";
import { http } from "../../../http.ts";
import TransactionsTable from "./TransactionsTable.tsx";
import { useNavigate } from "react-router-dom";

function Transactions() {
    const [transactions, setTransactions] = useState<TransactionResponse[] | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const navigate = useNavigate();

    const fetchTransactions = async () => {
        try {
            const res = await http.transactionAllList();
            setTransactions(res.data);
        } catch (error) {
            console.error("Error fetching transactions:", error);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        fetchTransactions();
    }, []);

    const handleCreateTransaction = () => {
        navigate("transfer");
    };

    return (
        <div className="p-6 bg-gray-50 dark:bg-gray-900 min-h-screen">
            <div className="max-w-6xl mx-auto">
                <div className="flex justify-between items-center mb-6">
                    <h1 className="text-2xl font-bold text-gray-900 dark:text-white">
                        Transactions
                    </h1>
                    <button
                        onClick={handleCreateTransaction}
                        className="px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
                    >
                        Transfer Money
                    </button>
                </div>

                {isLoading ? (
                    <div className="flex justify-center items-center h-64">
                        <div className="animate-spin rounded-full h-10 w-10 border-t-2 border-b-2 border-indigo-600"></div>
                    </div>
                ) : transactions && transactions.length > 0 ? (
                    <TransactionsTable transactions={transactions} />
                ) : (
                    <div className="text-center text-gray-500 dark:text-gray-400">
                        No transactions found.
                    </div>
                )}
            </div>
        </div>
    );
}

export default Transactions;
