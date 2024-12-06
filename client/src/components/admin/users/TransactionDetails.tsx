export interface UserTransactionsProps {
    transactionId: string;
    paymentMethod: string;
    amount: number;
    status: string;
    imagePath: string;
    transactionDate: string; // Keep as a string to parse/display later
}

function TransactionsDetails(){
    return (
        <></>
    )
}

export default TransactionsDetails;