import Navbar from "./components/general/Navbar.tsx";
import Footer from "./components/general/Footer.tsx";
import WeeksOfYear from "./components/game/WeeksOfYear.tsx";
import {Toaster} from "react-hot-toast";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {RoutePath} from "./utils/RoutePath.ts";
import Lab from "./pages/Lab.tsx";
import GamesOverview from "./components/user/game/GamesOverview.tsx";
import PlayGame from "./components/user/game/PlayGame.tsx";

function App() {
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
                          <Route path={RoutePath.game} element={<GamesOverview/>}/>
                          <Route path={RoutePath.game}>
                              <Route path=":id" element={<PlayGame />} />
                          </Route>
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

export default App
