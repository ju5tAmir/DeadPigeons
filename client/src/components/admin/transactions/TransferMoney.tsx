import React, { useState } from "react";

// Should get the account holder name, balance and other information first, and display before sending.

function TransferMoney() {
    const [formData, setFormData] = useState({
        recipient: "",
        amount: "",
        action: "send", // Default action
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        console.log("Form submitted:", formData);
        // Add logic to send this data to the backend
    };

    return (
        <div className="max-w-lg mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">
            <h2 className="text-xl font-bold text-gray-800 mb-6">Transfer Money</h2>
            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Recipient (Email or Phone Number)</label>
                    <input
                        type="text"
                        name="recipient"
                        value={formData.recipient}
                        onChange={handleChange}
                        placeholder="Enter email or phone number"
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                        required
                    />
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Amount</label>
                    <input
                        type="number"
                        name="amount"
                        value={formData.amount}
                        onChange={handleChange}
                        placeholder="Enter amount"
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                        required
                        min="1"
                    />
                </div>
                <div>
                    <label className="block text-gray-600 font-medium mb-1">Action</label>
                    <select
                        name="action"
                        value={formData.action}
                        onChange={handleChange}
                        className="w-full border border-gray-300 px-3 py-2 rounded-md"
                        required
                    >
                        <option value="send">Send Money</option>
                        <option value="reduce">Reduce Money</option>
                    </select>
                </div>
                <div className="flex justify-end space-x-4 mt-6">
                    <button
                        type="button"
                        className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400"
                        onClick={() => setFormData({ recipient: "", amount: "", action: "send" })}
                    >
                        Cancel
                    </button>
                    <button
                        type="submit"
                        className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                    >
                        Submit
                    </button>
                </div>
            </form>
        </div>
    );
}

export default TransferMoney;
