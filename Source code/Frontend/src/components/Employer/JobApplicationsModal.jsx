import { useState } from "react";
import axios from "axios";

const JobApplicationsModal = ({
  isOpen,
  onClose,
  applications,
  jobTitle,
  token,
}) => {
  if (!isOpen) return null;
  const [statusMap, setStatusMap] = useState({});
  

  const handleAcceptApplication = async (applicationId) => {
    try {
      await axios.put(
        `https://localhost:7260/JobApplication/AcceptApplicationForJob/${applicationId}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setStatusMap((prev)=>({...prev, [applicationId]:"accepted"}));
    } catch (error) {
      console.error(
        "Neuspelo prihvatanje prijave korisnika za oglas!",
        error.response?.data || error.response
      );
    }
  };

  const handleRejectApplication = async (applicationId, workerId) => {
    try {
      await axios.put(
        `https://localhost:7260/JobApplication/DeclineApplicationForJobByEmployer/${applicationId}`,
        {workerId},
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setStatusMap((prev)=>({...prev, [applicationId]:"declined"}))
    } catch (error) {
      console.error(
        "Neuspelo odbijanje prijave korisnika za oglas!",
        error.response?.data || error.response
      );
    }
  };

  return (
    <div className="fixed inset-0 flex justify-center items-center z-50 pointer-events-none">
      <div className="bg-white bg-opacity-20 rounded-lg shadow-lg w-80 max-h-[60vh] overflow-y-auto p-4 pointer-events-auto">
        <h2 className="text-xl font-bold mb-4">Prijave za oglas: {jobTitle}</h2>

        {applications.length === 0 ? (
          <p className="text-gray-600">Jos uvek nema prijava za ovaj oglas.</p>
        ) : (
          applications.map((app) => {
            const status =statusMap[app.id];
            return(
            <div
              key={app.id}
              className="border rounded-lg p-3 mb-2 flex justify-between items-center shadow-sm"
            >
              <p className="text-sm">
                <strong>{app.workerUserName}</strong> se prijavio za vaš oglas.
              </p>
              <div className="flex gap-2">
                <button
                  onClick={() => handleAcceptApplication(app.id)}
                  className={`px-2 py-1 rounded text-xs text-white ${
                      status === "accepted"
                        ? "bg-green-400 cursor-not-allowed"
                        : "bg-green-500 hover:bg-green-600"
                    }`}
                    disabled={status==="accepted"}
                >
                  {status==="accepted" ? "Prihvaceno" : "Prihvati"}
                </button>
                <button
                  onClick={() => onReject(app.id, app.workerId)}
                  className={`px-2 py-1 rounded text-xs text-white ${
                      status === "declined"
                        ? "bg-red-400 cursor-not-allowed"
                        : "bg-red-500 hover:bg-red-600"
                    }`}
                    disabled={status === "declined"}
                >
                  {status==="declined" ? "Odbijeno" : "Odbij"}
                </button>
              </div>
            </div>
          )}))}
        
        <button
          onClick={onClose}
          className="mt-4 w-full bg-gray-500 text-white py-2 rounded hover:bg-gray-600"
        >
          Zatvori
        </button>
      </div>
    </div>
  );
};

export default JobApplicationsModal;
