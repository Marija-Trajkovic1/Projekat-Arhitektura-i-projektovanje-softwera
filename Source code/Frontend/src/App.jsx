import { BrowserRouter as Router } from "react-router-dom";
import NavBar from "./pages/Home/NavBar";
import AppRoutes from "./routes/AppRoutes";

function App() {
  return (
    <Router>
      <NavBar />
      <AppRoutes  />
    </Router>
  );
}

export default App;
