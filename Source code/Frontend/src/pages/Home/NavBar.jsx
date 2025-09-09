import { useLocation, useNavigate } from "react-router-dom";
import { useState } from "react";
import { useAuth } from "../../context/AuthContext";

const NavBar = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const { user, role, logout, loading } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  console.log("user: ", user, " role:", role);
  const handleLogout = () => {
    logout();
    navigate("/");
  };

  const showHomeButton =
    location.pathname === "/login" || location.pathname === "/register";

  if (loading) {
    return <div className="bg-gray-800 text-white p-4">Loading...</div>;
  }

  return (
    <nav className="bg-gray-800 text-white p-4 flex justify-between items-center">
      <div className="flex justify-between items-center">
        <div className="text-xl font-bold text-blue-600">TaskIT</div>

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
          } md:flex md:items-center md:space-x-4`}
        >
          {showHomeButton && (
            <button
              onClick={() => navigate("/")}
              className="hover:underline block md:inline-block"
            >
              Home
            </button>
          )}

          {!user && !showHomeButton && (
            <>
              <button
                onClick={() => navigate("/login")}
                className="hover:underline block md:inline-block"
              >
                Login
              </button>
              <button
                onClick={() => navigate("/register")}
                className="hover:underline block md:inline-block"
              >
                Register
              </button>
            </>
          )}

          {user && (
            <>
              <button
                onClick={() => navigate("/profile")}
                className="hover:underline block md:inline-block"
              >
                Profile
              </button>

              {role?.toUpperCase() === "WORKER" && (
                <>
                  <button
                    onClick={() => navigate("/review-jobs")}
                    className="hover:underline block md:inline-block"
                  >
                    Jobs Review
                  </button>
                  <button
                    onClick={() => navigate("/signed-jobs")}
                    className="hover:underline block md:inline-block"
                  >
                    My Jobs
                  </button>
                  <button
                    onClick={() => navigate("/notifications-worker")}
                    className="hover:underline block md:inline-block"
                  >
                    Notifications
                  </button>
                  <button
                    onClick={handleLogout}
                    className="hover:underline text-red-400 block md:inline-block"
                  >
                    Logout
                  </button>
                </>
              )}
              {role?.toUpperCase() === "EMPLOYER" && (
                <>
                  <button
                    onClick={() => navigate("/posted-jobs")}
                    className="hover:underline block md:inline-block"
                  >
                    Posted Jobs
                  </button>
                  <button
                    onClick={() => navigate("/create-job")}
                    className="hover:underline block md:inline-block"
                  >
                    Create Job
                  </button>
                  <button
                    onClick={() => navigate("/notifications-employer")}
                    className="hover:underline block md:inline-block"
                  >
                    Notifications
                  </button>
                  <button
                    onClick={handleLogout}
                    className="hover:underline text-red-400 block md:inline-block"
                  >
                    Logout
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
