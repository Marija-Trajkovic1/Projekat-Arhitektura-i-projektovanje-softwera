import { useAuth } from "../../context/AuthContext";
import axios from "axios";
import { useState, useEffect } from "react";
import qs from "qs";
import { useSignalR } from "../../context/SignalRContext";

const JobsByType = ({ jobTypes }) => {
  const { token } = useAuth();
  const{connection} = useSignalR();
  const [jobAds, setJobAds] = useState([]);
  const [loading, setLoading] = useState(false);
  const [loadingIds, setLoadingIds] = useState([]);

  const getJobAds = async () => {
    if (!jobTypes || jobTypes.length === 0) {
      setJobAds([]);
      return;
    }

    try {
      setLoading(true);
      const response = await axios.get(
        `https://localhost:7260/JobAdvertisement/GetFilteredJobAdvertisements`,
        {
          headers: { Authorization: `Bearer ${token}` },
          params: {
            filterBy: "jobtypelist",
            jobTypes: jobTypes.filter((type) => type),
            excludeOwn: true,
            page: 1,
            pageSize: 10,
          },
          paramsSerializer: (params) =>
            qs.stringify(params, { arrayFormat: "repeat" }),
        }
      );
      setJobAds(response.data || []);
    } catch (error) {
      console.error(
        "Neuspelo pribavljanje oglasa prema tipu",
        error.response?.data || error.response
      );
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    getJobAds();
  }, [jobTypes, token]);

  useEffect(()=>{
    if(!connection) return;
    const handleRefreshJobs=()=>{
      getJobAds();
    }
    connection.on("WorkerApplication", handleRefreshJobs);
    connection.on("ApplicationRejected", handleRefreshJobs);
    connection.on("NewJobPosted", handleRefreshJobs);
    connection.on("JobDeleted", handleRefreshJobs);
    return()=>{
      connection.off("WorkerApplication", handleRefreshJobs);
      connection.off("ApplicationRejected", handleRefreshJobs);
      connection.off("NewJobPosted", handleRefreshJobs);
      connection.off("JobDeleted", handleRefreshJobs);
    }
  },[connection]);

  const applyToJob = async (jobId) => {
    try {
      setLoadingIds((prev) => [...prev, jobId]);
      await axios.put(
        `https://localhost:7260/JobApplication/SendApplayForJob/${jobId}`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      );
      alert("Uspešno ste se prijavili na oglas!");
      await getJobAds();
    } catch (error) {
      console.error(
        "Neuspelo prijavljivanje na oglas!",
        error.response?.data || error.response
      );
    } finally {
      setLoadingIds((prev) => prev.filter((id) => id !== jobId));
    }
  };

  if (!jobTypes || jobTypes.length === 0) {
    return <p className="text-gray-500">Niste odabrali nijedan tip posla.</p>;
  }

  if (jobAds.length === 0) {
    return (
      <p className="text-gray-500">
        Izaberite tipove da biste videli dostupne oglase!
      </p>
    );
  }
  return (
    <div className="flex gap-4 overflow-x-auto pb-4">
      {jobAds.map((job) => {
        const isLoading = loadingIds.includes(job.id);
        return (
          <div
            key={job.id}
            className="min-w-[280px] max-w-xs border rounded-lg p-4 shadow bg-white hover:shadow-lg transition-shadow"
          >
            <h3 className="font-blod text-lg mb-2">{job.title}</h3>
            <p className="text-sm text-gray-600 mb-2">{job.shortDescription}</p>
            <p className="mt-2 font-semibold text-green-600">
              Plata: {job.jobSalary} RSD/satu.
            </p>
            <p className="text-sm text-gray-700">
              {job.homeNumber}, {job.street}, {job.city}
            </p>
            <p className="text-sm text-gray-500 mt-1">
              Datum obavljanja: {job.dateOfExecution}
            </p>

            <button
              onClick={() => applyToJob(job.id)}
              disabled={isLoading}
              className={`mt-3 w-full px-4 py-2 rounded text-white font-semibold transition-colors ${
                isLoading
                  ? "bg-gray-400 cursor-not-allowed"
                  : "bg-blue-500 hover:bg-blue-600"
              }`}
            >
              {isLoading ? "Učitavanje" : "Prijavi se"}
            </button>
          </div>
        );
      })}
    </div>
  );
};

export default JobsByType;
