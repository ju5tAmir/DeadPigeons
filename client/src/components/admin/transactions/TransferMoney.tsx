import React, { useEffect, useState } from "react";
import toast from "react-hot-toast";
import { http } from "../../../http.ts";
import { UserInfo } from "../../../api.ts";
import {useNavigate} from "react-router-dom";

function TransferMoney() {
    const [formData, setFormData] = useState({
        recipient: "",
        amount: "",
        action: "send",
        note: "",
    });
    const [recipientDetails, setRecipientDetails] = useState<UserInfo | null>(null);
    const [isLoadingDetails, setIsLoadingDetails] = useState(false);
    const navigate = useNavigate();
    const [debouncedRecipient, setDebouncedRecipient] = useState("");

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        // Convert recipient to string if it’s a number
        setFormData((prev) => ({
            ...prev,
            [name]: name === "recipient" ? String(value) : value,
        }));
    };


    // Fetch recipient details with a debounce mechanism
    const fetchRecipientDetails = async (recipient: string) => {
        setIsLoadingDetails(true);

        try {
            const response = await http.userCreate({
                recipient: recipient
            });
            setRecipientDetails(response.data);
        } catch (error) {
            setRecipientDetails(null);
        } finally {
            setIsLoadingDetails(false);
        }
    };

    // Debounce the recipient input
    useEffect(() => {
        const handler = setTimeout(() => {
            setDebouncedRecipient(formData.recipient);
        }, 1000); // Delay of 1 second

        return () => {
            clearTimeout(handler); // Cleanup the timeout on change
        };
    }, [formData.recipient]);

    // Fetch recipient details only when debounced recipient changes
    useEffect(() => {
        if (debouncedRecipient) {
            fetchRecipientDetails(debouncedRecipient);
        } else {
            setRecipientDetails(null);
        }
    }, [debouncedRecipient]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!recipientDetails) {
            toast.error("Please verify recipient details before proceeding.");
            return;
        }

        const operation = formData.action === "send" ? "Add" : "Remove";

        try {
            await toast.promise(
                http.transactionSystemCreate({
                    recipiant: formData.recipient,
                    amount: formData.amount,
                    note: formData.note,
                    operation,
                }),
                {
                    success: () => {
                        setTimeout(() => {navigate(-1)}, 1000)

                        return "Transaction successful!";
                    },
                    loading: "Processing transaction...",
                    error: "Transaction failed. Please try again.",
                }
            );
            setFormData({ recipient: "", amount: "", action: "send", note: "" });
            setRecipientDetails(null);
        } catch (error) {
            console.error("Error processing transaction:", error);
        }
    };

    return (
        <div className="max-w-lg mx-auto mt-8 p-6 bg-white dark:bg-gray-800 shadow-md rounded-lg">
            <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-200 mb-6">Transfer Money</h2>
            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label className="block text-gray-600 dark:text-gray-300 font-medium mb-1">
                        Recipient (Email or Phone Number)
                    </label>
                    <input
                        type="text"
                        name="recipient"
                        value={formData.recipient}
                        onChange={handleChange}
                        placeholder="Enter UserId, email or phone number"
                        className="w-full border border-gray-300 dark:border-gray-600 bg-gray-50 dark:bg-gray-700 text-gray-800 dark:text-gray-200 px-3 py-2 rounded-md"
                        required
                    />
                </div>

                {isLoadingDetails ? (
                    <div className="text-gray-500 dark:text-gray-400 text-center">Loading recipient details...</div>
                ) : recipientDetails ? (
                    <div
                        className="bg-gray-50 dark:bg-gray-700 border border-gray-300 dark:border-gray-600 p-4 rounded-md">
                        <p className="text-gray-800 dark:text-gray-200 font-medium">
                            Account Holder: <span
                            className="font-semibold">{`${recipientDetails.firstName} ${recipientDetails.lastName}`}</span>
                        </p>
                        <p className="text-gray-800 dark:text-gray-200 font-medium">
                            Email: <span
                            className="font-semibold">{recipientDetails.email}</span>
                        </p>
                        <p className="text-gray-800 dark:text-gray-200 font-medium">
                            Phone: <span
                            className="font-semibold">{recipientDetails.phoneNumber}</span>
                        </p>
                        <p className="text-gray-800 dark:text-gray-200 font-medium">
                            Current Balance:{" "}
                            <span className="font-semibold">${recipientDetails.balance.toFixed (2)}</span>
                        </p>
                    </div>
                ) : formData.recipient ? (
                    <div className="text-red-500">Recipient details could not be fetched. Please check the input.</div>
                ) : null}

                <div>
                    <label className="block text-gray-600 dark:text-gray-300 font-medium mb-1">Amount</label>
                    <input
                        type="number"
                        name="amount"
                        value={formData.amount}
                        onChange={handleChange}
                        placeholder="Enter amount"
                        className="w-full border border-gray-300 dark:border-gray-600 bg-gray-50 dark:bg-gray-700 text-gray-800 dark:text-gray-200 px-3 py-2 rounded-md"
                        required
                        min="1"
                    />
                </div>
                <div>
                    <label className="block text-gray-600 dark:text-gray-300 font-medium mb-1">Action</label>
                    <select
                        name="action"
                        value={formData.action}
                        onChange={handleChange}
                        className="w-full border border-gray-300 dark:border-gray-600 bg-gray-50 dark:bg-gray-700 text-gray-800 dark:text-gray-200 px-3 py-2 rounded-md"
                        required
                    >
                        <option value="send">Send Money</option>
                        <option value="reduce">Reduce Money</option>
                    </select>
                </div>
                <div>
                    <label className="block text-gray-600 dark:text-gray-300 font-medium mb-1">Note</label>
                    <input
                        type="text"
                        name="note"
                        value={formData.note}
                        onChange={handleChange}
                        placeholder="Enter transaction note"
                        className="w-full border border-gray-300 dark:border-gray-600 bg-gray-50 dark:bg-gray-700 text-gray-800 dark:text-gray-200 px-3 py-2 rounded-md"
                    />
                </div>
                <div className="flex justify-end space-x-4 mt-6">
                    <button
                        type="button"
                        className="bg-gray-300 dark:bg-gray-600 text-gray-700 dark:text-gray-200 px-4 py-2 rounded hover:bg-gray-400 dark:hover:bg-gray-500"
                        onClick={() => setFormData({ recipient: "", amount: "", action: "send", note: "" })}
                    >
                        Cancel
                    </button>
                    <button
                        type="submit"
                        className="bg-blue-500 dark:bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-600 dark:hover:bg-blue-700"
                        disabled={!formData.recipient || !formData.amount}
                    >
                        Submit
                    </button>
                </div>
            </form>
        </div>
    );
}

export default TransferMoney;
