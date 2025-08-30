import { useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

const NavBar=()=>{
    const {user, role, logout}=useAuth();
    const navigate=useNavigate();
    const location=useLocation();

    const handleLogout=()=>{
        logout();
        navigate("/");
    }

    const showHomeButton = location.pathname === "/login" || location.pathname === "/register";

    return(
        <nav className="bg-gray-800 text-white p-4 flex justify-between items-center">
            <div className="text-xl font-bold text-blue-600">TaskIT</div>

            <div className="flex gap-4">
                {showHomeButton && (
                    <button onClick={()=>navigate("/")} className="hover:underline">Home</button>
                )}

                {!user && !showHomeButton && (
                    <>
                        <button onClick={()=>navigate("/login")} className="hover:underline">Login</button>
                        <button onClick={()=>navigate("/register")} className="hover:underline">Register</button>
                    </>
                )}

                {user && role ==="Worker" &&(
                    <>
                        <button onClick={()=>navigate("/profile")} className="hover:underline">Profile</button>
                        <button onClick={()=>navigate("/review-jobs")} className="hover:underline">Jobs Review</button>
                        <button onClick={()=>navigate("/signed-jobs")} className="hover:underline">My Jobs</button>
                        <button onClick={()=>navigate("/notifications")} className="hover:underline">Notifications</button>
                        <button onClick={()=>navigate("/be-employer")} className="hover:underline">Be an Employer</button>
                        <button onClick={handleLogout} className="hover:underline">Logout</button>
                    </>
                )}

                {user && role ==="Employer" &&(
                    <>
                        <button onClick={()=>navigate("/profile")} className="hover:underline">Profile</button>
                        <button onClick={()=>navigate("/posted-jobs")} className="hover:underline">Posted Jobs</button>
                        <button onClick={()=>navigate("/notifications")} className="hover:underline">Notifications</button>
                        <button onClick={()=>navigate("/create-job")} className="hover:underline">Create Job</button>
                        <button onClick={handleLogout} className="hover:underline">Logout</button>
                    </>
                )}
            </div>
        </nav>
    )
}

export default NavBar;