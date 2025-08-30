import { useNavigate } from "react-router-dom";

const Landing=()=>{
    const navigate=useNavigate();

    return(
        <div className="flex flex-col items-center justify-center text-center min-h-screen bg-gray-100 px-4">
           <h1 className="text-4xl font-bold mb-6">Dobrodošli na TaskIT platformu!</h1> 
            <p className="text-lg text-gray-700 mb-8"> 
                Prijavite se ili registrujte kako biste pregledali ili postavili oglase!
            </p>
            <div className="flex gap-4">
                <button
                onClick={()=>navigate("/login")}
                className="px-6 py-3 bg-blue-500 text-white rounded hover:bg-blue-600 transition">
                    Login
                </button>
                <button
                    onClick={()=>navigate("/register")}
                    className="px-6 py-3 bg-green-500 text-white rounded hover:bg-green-600 transition">
                    Register
                </button>
            </div>
        </div>
    )
}

export default Landing;