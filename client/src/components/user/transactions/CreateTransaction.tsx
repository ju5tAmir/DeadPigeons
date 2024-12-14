import { useState } from "react";
import { useNavigate } from "react-router-dom"; // Import for navigation
import { http } from "../../../http.ts";
import { TransactionResponse } from "../../../api.ts";
import toast from "react-hot-toast";

function CreateTransaction() {
    const [response, setResponse] = useState<TransactionResponse | null>(null);
    const [image, setImage] = useState<File | null>(null);
    const [note, setNote] = useState<string>("");
    const navigate = useNavigate(); // Hook for navigation

    const handleUpload = async () => {
        if (!image) {
            toast.error("Please select an image before uploading.");
            return;
        }

        const formData = new FormData();
        formData.append("ImageFile", image);
        if (note) {
            formData.append("Note", note);
        }

        toast.promise(
            http.transactionCreateCreate(formData),
            {
                loading: "Uploading your transaction...",
                success: "Transaction created successfully!",
                error: "Something went wrong. Please try again.",
            }
        )
            .then((response) => {
                const data = response.data as TransactionResponse;
                setResponse(data); // Update response state
            })
            .catch((error) => {
                console.error("Error during upload:", error);
                toast.error("An error occurred while sending the request.");
            });
    };

    return (
        <div className="flex flex-col items-center gap-6 p-6 bg-gray-100 dark:bg-gray-800 rounded-lg shadow-md max-w-md mx-auto">
            <h1 className="text-2xl font-bold text-gray-900 dark:text-white">
                Upload Your Transaction
            </h1>

            {/* File Input */}
            <div className="w-full">
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                    Select Image
                </label>
                <input
                    type="file"
                    accept="image/*"
                    className="block w-full text-sm text-gray-900 border border-gray-300 rounded-lg cursor-pointer bg-gray-50 focus:outline-none dark:bg-gray-700 dark:text-gray-400 dark:border-gray-600"
                    onChange={(e) => setImage(e.target.files?.[0] || null)}
                />
            </div>

            {/* Note Input */}
            <div className="w-full">
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                    Note (Optional)
                </label>
                <textarea
                    className="w-full p-2 border border-gray-300 rounded-lg text-sm text-gray-900 dark:bg-gray-700 dark:text-gray-300 dark:border-gray-600 focus:outline-none focus:ring focus:ring-indigo-500"
                    placeholder="Enter a note (max 300 characters)"
                    maxLength={300}
                    onChange={(e) => setNote(e.target.value)}
                />
            </div>

            {/* CreateTransaction Button */}
            <button
                onClick={handleUpload}
                className="px-6 py-2 font-medium text-white bg-indigo-600 rounded-lg hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
            >
                Upload Image
            </button>

            {/* Back Button */}
            {response && (
                <button
                    onClick={() => navigate(-1)} // Navigate back to the previous page
                    className="mt-4 px-6 py-2 font-medium text-white bg-gray-500 rounded-lg hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-400"
                >
                    Back
                </button>
            )}
        </div>
    );
}

export default CreateTransaction;
