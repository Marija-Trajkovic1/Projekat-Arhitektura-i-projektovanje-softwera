import { createContext, useState, useEffect, useContext } from "react";
import axios from "axios";

const AuthContext = createContext({});

export default function AuthProvider ({ children }) {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(localStorage.getItem("token") || null);
  const [role, setRole] = useState(localStorage.getItem("role") || null);
  const [loading, setLoading] = useState(true);
  const [isLoggingOut, setIsLoggingOut] = useState(false);

  const updateAuth = ({ token, user, role }) => {
    setToken(token);
    setUser(user);
    setRole(role);
    localStorage.setItem('token', token);
    localStorage.setItem('user', JSON.stringify(user));
    localStorage.setItem('role', role);
  }

  useEffect(() => {
    const validateToken = async () => {
      if(isLoggingOut) return;
    const storedUser = localStorage.getItem("user");
    const storedToken = localStorage.getItem("token");
    const storedRole = localStorage.getItem("role");

    if (storedToken && storedUser && storedRole) {
      try {
        await axios.get("https://localhost:7260/User/FindUserForProfile", {
            headers: { Authorization: `Bearer ${storedToken}` },
        });
        setToken(storedToken);
        setUser(JSON.parse(storedUser));
        setRole(storedRole.toUpperCase());
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


  const login = (userData, jwtToken, userRole) => {
    setUser(userData);
    setToken(jwtToken);
    setRole(userRole.toUpperCase());

    localStorage.setItem("token", jwtToken);
    localStorage.setItem("user", JSON.stringify(userData));
    localStorage.setItem("role", userRole.toUpperCase());
  };

  const logout = () => {
    setIsLoggingOut(true);
    setUser(null);
    setToken(null);
    setRole(null);

    localStorage.removeItem("user");
    localStorage.removeItem("token");
    localStorage.removeItem("role");
  };

  return (
    <AuthContext.Provider value={{ user, token, role, login, logout, loading, updateAuth }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
