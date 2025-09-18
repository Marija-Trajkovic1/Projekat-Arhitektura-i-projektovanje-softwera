import { useAuth } from "../../context/AuthContext";
import { useEffect, useState } from "react";
import axios from "axios";

const AcceptedJobs = () => {
  const { token } = useAuth();
  const [acceptedJobs, setAcceptedJobs] = useState([]);
  const [loading, setLoading] = useState(false);
  const [loadingIds, setLoadingIds] = useState([]);
  const [evaluations, setEvaluations] = useState({});

  useEffect(() => {
    const getAcceptedJobs = async () => {
      try {
        setLoading(true);
        const response = await axios.get(
          `https://localhost:7260/FinishedJob/GetFinishedJobAdvertisementsForWorker`,
          { headers: { Authorization: `Bearer ${token}` } }
        );
        setAcceptedJobs(response.data || []);
        console.log(response.data);
        const initialEvaluations = {};
        response.data.forEach((job) => {
          if (job.employerEvaluation) {
            initialEvaluations[job.finishedJobId] = job.employerEvaluation;
          }
        });
        setEvaluations(initialEvaluations);
        console.log(acceptedJobs);
      } catch (error) {
        console.error(
          "Greška pri dohvatanju završenih poslova.",
          error.response?.data || error.response
        );
      } finally {
        setLoading(false);
      }
    };

    getAcceptedJobs();
  }, [token]);

  const evaluateEmployer = async (finishedJobId) => {
    const value = evaluations[finishedJobId];
    if (!value || value < 1 || value > 5) {
      alert("Ocena mora biti između 1 i 5.");
      return;
    }

    try {
      console.log(finishedJobId);
      setLoadingIds((prev) => [...prev, finishedJobId]);
      await axios.put(
        `https://localhost:7260/FinishedJob/EmployerEvaluation?finishedJobId=${finishedJobId}&employerEvaluation=${value}`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setAcceptedJobs((prev) =>
        prev.map((job) =>
          job.finishedJobId === finishedJobId
            ? { ...job, employerEvaluation: value }
            : job
        )
      );
      alert("Uspešno ste ocenili poslodavca!");
    } catch (error) {
      console.error(
        "Greška pri ocenjivanju poslodavca.",
        error.response?.data || error.response
      );
    } finally {
      setLoadingIds((prev) => prev.filter((id) => id !== finishedJobId));
    }
  };

  const handleChange = (e, finishedJobId) => {
    const value = Number(e.target.value);
    setEvaluations((prev) => ({ ...prev, [finishedJobId]: value }));
  };

  if (loading) return <p className="text-gray-500">Učitavanje...</p>;

  return (
    <div className="mb-8">
      <h2 className="text-xl font-semibold mb-4 text-gray-800">
        Vaši završeni poslovi
      </h2>
      {acceptedJobs.length === 0 && (
        <p className="text-gray-500">Nemate završenih poslova.</p>
      )}
      <div className="flex gap-4 overflow-x-auto pb-4">
        {acceptedJobs.map((job) => {
          const isLoading = loadingIds.includes(job.finishedJobId);
          const currentEvaluation = evaluations[job.finishedJobId];

          return (
            <div
              key={job.finishedJobId}
              className="min-w-[280px] max-w-xs border rounded-lg p-4 shadow bg-white hover:shadow-lg transition-shadow"
            >
              <h3 className="font-bold text-lg mb-2 text-gray-900">
                {job.title}
              </h3>
              <p className="text-sm text-gray-600 mb-2">
                {job.shortDescription}
              </p>
              <p className="mt-2 font-semibold text-green-600">
                Plata: {job.jobSalary} RSD/satu
              </p>
              <p className="text-sm text-gray-700">
                {job.homeNumber}, {job.street}, {job.city}
              </p>
              <p className="text-sm text-gray-500 mt-1">
                Datum obavljanja: {job.dateOfExecution}
              </p>

              {job.employerEvaluation ? (
                <p className="mt-3 text-sm font-semibold text-blue-600">
                  Ocena poslodavca: {job.employerEvaluation}/5
                </p>
              ) : (
                <div className="mt-3 flex items-center gap-2">
                  <input
                    type="number"
                    name="employerEvaluation"
                    min="1"
                    max="5"
                    value={currentEvaluation ?? ""}
                    onChange={(e) => handleChange(e, job.finishedJobId)}
                    className="w-16 p-1 border rounded text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                    placeholder="1-5"
                    disabled={isLoading}
                  />
                  <button
                    onClick={() => evaluateEmployer(job.finishedJobId)}
                    disabled={isLoading || !currentEvaluation}
                    className={`px-3 py-1 rounded text-white font-semibold transition-colors ${
                      isLoading || !currentEvaluation
                        ? "bg-gray-400 cursor-not-allowed"
                        : "bg-blue-500 hover:bg-blue-600"
                    }`}
                  >
                    {isLoading ? "Učitavanje..." : "Oceni poslodavca"}
                  </button>
                </div>
              )}
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default AcceptedJobs;
