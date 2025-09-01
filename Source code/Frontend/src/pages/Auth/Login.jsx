import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import axios from "axios";

function Login() {
  const [loginCredentials, setLoginCredentials] = useState({
    email: "",
    password: "",
  });
  const [error, setError] = useState(null);

  const navigate = useNavigate();
  const { login} = useAuth();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setLoginCredentials({ ...loginCredentials, [name]: value });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError(null);

    try {
      const response = await axios.post(
        "https://localhost:7260/UserAuthentication/Login",
        loginCredentials
      );

      console.log("API response Login:", response.data);
      const { token, userResponse, role } = response.data;
      console.log("Podaci za login",{token, userResponse, role});
      
      const normalizedRole = role.toUpperCase();
      login(userResponse, token, normalizedRole);
      console.log("Uspesan login, preusmeravanje na profil");
      navigate("/profile");
    } catch (err) {
      console.log("Grska pri loginu:", err);
      setError("Your email and password are incorrect!");
    }
  };

  const inputStyle="border p-2 w-full mb-3 rounded";

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <form
        onSubmit={handleSubmit}
        className="bg-white p-6 rounded-xl shadow-md w-96"
      >
        <h2 className="text-2xl font-bold mb-4">Login</h2>
        
        {error && <p className="text-red-500 mb-2">{error}</p>}

        <input
          type="email"
          placeholder="Email"
          name="email"
          value={loginCredentials.email}
          onChange={handleChange}
          className={inputStyle}
          required
        />
        <input
          type="password"
          placeholder="Password"
          name="password"
          value={loginCredentials.password}
          onChange={handleChange}
          className={inputStyle}
          required
        />

        <button
          type="submit"
          className="w-full bg-blue-500 text-white p-2 rounded hover:bg-blue-600"
        >
          Login
        </button>
      </form>
    </div>
  );
}

export default Login;
