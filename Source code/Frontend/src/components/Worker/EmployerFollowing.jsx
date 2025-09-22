import { useAuth } from "../../context/AuthContext";
import { useState, useEffect } from "react";
import axios from "axios";
import { useSignalR } from "../../context/SignalRContext";

const EmployerFollowing = ({onEmployersChange}) => {
  const { token } = useAuth();
  const {connection} = useSignalR();
  const [employersList, setEmployersList] = useState([]);
  const [followedEmployers, setFollowedEmployers] = useState([]);
  const [loadingIds, setLoadingIds] = useState([]);

  useEffect(() => {
    const getEmployers = async () => {
      try {
        const response = await axios.get(
          `https://localhost:7260/User/GetEmployers`,
          { headers: { Authorization: `Bearer ${token}` } }
        );
        setEmployersList(response.data || []);

        const followedResponse = await axios.get(
          `https://localhost:7260/UserFollowing/GetFollowedEmployers`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        const followed = followedResponse.data.map((e) => String(e.id)) || []
        setFollowedEmployers(followed);
        onEmployersChange(followed);
      } catch (error) {
        console.error(
          "Pribavljanje poslodavaca nije uspelo",
          error.response?.data || error.response
        );
      }
    };
    getEmployers();
  }, [onEmployersChange]);

  const clickOnFollow = async (employerId) => {
    try {
      setLoadingIds((prev) => [...prev, employerId]);
      if (followedEmployers.includes(employerId)) {
        await axios.delete(
          `https://localhost:7260/UserFollowing/UnfollowEmployer/${employerId}`,
          { headers: { Authorization: `Bearer ${token}` } }
        );
       
        alert("Uspešno ste otpratili poslodavca!");
         await connection.invoke("UnfollowEmployer", employerId);
      } else {
        await axios.post(
          `https://localhost:7260/UserFollowing/NewFollowing/${employerId}`,
          {},
          { headers: { Authorization: `Bearer ${token}` } }
        );
        alert("Uspešno ste zapratili poslodavca!");
        await connection.invoke("FollowEmployer", employerId);
      }

      const refreshed = await axios.get(
        `https://localhost:7260/UserFollowing/GetFollowedEmployers`,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      const updated = refreshed.data.map((e) => String(e.id)) || [];
      setFollowedEmployers(updated);
      onEmployersChange(updated);
    } catch (error) {
      console.error(
        "Neuspelo osvežavanje liste praćenih poslodavaca",
        error.response?.data || error.response
      );
    } finally {
      setLoadingIds((prev) => prev.filter((id) => id !== employerId));
    }
  };

  const pStyle="text-sm text-gray-600 mb-2"

  return (
    <div>
      <h2 className="text-lg font-bold mb-4">
        Pregledajte i zapratite poslodavce
      </h2>
      <div className="flex flex-wrap gap-4">
        {employersList.map((employer)=>{
            const isFollowed = followedEmployers.includes(String(employer.id));
            const isLoading = loadingIds.includes(employer.id);
            
            return(
                <div key={employer.id}
                className="border p-4 rounded shadow flex flex-col items-start w-60">
                    <h3 className="font-semibold mb-2">{employer.userName}</h3>
                    <p className={pStyle}>{employer.name} {employer.surname}</p>
                    <p className={pStyle}>{employer.email}</p>
                    <p className={pStyle}>{employer.phoneNumber}</p>
                    <p className={pStyle}>{employer.city}, {employer.street}, {employer.homeNumber}</p>
                    <button onClick={()=>clickOnFollow(employer.id)}
                        disabled={isLoading}
                        className={`px-4 py-2 rounded text-white font-semibold transition-colors ${isFollowed? "bg-green-500 hover:bg-green-600":"bg-red-500 hover:bg-red-600"} ${isLoading ? "opacity-50 cursor-not-allowed": ""}`}>
                            {isLoading ? "Učitavanje..." : isFollowed ? "Otprati": "Zaprati" }
                        </button>
                </div>
            )
        })}
      </div>
    </div>
  );
};

export default EmployerFollowing;
