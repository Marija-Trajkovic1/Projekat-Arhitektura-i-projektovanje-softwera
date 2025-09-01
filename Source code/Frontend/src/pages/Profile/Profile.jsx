import { useAuth } from "../../context/AuthContext";
import { useState, useEffect } from "react";
import axios from "axios";
import { HttpTransportType } from "@microsoft/signalr";

const Profile = () => {
  const { user, token,role, logout, updateAuth} = useAuth();
  const [profileData, setProfileData] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [formData, setFormData] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    if (!user) return;

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
          phoneNumber: response.data.phoneNumber,
          street: response.data.street,
          city: response.data.city,
          homeNumber: response.data.homeNumber,
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
  }, [user, token, logout]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleUpdate = async () => {
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
      alert("Profile updated successfully!");
    } catch (error) {
      console.error(
        "Error updating profile:",
        error.response?.data || error.message
      );
      if (error.response?.status === 401) {
        logout(); // Automatski logout ako 401
      }
      alert("Failed to update profile. Please try again.");
    }
  };

  const handleBeEmployer = async () => {
    try{
      const response = await axios.post(
        `https://localhost:7260/UserAuthentication/ChangeRole`,
        {CurrentRole:"Worker"},
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      updateAuth({
        token:response.data.Token, 
        user:response.data.UserResponse, 
        role:response.data.Role});
        
      console.log("Successfully became an employer:", response.data);
      alert("You are now an employer!");
      navigate("/profile");

    }catch(error){
      console.error('Greška pri promeni role:', error.response?.data || error.message);
      if (error.response?.status === 401) {
        logout();
      }
      alert('Neuspešna promena role. Pokušajte ponovo.');
    }
  }
  if (loading) return <p>Loading profile...</p>;
  if (!profileData) return <p>Profile not found!</p>;

  const inputStyle = "border p-2 w-full mb-2 rounded";

  const editeModeInput = (
    <>
      <input
        type="text"
        name="phoneNumber"
        value={formData.phoneNumber}
        onChange={handleChange}
        className={inputStyle}
        placeholder="Phone Number"
      />
      <input
        type="text"
        name="street"
        value={formData.street}
        onChange={handleChange}
        className={inputStyle}
        placeholder="Street"
      />
      <input
        type="text"
        name="city"
        value={formData.city}
        onChange={handleChange}
        className={inputStyle}
        placeholder="City"
      />
      <input
        type="text"
        name="homeNumber"
        value={formData.homeNumber}
        onChange={handleChange}
        className={inputStyle}
        placeholder="Home Number"
      />
      <div className="flex gap-2 mt-2">
        <button
          onClick={handleUpdate}
          className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
        >
          Save
        </button>
        <button
          onClick={() => setEditMode(false)}
          className="bg-gray-500 text-white px-4 py-2 rounded hover: bg-bray-600"
        >
          Cancel
        </button>
      </div>
    </>
  );

  const displayModeShow = (
    <>
      <p>
      <strong>Trenutno ste ulogovani kao:</strong>{role}
      </p>
      <p>
        <strong>Name:</strong> {profileData.name}
      </p>
      <p>
        <strong>Surname:</strong>
        {profileData.surname}
      </p>
      <p>
        <strong>Email:</strong>
        {profileData.email}
      </p>
      <p>
        <strong>Phone number:</strong>
        {profileData.phoneNumber}
      </p>
      <p>
        <strong>User name:</strong>
        {profileData.userName}
      </p>
      <p>
        <strong>City:</strong>
        {profileData.city}
      </p>
      <p>
        <strong>Street:</strong>
        {profileData.street}
      </p>
      <p>
        <strong>Home number:</strong>
        {profileData.homeNumber}
      </p>

      <button
        onClick={() => setEditMode(true)}
        className="mt-2 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
      >
        Edit Profile
      </button>

      <button onClick={handleBeEmployer}className="mt-2 bg-white-400 text-black px-4 py-2 rounded hover: bg-blue-600">
        Become an employer
      </button>

      <button
        onClick={logout}
        className="mt-2 bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 ml-2"
      >
        Logout
      </button>

    </>
  );

  return (
    <div className="p-6 max-w-3x1 mx-auto">
      <h1 className="text-3xl font-bold mb-4">Profile</h1>
      <div className="bg-white p-4 rounded shadow mb-4">
        {editMode ? editeModeInput : displayModeShow}
      </div>
    </div>
  );
};

export default Profile;
