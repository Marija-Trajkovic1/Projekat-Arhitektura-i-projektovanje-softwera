import { createContext, useState, useEffect, useContext } from "react";
import axios from "axios";
import {signalRService } from "../services/SignalRService";

const AuthContext = createContext({});

export default function AuthProvider ({ children }) {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(sessionStorage.getItem("token") || null);
  const [role, setRole] = useState(sessionStorage.getItem("role") || null);
  const [loading, setLoading] = useState(true);
  const [isLoggingOut, setIsLoggingOut] = useState(false);

  const updateAuth = ({ token, user, role }) => {
    setToken(token);
    setUser(user);
    setRole(role);
    sessionStorage.setItem('token', token);
    sessionStorage.setItem('user', JSON.stringify(user));
    sessionStorage.setItem('role', role);
  }

  useEffect(() => {
    const validateToken = async () => {
      if(isLoggingOut) return;
    const storedUser = sessionStorage.getItem("user");
    const storedToken = sessionStorage.getItem("token");
    const storedRole = sessionStorage.getItem("role");

    if (storedToken && storedUser && storedRole) {
      try {
        await axios.get("https://localhost:7260/User/FindUserForProfile", {
            headers: { Authorization: `Bearer ${storedToken}` },
        });
        setToken(storedToken);
        setUser(JSON.parse(storedUser));
        setRole(storedRole.toUpperCase());

        await signalRService.startConnection(storedToken);
      } catch (e) {
        if (e.response?.status === 401) {
            console.error("Nevažeći token, odjavljujem se:", e);
            logout();
          } else {
            console.error("Greška prilikom validacije tokena:", e);
          }
      }
    }
    setLoading(false);
  };
  validateToken();
  }, [isLoggingOut]);

  useEffect(()=>{
    if(token && !isLoggingOut){
      signalRService.startConnection(token).catch((error)=>{
        console.error('Greška pri pokretanju SignalR konekcije:', error);
      });
    }else{
      signalRService.stopConnection().catch((error)=>{
        console.error("Greška pri yaustavljanju SignalR konekcije:", error);
      });
    }

    return ()=>{
      signalRService.stopConnection().catch((error)=>{
        console.error("Greška pri zaustavljanju SignalR konekcije:", error);
      });
    };
  }, [token, isLoggingOut]);

  const login = (userData, jwtToken, userRole) => {
    setUser(userData);
    setToken(jwtToken);
    setRole(userRole.toUpperCase());

    sessionStorage.setItem("token", jwtToken);
    sessionStorage.setItem("user", JSON.stringify(userData));
    sessionStorage.setItem("role", userRole.toUpperCase());
  };

  const logout = () => {
    setIsLoggingOut(true);
    signalRService.stopConnection()
    .then(()=>{
      setUser(null);
      setToken(null);
      setRole(null);
      sessionStorage.removeItem("user");
      sessionStorage.removeItem("token");
      sessionStorage.removeItem("role");
      setIsLoggingOut(false);
    }).catch((error)=>{
      console.error("Greška pri zaustavljanju SignakR konekcije:", error);
      setUser(null);
      setToken(null);
      setRole(null);
      sessionStorage.removeItem("user");
      sessionStorage.removeItem("role");
      sessionStorage.removeItem("token");
      setIsLoggingOut(false);
    });
  };

  return (
    <AuthContext.Provider value={{ user, token, role, login, logout, loading, updateAuth }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
