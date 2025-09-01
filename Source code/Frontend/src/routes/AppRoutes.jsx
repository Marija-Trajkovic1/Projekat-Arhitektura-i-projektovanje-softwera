import ProtectedRoute from "../components/ProtectedRoute";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "../pages/Home/Home";
import Login from "../pages/Auth/Login";
import Register from "../pages/Auth/Register";
import Profile from "../pages/Profile/Profile";
import ReviewJobs from "../pages/WorkerPages/ReviewJobs";
import SignedJobs from "../pages/WorkerPages/SignedJobs";
import NotificationsWorker from "../pages/Notifications/NotificationsWorker";
import BeEmployer from "../pages/WorkerPages/BeEmployer";
import PostedJobs from "../pages/EmployerPages/PostedJobs";
import NotificationsEmployer from "../pages/Notifications/NotificationsEmployer";
import CreateJob from "../pages/EmployerPages/CreateJob";

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
        <Route path="/be-employer" element={<ProtectedRoute><BeEmployer /></ProtectedRoute>} />
        <Route path="/posted-jobs" element={<ProtectedRoute><PostedJobs /></ProtectedRoute>} />
        <Route path="/notifications-employer" element={<ProtectedRoute><NotificationsEmployer/></ProtectedRoute>} />
        <Route path="/create-job" element={<ProtectedRoute><CreateJob /></ProtectedRoute>} />
        <Route path="/be-worker" element={<ProtectedRoute><BeWorker/></ProtectedRoute>} />
      </Routes>
  
  );
};

export default AppRoutes;
