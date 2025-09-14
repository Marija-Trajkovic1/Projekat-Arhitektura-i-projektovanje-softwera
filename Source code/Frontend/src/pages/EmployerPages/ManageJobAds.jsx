import { useAuth } from "../../context/AuthContext";
import { useState, useCallback } from "react";
import axios from "axios";
import JobList from "../../components/Employer/JobList";

const ManageJobAds = () => {
  const { token, role } = useAuth();
  const [isModalActive, setIsModalActive] = useState(false);
  const [formData, setFormData] = useState({
    title: "",
    shortDescription: "",
    city: "",
    street: "",
    homeNumber: "",
    dateOfExecution: "",
    workDuration: "",
    jobSalary: "",
    jobType: "",
  });
  const [loading, setLoading] = useState(false);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const refreshJobAds = useCallback(async () => {
    try {
      const response = await axios.get(
        `https://localhost:7260/JobAdvertisement/GetFilteredJobAdvertisements?filterBy=employer`,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      console.log("API odgovor:", response.data);
      const jobList = response.data.jobAdvList || response.data || [];
      console.log("Parsirana lista", jobList);
      return jobList;
    } catch (error) {
      console.error("Greska pri osvezavanju poslova!", error);
      return [];
    }
  }, [token]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleNewJobSubmit = async (e) => {
     if(e &&e.preventDefault) e.preventDefault();
    setLoading(true);
    try {
      const response = await axios.post(
        `https://localhost:7260/JobAdvertisement/AddNewJobAdvertisement`,
        formData,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      alert("Uspesno ste kreirali novi oglas!");
      setIsModalActive(false);
      setFormData({
        title: "",
        shortDescription: "",
        city: "",
        street: "",
        homeNumber: "",
        dateOfExecution: "",
        workDuration: "",
        jobSalary: "",
        jobType: "",
      });
      setRefreshTrigger((prev)=>prev+1);
    } catch (error) {
      console.error(
        "Greska pri kreiranju oglasa:",
        error.response?.data || error.message
      );
      alert("Kreiranje oglasa nije uspelo! Probajte ponovo!");
    } finally {
      setLoading(false);
    }
  };

  const handleAddNewAdvClick = () => {
    setIsModalActive(true);
  };

  const modalForNewJob = isModalActive && (
    <div className="fixed inset-0 bg-gray-600 bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white p-6 rounded shadow-lg w-full max-w-md">
        <h2 className="text-xl font-bold mb-4">Unesite podatke za novi oglas</h2>
        <form onSubmit={handleNewJobSubmit} className="flex flex-col gap-4">
          <input
            type="text"
            name="title"
            value={formData.title}
            onChange={handleChange}
            className="border p-2 rounded"
            placeholder="Naslov oglasa"
            required
          />
          <input
            type="text"
            name="shortDescription"
            value={formData.shortDescription}
            onChange={handleChange}
            className="border p-2 rounded"
            placeholder="Kratak opis"
            required
          />
          <input
            type="text"
            name="city"
            value={formData.city}
            onChange={handleChange}
            className="border p-2 rounded"
            placeholder="Grad"
            required
          />
          <input
            type="text"
            name="street"
            value={formData.street}
            onChange={handleChange}
            className="border p-2 rounded"
            placeholder="Ulica"
            required
          />
          <input
            type="text"
            name="homeNumber"
            value={formData.homeNumber}
            onChange={handleChange}
            className="border p-2 rounded"
            placeholder="Kucni broj"
            required
          />
          <input
            type="date"
            name="dateOfExecution"
            value={formData.dateOfExecution}
            onChange={handleChange}
            className="border p-2 rounded"
            placeholder="Datum"
            required
          />
          <input
            type="number"
            name="workDuration"
            value={formData.workDuration}
            onChange={handleChange}
            className="border p-2 rounded"
            placeholder="Trajanje posla u casovima"
            required
          />
          <input
            type="number"
            name="jobSalary"
            value={formData.jobSalary}
            onChange={handleChange}
            className="border p-2 rounded"
            placeholder="Visina naknade u dinarima po satu"
            required
          />
          <input
            type="text"
            name="jobType"
            value={formData.jobType}
            onChange={handleChange}
            className="border p-2 rounded"
            placeholder="Tip posla"
          />

          <div className="flex gap-2">
            <button
              type="submit"
              className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
              disabled={loading}
            >
              {loading ? "Čuvanje..." : "Sačuvaj"}
            </button>
            <button
              type="button"
              onClick={() => setIsModalActive(false)}
              className="bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600"
              disabled={loading}
            >
              Otkaži
            </button>
          </div>
        </form>
      </div>
    </div>
  );

  return (
    <div className="p-6 max-w-3x1 mx-auto">
      <h1 className="text-3xl font-bold mb-4">
        Kreirajte novi oglas ili pregledajte postojeće
      </h1>
      <div className="mb-4">
        <button
          onClick={handleAddNewAdvClick}
          className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
          disabled={loading}
        >
          {loading ? "Učitavanje..." : "Kreiraj novi oglas"}
        </button>
      </div>
      {modalForNewJob}
      <div className="mt-6">
        <JobList
          refreshJobAds={refreshJobAds}
          refreshTrigger={refreshTrigger}
          setRefreshTrigger={setRefreshTrigger}
        />
      </div>
    </div>
  );
};

export default ManageJobAds;
