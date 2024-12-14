// DeleteUserModal.tsx
import React from "react";

interface DeleteUserModalProps {
    isModalOpen: boolean;
    cancelDelete: () => void;
    confirmDelete: () => void;
}

const DeleteUserModal: React.FC<DeleteUserModalProps> = ({ isModalOpen, cancelDelete, confirmDelete }) => {
    if (!isModalOpen) return null;

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
            <div className="bg-white p-6 rounded-lg shadow-lg w-96">
                <div className="flex justify-between items-center">
                    <h3 className="text-lg font-medium text-gray-700">Delete User?</h3>
                    <button onClick={cancelDelete} className="text-gray-400 hover:text-gray-600">
                        <svg xmlns="http://www.w3.org/2000/svg" className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" d="M6 18L18 6M6 6l12 12"/>
                        </svg>
                    </button>
                </div>
                <p className="mt-4 text-gray-600">Are you sure you want to delete this user? This action cannot be undone.</p>
                <div className="mt-6 flex justify-end gap-4">
                    <button onClick={cancelDelete} className="bg-gray-300 px-4 py-2 rounded-md hover:bg-gray-400">Cancel</button>
                    <button onClick={confirmDelete} className="bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600">Confirm</button>
                </div>
            </div>
        </div>
    );
};

export default DeleteUserModal;
