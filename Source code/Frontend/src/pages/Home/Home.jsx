import { useNavigate } from "react-router-dom";
import Landing from "./Landing";
const Home = () => {
  const navigate = useNavigate();

  return (
      <div className="min-h-screen bg-gray-100">
        <Landing />
      </div>
  );
};

export default Home;
