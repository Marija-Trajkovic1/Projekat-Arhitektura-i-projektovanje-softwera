import { useNavigate } from "react-router-dom";
import { useState } from "react";
import axios from "axios";

const Register = () => {
  const navigate = useNavigate();
  const [userRegisterData, setUserRegisterData] = useState({
    name: "",
    surname: "",
    userName: "",
    email: "",
    password: "",
    phoneNumber: "",
    street: "",
    city: "",
    homeNumber: "",
    role: "Worker",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUserRegisterData({ ...userRegisterData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await axios.post(
        "https://localhost:7260/UserAuthentication/RegisterNewUser",
        userRegisterData
      );
      alert("Succesfuly registered!");
      navigate("/login");
    } catch (err) {
      console.error(err);
      alert("Trouble while registration!");
    }
  };

  const inputStyle="p-2 border border-gray-300 rounded";

  return(
  <div className="flex items-center justify-center h-screen bg-blue-200">
    <div className="bg-white p-6 rounded-xl shadow-md w-full max-w-md">
      <h2 className="text-2xl font-bold mb-4 text-center">Register</h2>
      <form
        onSubmit={handleSubmit}
        className="flex flex-col gap-4"
      >
        <input
          type="text"
          name="name"
          placeholder="Name"
          value={userRegisterData.name}
          onChange={handleChange}
          className={inputStyle}
          required
        />
        <input
          type="text"
          name="surname"
          placeholder="Surname"
          value={userRegisterData.surname}
          onChange={handleChange}
          className={inputStyle}
          required
        />
        <input
          type="text"
          name="userName"
          placeholder="Username"
          value={userRegisterData.userName}
          onChange={handleChange}
          className={inputStyle}
          required
        />
        <input
          type="email"
          name="email"
          placeholder="Email"
          value={userRegisterData.email}
          onChange={handleChange}
          className={inputStyle}
          required
        />
        <input
          type="password"
          name="password"
          placeholder="Password"
          value={userRegisterData.password}
          onChange={handleChange}
          className={inputStyle}
          required
        />
        <input
          type="text"
          name="phoneNumber"
          placeholder="Phone Number"
          value={userRegisterData.phoneNumber}
          onChange={handleChange}
          className={inputStyle}
          required
        />
        <input
          type="text"
          name="street"
          placeholder="Street"
          value={userRegisterData.street}
          onChange={handleChange}
          className={inputStyle}
          required
        />
        <input
          type="text"
          name="city"
          placeholder="City"
          value={userRegisterData.city}
          onChange={handleChange}
          className={inputStyle}
          required
        />
        <input
          type="text"
          name="homeNumber"
          placeholder="Home Number"
          value={userRegisterData.homeNumber}
          onChange={handleChange}
          className={inputStyle}
          required
        />
        <button
          type="submit"
          className="bg-blue-500 text-white py-2 rounded hover:bg-blue-600"
        >
          Register
        </button>
      </form>
    </div>
  </div>
  );
};

export default Register;
