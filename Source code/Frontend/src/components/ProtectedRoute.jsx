import {Navigate} from "react-router-dom"
import { useAuth } from "../context/AuthContext";

const ProtectedRoute = ({children})=>{
    const {token, role, loading} = useAuth();
    console.log(token, role, loading);
    

    if(loading) return <div>Loading...</div>;
    
    if(!token || !["WORKER", "EMPLOYER"].includes(role)){
        return <Navigate to="/" replace />;
    }
    return children;
}

export default ProtectedRoute;