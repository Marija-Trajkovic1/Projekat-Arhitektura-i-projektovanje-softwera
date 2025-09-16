import AppliedJobs from "../../components/Worker/AppliedJobs";
import AcceptedJobs from "../../components/Worker/AcceptedJobs";

const SignedJobs=()=>{
    return(<div className="p-6 max-w-3x1 mx-auto">
        <h1 className="text-3xl font-bold mb-4">Pregledajte prijavljene i prihvaćene poslove</h1>
        <div className="mb-4">
            <AppliedJobs />
            <AcceptedJobs /> 
        </div>
    </div>); 
}

export default SignedJobs;