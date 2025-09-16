import { useAuth } from "../../context/AuthContext";
import { useEffect, useState } from "react";
import axios from "axios";
import qs from "qs";

const JobsByEmployer = ({ employerIds }) => {
  const { token } = useAuth();
  const [jobAds, setJobAds] = useState([]);
  const [loadingIds, setLoadingIds] = useState([]);

  useEffect(() => {
    if (!employerIds || employerIds.length === 0) {
      setJobAds([]);
      return;
    }

    const getJobAds = async () => {
      try {
        const response = await axios.get(
          `https://localhost:7260/JobAdvertisement/GetFilteredJobAdvertisements`,
          {
            headers: { Authorization: `Bearer ${token}` },
            params: {
              filterBy: "employerlist",
              employerIds: employerIds.filter((id) => id),
              page: 1,
              pageSize: 10,
            },
            paramsSerializer: (params) =>
              qs.stringify(params, { arrayFormat: "repeat" }),
          }
        );
        console.log("GetFilteredJobAdvertisements response:", response.data);
        setJobAds(response.data || []);
      } catch (error) {
        console.error(
          "Neuspelo pribavljanje oglasa prema poslodavcu",
          error.response?.data || error.response
        );
      }
    };

    getJobAds();
  }, [employerIds, token]);

  const applyToJob = async (jobId) => {
    try {
      setLoadingIds((prev) => [...prev, jobId]);
      const response = await axios.put(
        `https://localhost:7260/JobApplication/SendApplayForJob/${jobId}`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      );
      console.log("Apply response:", response.data);
      setJobAds((prev) =>
        prev.map((job) => 
          job.id === jobId ? {...job, isAvailable:false} : job)
      );
      alert("Uspešno ste se prijavili na oglas!");
    } catch (error) {
      console.error(
        "Neuspelo prijavljivanje na oglas!",
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
      console.log("Decline response:", response.data);
      setJobAds((prev) =>
        prev.map((job) => 
          job.id === jobId ? {...job, isAvailable: true} : job)
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

  if (!employerIds || employerIds.length === 0) {
    return <p className="text-gray-500">Niste odabrali nijednog poslodavca.</p>;
  }

  return (
    <div className="flex gap-4 overflow-x-auto pb-4">
      {jobAds.length === 0 ? (
        <p className="text-gray-500">
          Zapratite poslodavce da biste pregledali oglase!
        </p>
      ) : (
        jobAds.map((job) => {
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
                Plata: {job.jobSalary} RSD/satu.
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
        })
      )}
    </div>
  );
};

export default JobsByEmployer;
