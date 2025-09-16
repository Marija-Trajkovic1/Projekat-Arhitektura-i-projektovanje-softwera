import { useState, useEffect } from "react";
import { useAuth } from "../../context/AuthContext";
import axios from "axios";

const AppliedJobs = () => {
  const { token } = useAuth();
  const [appliedJobs, setAppliedJobs] = useState([]);
  const [loading, setLoading] = useState(false);
  const [loadingIds, setLoadingIds] = useState([]);

  useEffect(() => {
    const getAppliedJobs = async () => {
      try {
        setLoading(true);
        const response = await axios.get(
          `https://localhost:7260/JobApplication/GetAppliedJobsForWorker`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        console.log("API response:", response.data);
        setAppliedJobs(response.data || []);
      } catch (error) {
        console.log(
          "Neuspelo pribavljanje prijavljenih poslova;",
          error.response?.data || error.response
        );
      } finally {
        setLoading(false);
      }
    };

    getAppliedJobs();
  }, [token]);

  const applyToJob = async (jobId) => {
    try {
      setLoadingIds((prev) => [...prev, jobId]);
      const response = await axios.put(
        `https://localhost:7260/JobApplication/SendApplayForJob/${jobId}`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      );
      console.log("Apply response:", response.data);
      setAppliedJobs((prev) =>
        prev.map((job) =>
          job.id === jobId ? { ...job, isAvailable: false } : job
        )
      );
      alert("Uspešno ste se prijavili na oglas!");
    } catch (error) {
      console.error(
        "Neuspesna prijava na oglas",
        error.response?.data || error.response
      );
    } finally {
      setLoadingIds((prev) => prev.filter((id) => id !== jobId));
    }
  };

  const declineApplication = async (jobId) => {
    try {
      setLoadingIds((prev) => [...prev, jobId]);
      const response = await axios.put(
        `https://localhost:7260/JobApplication/DeclineApplicationForJobByWorker/${jobId}`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      );

      setAppliedJobs((prev) =>
        prev.map((job) =>
          job.id === jobId ? { ...job, isAvailable: true } : job
        )
      );
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

  if (loading) {
    return <p className="text-gray-500">Učitavanje...</p>;
  }

  if (appliedJobs.length === 0) {
    return <p className="text-gray-500">Nema prijavljenih oglasa.</p>;
  }

  return (
    <div className="mb-8">
        <h2 className="text-xl font-semibold mb-4 text-gray-800">Vaši prijavljeni poslovi</h2>
        <div className="flex gap-4 overflow-x-auto pb-4" >
      {appliedJobs.map((job) => {
        const isLoading = loadingIds.includes(job.id);

        return (
          <div
            key={job.id}
            className="min-w-[280px] max-w-xs border rounded-lg p-4 shadow bg-white hover:shadow-lg transition-shadow"
          >
            <h3 className="font-bold text-lg mb-2">{job.title}</h3>
            <p className="text-sm text-gray-600 mb-2">{job.shortDescription}</p>
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
              onClick={() =>
                job.isAvailable
                  ? applyToJob(job.id)
                  : declineApplication(job.id)
              }
              disabled={isLoading}
              className={`mt-3 w-full px-4 py-2 rounded text-white font-semibold transition-colors ${
                isLoading
                  ? "bg-gray-400 cursor-not-allowed"
                  : job.isAvailable
                  ? "bg-blue-500 hover:bg-blue-600"
                  : "bg-red-500 hover:bg-red-600"
              }`}
            >
              {isLoading
                ? "Učitavanje"
                : job.isAvailable
                ? "Prijavi se"
                : "Otkaži prijavu"}
            </button>
          </div>
        );
      })}
    </div>
    </div>
  );
};

export default AppliedJobs;
