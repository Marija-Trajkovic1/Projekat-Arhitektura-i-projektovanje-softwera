import { useState } from "react";
import axios from "axios";
import { useAuth } from "../../context/AuthContext";
import JobAdvertisementForm from "./JobAdvertisementForm";
import JobAdvertisementDetails from "./JobAdvertisementDetails";
import JobApplicationsModal from "./JobApplicationsModal";

const JobAdvertisement = ({ ad, setRefreshTrigger }) => {
  const { token } = useAuth();
  const [editMode, setEditMode] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [applicationList, setApplicationList] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  
  const [formData, setFormData] = useState({
    title: ad.title,
    shortDescription: ad.shortDescription,
    city: ad.city,
    street: ad.street,
    homeNumber: ad.homeNumber,
    dateOfExecution: ad.dateOfExecution,
    workDuration: ad.workDuration,
    jobSalary: ad.jobSalary,
    jobType: ad.jobType,
  });

  const handleDeleteJobAdv = async () => {
    if (
      !window.confirm("Da li ste sigurni da želite da obrišete ovaj oglas?")
    ) {
      return;
    }

    try {
      const id = ad.id;
      await axios.delete(
        `https://localhost:7260/JobAdvertisement/DeleteJobAdvertisement/${id}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      alert("Oglas je uspešno obrisan!");
      setRefreshTrigger((prev) => prev + 1);
    } catch (error) {
      console.error(
        "Neuspešno brisanje oglasa!",
        error.response?.data || error.message
      );
    }
  };

  const handleUpdateJobAdvertisement = async () => {
    setIsLoading(true);
    try {
      const id = ad.id;
      const response = await axios.put(
        `https://localhost:7260/JobAdvertisement/UpdateJobAdvertisement/${id}`,
        formData,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      ad = response.data;
      alert("Uspešno ste ažurirali oglas!");
      setEditMode(false);
      setRefreshTrigger((prev) => prev + 1);
    } catch (error) {
      console.error(
        "Neuspešno ažuriranje oglasa!",
        error.response?.data || error.message
      );
      alert("Oglas nije ažuriran, molimo Vas pokušajte ponovo!");
    } finally {
      setIsLoading(false);
    }
  };

  const handleCheckApplications = async () => {
    try {
      const id = ad.id;
      const response = await axios.get(
        `https://localhost:7260/JobApplication/GetJobApplicationsForJobAdd/${id}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setApplicationList(response.data || []);
      setIsModalOpen(true);
    } catch (error) {
      console.error(
        "Učitavanje liste prijava za oglas nije uspelo!",
        error.response?.data || error.response
      );
    }
  };

  
  return (
    <div className="w-80 p-4 bg-white rounded-lg shadow-md border border-gray-200">
      {editMode ? (
        <JobAdvertisementForm
          formData={formData}
          onChange={handleInputChange}
          onSubmit={handleUpdateJobAdvertisement}
          onCancel={() => setEditMode(false)}
          isLoading={isLoading}
        />
      ) : (
        <JobAdvertisementDetails
          ad={ad}
          onEdit={() => setEditMode(true)}
          onDelete={handleDeleteJobAdv}
          onCheckApplications={handleCheckApplications}
        />
      )}

      <JobApplicationsModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        applications={applicationList}
        jobTitle={ad.title}
        token ={token}
      />
    </div>
  );
};

export default JobAdvertisement;
