const JobAdvertisementForm =({formData, setFormData, onSave, onCancel, isLoading})=>{
    const inputStyle = "border p-2 w-full mb-2 rounded";

    const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  return(
    <>
      <h2 className="text-xl font-bold mb-4">Izmeni oglas</h2>
      <input
        type="text"
        name="title"
        value={formData.title}
        onChange={handleInputChange}
        className={inputStyle}
        placeholder="Naslov oglasa"
      />
      <input
        type="text"
        name="shortDescription"
        value={formData.shortDescription}
        onChange={handleInputChange}
        className={inputStyle}
        placeholder="Kratak opis"
        required
      />
      <input
        type="text"
        name="city"
        value={formData.city}
        onChange={handleInputChange}
        className={inputStyle}
        placeholder="Grad"
        required
      />
      <input
        type="text"
        name="street"
        value={formData.street}
        onChange={handleInputChange}
        className={inputStyle}
        placeholder="Ulica"
        required
      />
      <input
        type="text"
        name="homeNumber"
        value={formData.homeNumber}
        onChange={handleInputChange}
        className={inputStyle}
        placeholder="Kucni broj"
        required
      />
      <input
        type="date"
        name="dateOfExecution"
        value={formData.dateOfExecution}
        onChange={handleInputChange}
        className={inputStyle}
        placeholder="Datum"
        required
      />
      <input
        type="number"
        name="workDuration"
        value={formData.workDuration}
        onChange={handleInputChange}
        className={inputStyle}
        placeholder="Trajanje posla u casovima"
        required
      />
      <input
        type="number"
        name="jobSalary"
        value={formData.jobSalary}
        onChange={handleInputChange}
        className={inputStyle}
        placeholder="Visina naknade u dinarima po satu"
        required
      />
      <input
        type="text"
        name="jobType"
        value={formData.jobType}
        onChange={handleInputChange}
        className={inputStyle}
        placeholder="Tip posla"
      />

      <div className="flex gap-2 mt-2">
        <button
          onClick={onSave}
          className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
          disabled={isLoading}
        >
          {isLoading ? "Čuvanje..." : "Ažuriraj oglas"}
        </button>
        <button
          onClick={onCancel}
          className="bg-gray-500 text-white px-4 py-2 rounded hover:bg-bray-600"
          disabled={isLoading}
        >
          Otkaži
        </button>
      </div>
    </>
  );
}

export default JobAdvertisementForm;