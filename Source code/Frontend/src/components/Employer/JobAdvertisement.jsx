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
    if (!window.confirm("Da li ste sigurni da želite da obrišete ovaj oglas?")) return;

    try {
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
      await axios.put(
        `https://localhost:7260/JobAdvertisement/UpdateJobAdvertisement/${id}`,
        formData,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
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

  const handleCheckApplications = () => {
      setIsModalOpen(true);
    }

  return (
    <div className="w-80 p-4 bg-white rounded-lg shadow-md border border-gray-200">
      {editMode ? (
        <JobAdvertisementForm
          formData={formData}
          setFormData={setFormData}
          onSave={handleUpdateJobAdvertisement}
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
        jobId={ad.id}
        jobTitle={ad.title}
      />
    </div>
  );
};

export default JobAdvertisement;
