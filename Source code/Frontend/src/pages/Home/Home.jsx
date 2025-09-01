import { useNavigate } from "react-router-dom";
import NavBar from "./NavBar";
import Landing from "./Landing";
import AuthProvider from "../../context/AuthContext";
const Home = () => {
  const navigate = useNavigate();

  return (
      <div className="min-h-screen bg-gray-100">
        <Landing />
      </div>
  );
};

export default Home;
