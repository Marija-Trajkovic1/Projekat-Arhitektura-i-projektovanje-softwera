import { useState, useEffect } from "react";
import { useAuth } from "../../context/AuthContext";
import axios from "axios";
import { jobTypes } from "../../constants/JobTypes";

const JobTypeFollowing = ({onJobTypeChange}) => {
  const { token } = useAuth();
  const [followedJobTypes, setFollowedJobTypes] = useState([]);

  useEffect(() => {
    const getFollowedTypes = async () => {
      try {
        const response = await axios.get(
          `https://localhost:7260/WorkerJobTypeFollowing/GetTypesForWorker`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        const types=response.data || [];
        setFollowedJobTypes(response.data || []);
        onJobTypeChange(types);
      } catch (error) {
        console.log(
          "Neuspelo dohvatanje tipova koje pratite!",
          error.response?.data || error.response
        );
      }
    };
    getFollowedTypes();
  }, [token, onJobTypeChange]);

  const clickOnJobType = async (type) => {
    const jobType=encodeURIComponent(type);
    try {
      if (followedJobTypes.includes(type)) {
        await axios.delete(
          `https://localhost:7260/WorkerJobTypeFollowing/DeleteFollowing/${jobType}`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
      } else {
        await axios.post(
          `https://localhost:7260/WorkerJobTypeFollowing/AddNewFollowing/${jobType}`,
          {},
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
      }

      const refreshed = await axios.get(
        `https://localhost:7260/WorkerJobTypeFollowing/GetTypesForWorker`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      const updatedTypes = refreshed.data || [];
      setFollowedJobTypes(refreshed.data || []);
      onJobTypeChange(updatedTypes);
    } catch (error) {
      console.error("Neuspešno osvežavanje liste prećenja tipova oglasa", error.response?.data || error.response);
    }
  };

  return (
    <div>
      <h2 className="text-lg font-bold mb-4">
        Zapratite tipove poslova za koje želite da dobijate obaveštenja
      </h2>
      <div className="flex flex-wrap gap-2">
        {jobTypes.map((type) => {
          const isFollowed = followedJobTypes.includes(type);
          return (
            <button
              key={type}
              onClick={() => clickOnJobType(type)}
              className={
                `px-4 py-2 rounded text-white font-semibold transition-colors ${isFollowed ? "bg-green-500 hover:bg-green-600":"bg-blue-500 hover:bg-blue-600"}`
              }
            >
              {type}
            </button>
          );
        })}
      </div>
    </div>
  );
};

export default JobTypeFollowing;
