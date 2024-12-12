import GameDetails from "../game/GameDetails.tsx";
import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {UserInfo} from "../../../api.ts";
import {http} from "../../../http.ts";
import ViewUser from "./ViewUser.tsx";


function UserManager() {
    const { id } = useParams();
    const [user, setUser] = useState<UserInfo>();


    const fetchUser = async (id: string) => {
        const res = await http.userDetail(id);
        setUser(res.data);
    }

    useEffect (() => {
        if (id) fetchUser(id);
    }, []);



    return (
        <>
            <ViewUser user={user} />
            <div className="max-w-7xl mx-auto mt-8 p-6 bg-white shadow-md rounded-lg">

                {/* Back Button */}
                <div className="mt-6">
                    <button
                        className="px-4 py-2 bg-blue-500 text-white rounded shadow hover:bg-blue-600"
                        onClick={() => window.history.back()}
                    >
                        Back
                    </button>
                </div>
            </div>
        </>
    );
}

export default UserManager;