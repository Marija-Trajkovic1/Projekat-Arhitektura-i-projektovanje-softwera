import ProtectedRoute from "../components/ProtectedRoute";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "../pages/Home/Home";
import Login from "../pages/Auth/Login";
import Register from "../pages/Auth/Register";
import Profile from "../pages/Profile/Profile";
import ReviewJobs from "../pages/WorkerPages/ReviewJobs";
import SignedJobs from "../pages/WorkerPages/SignedJobs";
import NotificationsWorker from "../pages/Notifications/NotificationsWorker";
import NotificationsEmployer from "../pages/Notifications/NotificationsEmployer";
import ManageJobAds from "../pages/EmployerPages/ManageJobAds";

const AppRoutes = () => {
  return (
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />

        <Route path="/profile" element={<ProtectedRoute><Profile /></ProtectedRoute>} />

        <Route path="/review-jobs" element={<ProtectedRoute><ReviewJobs /></ProtectedRoute>} />
        <Route path="/signed-jobs" element={<ProtectedRoute><SignedJobs /></ProtectedRoute>} />
        <Route path="/notifications-worker" element={<ProtectedRoute><NotificationsWorker/></ProtectedRoute>} />
        
        <Route path="/manage-job-ads" element={<ProtectedRoute><ManageJobAds /></ProtectedRoute>} />
        <Route path="/notifications-employer" element={<ProtectedRoute><NotificationsEmployer/></ProtectedRoute>} />
        
      </Routes>
  
  );
};

export default AppRoutes;
