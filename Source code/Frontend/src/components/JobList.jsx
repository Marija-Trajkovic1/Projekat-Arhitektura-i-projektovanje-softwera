import { useAuth } from "../context/AuthContext";
import { useState, useEffect } from "react";
import JobAdvertisement from "./JobAdvertisement";

const JobList = ({ refreshJobAds, refreshTrigger, setRefreshTrigger }) => {
  const [jobAdvList, setJobAdvList] = useState([]);
  const [loading, setLoading] = useState(false);

  const getListJobAd = async () => {
    setLoading(true);
    try {
      const jobsList = await refreshJobAds();
      console.log("dohvaceni poslovi: ", jobsList);
      setJobAdvList(jobsList || []);
    } catch (error) {
      console.error(
        "Greska pri pribavljanju poslova za poslodavca!",
        error.response?.data || error.message
      );
      setJobAdvList([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    getListJobAd();
  }, [refreshJobAds, refreshTrigger]);

  const prikazPoslova =
    jobAdvList.length === 0 ? (
      <p>Jos uvek nemate postavljen oglas.</p>
    ) : (
      <div className="flex flex-row flex-wrap gap-4">
        {" "}
        {jobAdvList.map((ad) => (
          <JobAdvertisement key={ad.id} ad={ad} setRefreshTrigger={setRefreshTrigger} />
        ))}
      </div>
    );

  return <div>{loading ? <p>Učitavanje...</p> : prikazPoslova}</div>;
};

export default JobList;
