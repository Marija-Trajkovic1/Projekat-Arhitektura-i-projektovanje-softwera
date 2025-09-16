import JobTypeFollowing from "../../components/Worker/JobTypeFollowing";
import EmployerFollowing from "../../components/Worker/EmployerFollowing";
import AvailableJobs from "../../components/Worker/AvailableJobs";
import { useState } from "react";

const ReviewJobs=()=>{
    const[jobTypes, setJobTypes]=useState([]);
    const [employerIds, setEmployerIds]=useState([]);

    return(<div className="p-6 max-w-3x1 mx-auto">
        <h1 className="text-3xl font-bold mb-4">Pregledajte dostupne oglase</h1>

        <div className="mb-4">
            <JobTypeFollowing onJobTypeChange={setJobTypes}/>
            <EmployerFollowing onEmployersChange={setEmployerIds}/>
            
           <AvailableJobs jobTypes={jobTypes} employerIds={employerIds} />
        </div>

    </div>);
}   

export default ReviewJobs;
