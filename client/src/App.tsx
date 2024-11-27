import Navbar from "./components/general/Navbar.tsx";
import Footer from "./components/general/Footer.tsx";
import WeeksOfYear from "./components/game/WeeksOfYear.tsx";
import {Toaster} from "react-hot-toast";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import React from "react";
import {RoutePath} from "./utils/RoutePath.ts";

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
                          <Route path={RoutePath.home} element={</>}/>
                      </Routes>
                  </BrowserRouter>
                  <WeeksOfYear/>
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
