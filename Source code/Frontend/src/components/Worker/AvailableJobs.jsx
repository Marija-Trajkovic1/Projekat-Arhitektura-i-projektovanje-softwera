import { useState } from "react";
import JobsByType from "./JobsByType";
import JobsByEmployer from "./JobsByEmployer";

const AvailableJobs=({jobTypes, employerIds})=>{
    const [activeTab, setActiveTab] = useState("type");

    return(
        <div>
            <h2 className="text-lg font-bold mb-4">Izaberite filter za prikaz oglasa:</h2>
            
            <div className="mt-6">
                <div className="flex border-v mb-4">
                    <button
                    onClick={()=>setActiveTab("type")}
                    className={`px-4 py-2 font-semibold ${activeTab==="type"? "border-b-2 border-blue-500 text-blue-500":"text-gray-600 hover:text-blue-500"}`}
                    >
                        Tip posla
                    </button>

                    <button
                    onClick={()=>setActiveTab("employer")}
                    className={`px-4 py-2 font-semibold ${activeTab==="employer" ? "border-b-2 border-blue-500 text-blue-500": "text-gray-600 hover:text-blue-500"}`}>
                        Poslodavci
                    </button>
                </div>
                
                {activeTab==="type" ? (
                    <JobsByType jobTypes={jobTypes} />
                ):(
                    <JobsByEmployer employerIds={employerIds} />
                )}
                

            </div>
        </div>
    )

}

export default AvailableJobs;