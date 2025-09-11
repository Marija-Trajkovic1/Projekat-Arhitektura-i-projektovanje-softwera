import {Navigate} from "react-router-dom"
import { useAuth } from "../context/AuthContext";

const ProtectedRoute = ({children})=>{
    const {token, role, loading} = useAuth();
    
    

    if(loading) return <div>Učitavanje...</div>;
    
    if(!token || !["WORKER", "EMPLOYER"].includes(role)){
        if(token){alert("Nemate potrebne dozvole!");}
        return <Navigate to="/" replace />;
    }
    return children;
}

export default ProtectedRoute;