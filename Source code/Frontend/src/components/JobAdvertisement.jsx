import { useState } from "react";
import axios from "axios";

const JobAdvertisement = ({ ad, setRefreshTrigger }) => {
  const [editMode, setEditMode] = useState(false);
  const handleDeleteJobAdv = async () => {
    if (!window.confirm("Da li ste sigurni da želite da obrišete ovaj oglas?")) {
      return;
    }

    try{
      
      const response =await axios.delete(
        `https://localhost:7260/JobAdvertisement/DeleteJobAdvertisement?jobAdvertisementId=${ad.id}`,
        {},
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      alert("Oglas je uspešno obrisan!")
      setRefreshTrigger((prev) => prev + 1);
    }catch(error){
      console.error("Neuspešno brisanje oglasa!",  error.response?.data || error.message);
    }
  };

  return (
    <div className="border p-4 mb-4 rounded">
      <p>
        <strong>Naslov oglasa: </strong>
        {ad.title}
      </p>
      <p>
        <strong>Kratak opis: </strong>
        {ad.shortDescription}
      </p>
      <p>
        <strong>Grad: </strong>
        {ad.city}
      </p>
      <p>
        <strong>Ulica: </strong>
        {ad.street}
      </p>
      <p>
        <strong>Kucni broj: </strong>
        {ad.homeNumber}
      </p>
      <p>
        <strong>Datum obavljanja posla: </strong>
        {ad.dateOfExecution}
      </p>
      <p>
        <strong>Trajanje posla: </strong>
        {ad.workDuration}
      </p>
      <p>
        <strong>Naknada po satu: </strong>
        {ad.jobSalary}
      </p>
      <p>
        <strong>Tip posla: </strong>
        {ad.jobType}
      </p>

      <button
        onClick={() => setEditMode(true)}
        className="mt-2 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
      >
        Izmenite oglas
      </button>

      <button
        onClick={handleDeleteJobAdv}
        className="mt-2 bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
      >
        Obriši oglas
      </button>
    </div>
  );
};

export default JobAdvertisement;
