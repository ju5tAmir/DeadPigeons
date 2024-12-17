import React, { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import {BrowserRouter, Routes} from "react-router-dom";
import {Toaster} from "react-hot-toast";

createRoot(document.getElementById('root')!).render(
    <BrowserRouter>
        <App/>
        <Toaster
            position="top-center"
            reverseOrder={false}
        />
    </BrowserRouter>
)
