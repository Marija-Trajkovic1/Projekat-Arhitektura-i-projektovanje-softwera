import { useAuth } from "../../context/AuthContext";
import { useState, useEffect, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const Profile = () => {
  const { user, token, role, logout, updateAuth } = useAuth();
  const [profileData, setProfileData] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [formData, setFormData] = useState({
    phoneNumber: "",
    street: "",
    city: "",
    homeNumber: "",
  });
  const [loading, setLoading] = useState(true);
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    if (!user || profileData) return;

    const fetchProfileData = async () => {
      try {
        const response = await axios.get(
          `https://localhost:7260/User/FindUserForProfile`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        setProfileData(response.data);
        setFormData({
          phoneNumber: response.data.phoneNumber || "",
          street: response.data.street || "",
          city: response.data.city || "",
          homeNumber: response.data.homeNumber|| "",
        });
      } catch (error) {
        console.error(
          "Error fetching profile data:",
          error.response?.data || error.message
        );
        if (error.response?.status === 401) {
          logout();
        }
      } finally {
        setLoading(false);
      }
    };
    fetchProfileData();
  }, [user, token, logout, profileData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleUpdate = async () => {
    setIsLoading(true);
    try {
      const response = await axios.put(
        `https://localhost:7260/User/UpdateUserInformation`,
        formData,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setProfileData(response.data);
      setEditMode(false);
      alert("Podaci su uspešno ažurirani!");
    } catch (error) {
      console.error(
        "Error updating profile:",
        error.response?.data || error.message
      );
      if (error.response?.status === 401) {
        logout();
      }
      alert("Podaci sa profila nisu ažurirani! Molimo Vas pokušajte ponovo!");
    }finally{
      setIsLoading(false);
    }
  };

  const handleRoleChange = useCallback(async () => {
    setIsLoading(true);
    try {
      const currentRole = role;
      const response = await axios.post(
        `https://localhost:7260/UserAuthentication/ChangeRole?currentRole=${currentRole}`,
        {},
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      updateAuth({
        token: response.data.token,
        user: response.data.userResponse,
        role: response.data.role,
      });

      setProfileData(response.data.userResponse);
      setFormData({
        phoneNumber: response.data.userResponse.phoneNumber || "",
        street:  response.data.userResponse.street || "",
        city:  response.data.userResponse.city|| "",
        homeNumber:  response.data.userResponse.homeNumber || "",
      });
    } catch (error) {
      console.error(
        "Greška pri promeni role:",
        error.response?.data || error.message
      );
      if (error.response?.status === 401) {
        console.log("401 Unauthorized detected, logging out");
        logout();
      }
      alert("Neuspešna promena uloge! Molimo Vas pokušajte ponovo!");
    }finally{
      setIsLoading(false);
    }
  }, [role, token, updateAuth, navigate, logout, isLoading]);

  if (loading) return <p>Profil se učitava...</p>;
  if (!profileData) return <p>Profil nije pronađen!</p>;

  const inputStyle = "border p-2 w-full mb-2 rounded";

  const editeModeInput = (
    <>
      <input
        type="number"
        name="phoneNumber"
        value={formData.phoneNumber}
        onChange={handleChange}
        className={inputStyle}
        placeholder="Broj telefona"
      />
      <input
        type="text"
        name="city"
        value={formData.city}
        onChange={handleChange}
        className={inputStyle}
        placeholder="Grad"
      />
      <input
        type="text"
        name="street"
        value={formData.street}
        onChange={handleChange}
        className={inputStyle}
        placeholder="Ulica"
      />
      <input
        type="text"
        name="homeNumber"
        value={formData.homeNumber}
        onChange={handleChange}
        className={inputStyle}
        placeholder="Kućni broj"
      />

      <div className="flex gap-2 mt-2">
        <button
          onClick={handleUpdate}
          className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
          disabled={isLoading}
        >
          {isLoading ? "Čuvanje..." : "Sačuvaj"}
        </button>
        <button
          onClick={() => setEditMode(false)}
          className="bg-gray-500 text-white px-4 py-2 rounded hover:bg-bray-600"
          disabled={isLoading}
        >
          Otkaži
        </button>
      </div>
      
    </>
  );

  const displayModeShow = (
    <>
      <p>
        <strong>Trenutno ste ulogovani kao: </strong>
        {role}
      </p>
      <p>
        <strong>Ime: </strong>
        {profileData.name}
      </p>
      <p>
        <strong>Prezime: </strong> 
        {profileData.surname}
      </p>
      <p>
        <strong>Email: </strong>
        {profileData.email}
      </p>
      <p>
        <strong>Broj telefona: </strong>
        {profileData.phoneNumber}
      </p>
      <p>
        <strong>Korisničko ime: </strong>
        {profileData.userName}
      </p>
      <p>
        <strong>Grad: </strong>
        {profileData.city}
      </p>
      <p>
        <strong>Ulica: </strong>
        {profileData.street}
      </p>
      <p>
        <strong>Kućni broj: </strong>
        {profileData.homeNumber}
      </p>

      <button
        onClick={() => setEditMode(true)}
        className="mt-2 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 ml-2"
      >
        Izmenite profil
      </button>

      <button
        onClick={handleRoleChange}
        className="mt-2 bg-blue-400 text-white px-4 py-2 rounded hover: bg-blue-600 ml-2"
        disabled={isLoading}
      >
       {isLoading ? "Obrada...":role==="WORKER"? "Postani poslodavac" : "Postani radnik"}
      </button>

      <button
        onClick={logout}
        className="mt-2 bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 ml-2"
      >
        Odjavi se
      </button>
    </>
  );

  return (
    <div className="p-6 max-w-3x1 mx-auto">
      <h1 className="text-3xl font-bold mb-4">Pregledajte ili ažurirajte Vaše podatke: </h1>
      <div className="bg-white p-4 rounded shadow mb-4">
        {editMode ? editeModeInput : displayModeShow}
      </div>
    </div>
  );
};

export default Profile;
