import * as signalR from "@microsoft/signalr";
import { createContext, useContext, useState } from "react";
const SignalRContext = createContext(null);

export function useSignalR() {
  return useContext(SignalRContext);
}

export function SignalRProvider({ children }) {
  const [connection, setConnection] = useState(null);

  const startConnection = async (token) => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7260/taskItHub", {
        accessTokenFactory: async () => token,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    try {
      await newConnection.start();
      console.log("SignalR povezan.");
      setConnection(newConnection);
    } catch (error) {
      console.error("Neuspelo startovanje konekcije:", error);
    }
  };

  const stopConnection = async () => {
    if (connection) {
      await connection.stop();
      console.log("SignalR konekcija je zaustavljena.");
      setConnection(null);
    }
  };

  const refreshConnection = async (token) => {
    try {
      await stopConnection();
      await startConnection(token);
    } catch (error) {
      console.error("Neuspelo osvežavanje konekcije:", error);
    }
  };

  return (
    <SignalRContext.Provider
      value={{ connection, startConnection, refreshConnection, stopConnection }}
    >
      {children}
    </SignalRContext.Provider>
  );
}
