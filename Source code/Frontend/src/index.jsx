import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App"
import AuthProvider from "./context/AuthContext";
import "./index.css"; 
import { SignalRProvider } from "./context/SignalRContext";

ReactDOM.createRoot(document.getElementById("root")).render(
    <React.StrictMode>
        <AuthProvider>
            <SignalRProvider>
                <App />
            </SignalRProvider>
        </AuthProvider>
    </React.StrictMode>
);
