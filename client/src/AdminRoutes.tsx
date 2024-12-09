import Navbar from "./components/admin/general/Navbar.tsx";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {RoutePath} from "./utils/admin/RoutePath.ts";
import Lab from "./pages/Lab.tsx";
import {Toaster} from "react-hot-toast";
import Footer from "./components/general/Footer.tsx";

function AdminRoutes() {
    return (
        <>
            <div className="flex flex-col min-h-screen">
                {/* Navbar stays at the top */}
                <Navbar/>

                {/* Main content stretches to fill the remaining space */}
                <main className="flex-grow">
                    <BrowserRouter>
                        <Routes>
                            <Route path={RoutePath.lab} element={<Lab/>}/>
                            <Route path={RoutePath.game} element={<Lab/>}/>
                        </Routes>
                    </BrowserRouter>
                    <Toaster
                        position="top-center"
                        reverseOrder={false}
                    />
                </main>

                {/* Footer stays at the bottom */}
                <Footer/>
            </div>
        </>
    )
}

export default AdminRoutes
