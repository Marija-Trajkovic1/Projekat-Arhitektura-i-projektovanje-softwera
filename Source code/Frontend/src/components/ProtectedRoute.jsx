import {Navigate} from "react-router-dom"


const ProtectedRoute = ({children, role})=>{
    const {token, role: userRole} = useAuth();

    if(!token){
        return <Navigate to="/login" replace />;
    }

    if(role && userRole != role){
        return <Navigate to="/" replace />;
    }

    return children;
}

export default ProtectedRoute;