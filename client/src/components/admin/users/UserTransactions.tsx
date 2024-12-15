import React from "react";
import TransactionsTable from "../transactions/TransactionsTable.tsx";
import {TransactionResponse} from "../../../api.ts";



function UserTransactions({transactions} : {transactions: TransactionResponse[]}) {
    return (
        <TransactionsTable transactions={transactions}/>
    );
}

export default UserTransactions;
