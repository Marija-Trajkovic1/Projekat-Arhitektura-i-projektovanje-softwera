import {BrowserRouter as Router } from "react-router-dom";
import AppRoutes from "./routes/AppRoutes";
import NavBar from "./pages/Home/NavBar";

function App() {
  return (
    <Router>
      <NavBar />
      <AppRoutes />
    </Router>
  )
}

export default App;
