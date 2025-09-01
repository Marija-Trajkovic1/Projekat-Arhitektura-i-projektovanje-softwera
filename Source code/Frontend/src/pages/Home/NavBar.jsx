import { useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

const NavBar = () => {
  const { user, role, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  console.log("user: ", user, " role:", role);
  const handleLogout = () => {
    logout();
    navigate("/");
  };

  const showHomeButton =
    location.pathname === "/login" || location.pathname === "/register";

  return (
    <nav className="bg-gray-800 text-white p-4 flex justify-between items-center">
      <div className="text-xl font-bold text-blue-600">TaskIT</div>

      <div className="flex gap-4">
        {showHomeButton && (
          <>
          <button onClick={()=>navigate("/")} className="hover:underline">
            Home
          </button>
          </>
        )}
        
        {!user && !showHomeButton && (
          <>
            <button
              onClick={() => navigate("/login")}
              className="hover:underline"
            >
              Login
            </button>
            <button
              onClick={() => navigate("/register")}
              className="hover:underline"
            >
              Register
            </button>
          </>
        )}

        {user && (
          <>
            <button onClick={() => navigate("/profile")} className="hover:underline"> Profile </button>
            {role?.toUpperCase() === "WORKER" && (
              <>
                <button onClick={() => navigate("/review-jobs")} className="hover:underline">Jobs Review</button>
                <button onClick={() => navigate("/signed-jobs")} className="hover:underline">My Jobs</button>
                <button onClick={() => navigate("/notifications-worker")} className="hover:underline">Notifications</button>
                <button onClick={handleLogout} className="hover:underline text-red-400">Logout</button>
            </>
            )}
            {role?.toUpperCase() === "EMPLOYER" && (
              <>
                <button onClick={() => navigate("/posted-jobs")} className="hover:underline">Posted Jobs</button>
                <button onClick={() => navigate("/create-job")} className="hover:underline">Create Job</button>
                <button onClick={() => navigate("/notifications-employer")}className="hover:underline">Notifications</button>
                <button onClick={handleLogout} className="hover:underline text-red-400">Logout</button>
              </>
            )}
            </>
        )}
      
      </div>
    </nav>
  );
};

export default NavBar;
