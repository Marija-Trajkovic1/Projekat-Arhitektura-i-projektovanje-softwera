import JobTypeFollowing from "../../components/Worker/JobTypeFollowing";
import EmployerFollowing from "../../components/Worker/EmployerFollowing";
import AvailableJobs from "../../components/Worker/AvailableJobs";

const ReviewJobs=()=>{

    return(<div className="p-6 max-w-3x1 mx-auto">
        <h1 className="text-3xl font-bold mb-4">Pregledajte dostupne oglase</h1>

        <div className="mb-4">
            <JobTypeFollowing />
            <EmployerFollowing />
            
           <AvailableJobs />
        </div>

    </div>);
}   

export default ReviewJobs;
