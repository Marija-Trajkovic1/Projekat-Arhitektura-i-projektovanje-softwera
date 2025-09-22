import { useLocation, useNavigate } from "react-router-dom";
import { useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { useSignalR } from "../../context/SignalRContext";

const NavBar = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const { user, role, token, logout, loading } = useAuth();
  const {stopConnection} = useSignalR();
  const navigate = useNavigate();
  const location = useLocation();

  const handleLogout = () => {
    logout();
    stopConnection(token);
    navigate("/");
  };

  const showHomeButton =
    location.pathname === "/login" || location.pathname === "/register";

  if (loading) {
    return <div className="bg-gray-800 text-white p-4">Loading...</div>;
  }

  return (
    <nav className="bg-gray-800 text-white p-4 flex justify-between items-center">
        <div className="text-xl font-bold text-blue-600">TaskIT</div>

      <div className="flex items-center">
        <div className="md:hidden">
          <button
            onClick={() => setIsMenuOpen(!isMenuOpen)}
            className="text-white focus:outline-none"
          >
            <svg
              className="w-6 h-6"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M4 6h16M4 12h16M4 18h16"
              ></path>
            </svg>
          </button>
        </div>

        <div
          className={`${
            isMenuOpen ? "block" : "hidden"
          } md:flex md:items-center md:space-x-4 absolute md:static top-16 left-0 w-full md:w-auto bg-gray-800 md:bg-transparent p-4 md:p-0 z-10 md:z-auto`}
        >
          {showHomeButton && (
            <button
              onClick={() => navigate("/")}
              className="hover:underline block md:inline-block text-white py-2 md:py-0"
            >
              Početna stranica
            </button>
          )}

          {!user && !showHomeButton && (
            <>
              <button
                onClick={() => navigate("/login")}
                className="hover:underline block md:inline-block text-white py-2 md:py-0"
              >
                Prijavi se
              </button>
              <button
                onClick={() => navigate("/register")}
                className="hover:underline block md:inline-block text-white py-2 md:py-0"
              >
                Registruj se
              </button>
            </>
          )}

          {user && (
            <>
              <button
                onClick={() => navigate("/profile")}
                className="hover:underline block md:inline-block text-white py-2 md:py-0"
              >
                Profil
              </button>

              {role?.toUpperCase() === "WORKER" && (
                <>
                  <button
                    onClick={() => navigate("/review-jobs")}
                    className="hover:underline block md:inline-block text-white py-2 md:py-0"
                  >
                    Pregled oglasa 
                  </button>
                  <button
                    onClick={() => navigate("/signed-jobs")}
                    className="hover:underline block md:inline-block text-white py-2 md:py-0"
                  >
                    Prijavljeni poslovi
                  </button>
                  <button
                    onClick={() => navigate("/notifications-worker")}
                    className="hover:underline block md:inline-block text-white py-2 md:py-0"
                  >
                    Obaveštenja
                  </button>
                  <button
                    onClick={handleLogout}
                    className="hover:underline text-red-400 block md:inline-block text-white py-2 md:py-0"
                  >
                    Odjavi se
                  </button>
                </>
              )}
              {role?.toUpperCase() === "EMPLOYER" && (
                <>
                  <button
                    onClick={() => navigate("/manage-job-ads")}
                    className="hover:underline block md:inline-block text-white py-2 md:py-0"
                  >
                    Upravljaj svojim oglasima
                  </button>
                  <button
                    onClick={() => navigate("/notifications-employer")}
                    className="hover:underline block md:inline-block text-white py-2 md:py-0"
                  >
                    Obaveštenja
                  </button>
                  <button
                    onClick={handleLogout}
                    className="hover:underline text-red-400 block md:inline-block text-white py-2 md:py-0"
                  >
                    Odjavi se
                  </button>
                </>
              )}
            </>
          )}
        </div>
      </div>
    </nav>
  );
};

export default NavBar;
