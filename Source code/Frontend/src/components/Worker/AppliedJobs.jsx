import { useState, useEffect } from "react";
import { useAuth } from "../../context/AuthContext";
import axios from "axios";

const AppliedJobs = () => {
  const { token } = useAuth();
  const [appliedJobs, setAppliedJobs] = useState([]);
  const [loading, setLoading] = useState(false);
  const [loadingIds, setLoadingIds] = useState([]);

  const getAppliedJobs = async () => {
    try {
      setLoading(true);
      const response = await axios.get(
        `https://localhost:7260/JobApplication/GetAppliedJobsForWorker`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setAppliedJobs(response.data || []);
    } catch (error) {
      console.error(
        "Neuspelo pribavljanje prijavljenih poslova;",
        error.response?.data || error.response
      );
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    getAppliedJobs();
  }, [token]);

  const declineApplication = async (jobId) => {
    try {
      setLoadingIds((prev) => [...prev, jobId]);
      await axios.put(
        `https://localhost:7260/JobApplication/DeclineApplicationForJobByWorker/${jobId}`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      );

      await getAppliedJobs();
      alert("Prijava je otkazana!");
    } catch (error) {
      console.error(
        "Neuspešno otkazivanje prijave!",
        error.response?.data || error.response
      );
    } finally {
      setLoadingIds((prev) => prev.filter((id) => id !== jobId));
    }
  };

  return (
    <div className="mb-8">
      <h2 className="text-xl font-semibold mb-4 text-gray-800">
        Vaši prijavljeni poslovi
      </h2>

      {(loading) && <p className="text-gray-500">Učitavanje...</p>}
      {(appliedJobs.length === 0) && <p className="text-gray-500">Nema prijavljenih oglasa.</p>}

      <div className="flex gap-4 overflow-x-auto pb-4">
        {appliedJobs.map((job) => {
          const isLoading = loadingIds.includes(job.id);

          return (
            <div
              key={job.id}
              className="min-w-[280px] max-w-xs border rounded-lg p-4 shadow bg-white hover:shadow-lg transition-shadow"
            >
              <h3 className="font-bold text-lg mb-2">{job.title}</h3>
              <p className="text-sm text-gray-600 mb-2">
                {job.shortDescription}
              </p>
              <p className="mt-2 font-semibold text-green-600">
                Plata: {job.jobSalary} RSD/satu
              </p>
              <p className="text-sm text-gray-700">
                {job.homeNumber}, {job.street}, {job.city}
              </p>
              <p className="text-sm text-gray-500 mt-1">
                Datum obavljanja: {job.dateOfExecution}
              </p>

              <button
                onClick={() => declineApplication(job.id)}
                disabled={isLoading}
                className={`mt-3 w-full px-4 py-2 rounded text-white font-semibold transition-colors ${
                  isLoading
                    ? "bg-gray-400 cursor-not-allowed"
                    : "bg-red-500 hover:bg-red-600"
                }`}
              >
                {isLoading ? "Učitavanje" : "Otkaži prijavu"}
              </button>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default AppliedJobs;
