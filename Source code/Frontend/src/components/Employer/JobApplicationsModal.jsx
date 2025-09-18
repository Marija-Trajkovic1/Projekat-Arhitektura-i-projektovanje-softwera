import { useState, useEffect } from "react";
import axios from "axios";
import { useAuth } from "../../context/AuthContext";

const JobApplicationsModal = ({ isOpen, onClose, jobId, jobTitle }) => {
  const { token } = useAuth();
  const [applications, setApplications] = useState({});
  const [loading, setLoading] = useState(false);
  const [loadingIds, setLoadingIds] = useState([]);

  const getApplications = async () => {
    try {
      setLoading(true);
      const response = await axios.get(
        `https://localhost:7260/JobApplication/GetJobApplicationsForJobAdd/${jobId}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setApplications(response.data || []);
    } catch (error) {
      console.error(
        "Neuspešno pribavljanje prijava za oglas",
        error.response?.data || error.response
      );
    } finally {
      setLoading(false);
    }
  };
  useEffect(() => {
    getApplications();
  }, [isOpen, applications]);

  if (!isOpen) return;

  const handleAcceptApplication = async (applicationId) => {
    try {
      setLoadingIds((prev) => [...prev, applicationId]);
      await axios.put(
        `https://localhost:7260/JobApplication/AcceptApplicationForJob/${applicationId}`,
        {},
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      alert("Prihvatili ste prijavu!");
      await getApplications();
    } catch (error) {
      console.error(
        "Neuspelo prihvatanje prijave korisnika za oglas!",
        error.response?.data || error.response
      );
    } finally {
      setLoadingIds((prev) => prev.filter((id)=>id !== applicationId));
    }
  };

  const handleRejectApplication = async (applicationId, workerId) => {
    try {
      setLoadingIds((prev) => [...prev, applicationId]);
      await axios.put(
        `https://localhost:7260/JobApplication/DeclineApplicationForJobByEmployer?jobApplicationId=${applicationId}&workerId=${workerId}`,
        {},
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      alert("Odbili ste prijavu!");
      await getApplications();
    } catch (error) {
      console.error(
        "Neuspelo odbijanje prijave korisnika za oglas!",
        error.response?.data || error.response
      );
    } finally {
      setLoadingIds((prev) => prev.filter((id) => id !== applicationId));
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 flex justify-center items-center z-50 pointer-events-none">
      <div className="bg-white bg-opacity-20 rounded-lg shadow-lg w-80 max-h-[60vh] overflow-y-auto p-4 pointer-events-auto">
        <h2 className="text-xl font-bold mb-4">Prijave za oglas: {jobTitle}</h2>

        {applications.length === 0 ? (
          <p className="text-gray-600">Jos uvek nema prijava za ovaj oglas.</p>
        ) : (
          applications.map((app) => {
            const isLoading = loadingIds.includes(jobId.id);
            return (
              <div
                key={app.id}
                className="border rounded-lg p-3 mb-2 flex justify-between items-center shadow-sm"
              >
                <p className="text-sm">
                  <strong>{app.workerUserName}</strong> se prijavio za vaš
                  oglas.
                </p>
                <div className="flex gap-2">
                  {app.isAccepted ? (
                    <span className="text-greeen-600 font-semibold text-sm">
                      Prihvaćeno
                    </span>
                  ) : (
                    <>
                      <button
                        onClick={() => handleAcceptApplication(app.id)}
                        disabled={isLoading}
                        className="px-2 py--1 rounded text-xs text-white bg-green-500 hover:bg-green-600"
                      >
                        Prihvati
                      </button>

                      <button
                        onClick={() =>
                          handleRejectApplication(app.id, app.workerId)
                        }
                        className={`px-2 py-1 rounded text-xs text-white bg-red-500 hover:bg-red-600"
                        }`}
                      >
                        Odbij
                      </button>
                    </>
                  )}
                </div>
              </div>
            );
          })
        )}

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
