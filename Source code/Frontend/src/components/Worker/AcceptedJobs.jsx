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
        setError(null);
        const response = await axios.get(
          `https://localhost:7260/FinishedJob/GetFinishedJobAdvertisementsForWorker`,
          { headers: { Authorization: `Bearer ${token}` } }
        );
        setAcceptedJobs(response.data || []);
        const initialEvaluations = response.data.reduce((acc, job) => {
          if (job.employerEvaluation) {
            acc[job.finishedJobId] = job.employerEvaluation;
          }
          return acc;
        }, {});
        setEvaluations(initialEvaluations);
      } catch (error) {
        console.error("Greška pri dohvatanju završenih poslova.", error.response?.data || error.response);
      } finally {
        setLoading(false);
      }
    };

      getAcceptedJobs();
 
  }, [token]);

  const evaluateEmployer = async (finishedJobId) => {
    const evaluation = evaluations[finishedJobId];
    if (!evaluation || evaluation < 1 || evaluation > 5) {
      setError("Ocena mora biti između 1 i 5.");
      return;
    }

    try {
      setLoadingIds((prev) => [...prev, finishedJobId]);
      const response = await axios.put(
        `https://localhost:7260/api/FinishedJob/EmployerEvaluation/${finishedJobId}`,
        evaluation,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setAcceptedJobs((prev) =>
        prev.map((job) =>
          job.finishedJobId === finishedJobId ? { ...job, employerEvaluation: evaluation } : job
        )
      );
      setEvaluations((prev) => ({
        ...prev,
        [finishedJobId]: evaluation,
      }));
      alert("Uspešno ste ocenili poslodavca!");
    } catch (error) {
      setError("Greška pri ocenjivanju poslodavca.");
    } finally {
      setLoadingIds((prev) => prev.filter((id) => id !== finishedJobId));
    }
  };

  const handleEvaluationChange = (finishedJobId, value) => {
    const numValue = parseInt(value);
    if (isNaN(numValue) || (numValue >= 1 && numValue <= 5)) {
      setEvaluations((prev) => ({
        ...prev,
        [finishedJobId]: numValue || null,
      }));
    }
  };

  if (loading) return <p className="text-gray-500 text-center">Učitavanje...</p>;
  if (acceptedJobs.length === 0) return <p className="text-gray-500 text-center">Nema završenih poslova.</p>;

  return (
    <div className="mb-8">
      <h2 className="text-xl font-semibold mb-4 text-gray-800">Vaši završeni poslovi</h2>
      <div className="flex gap-4 overflow-x-auto pb-4 scrollbar-thin scrollbar-thumb-gray-300 scrollbar-track-gray-100">
        {acceptedJobs.map((job) => {
          const isLoading = loadingIds.includes(job.finishedJobId);
          const hasEvaluation = job.employerEvaluation || evaluations[job.finishedJobId];

          return (
            <div
              key={job.finishedJobId}
              className="min-w-[280px] max-w-xs border rounded-lg p-4 shadow bg-white hover:shadow-lg transition-shadow"
            >
              <h3 className="font-bold text-lg mb-2 text-gray-900">{job.title}</h3>
              <p className="text-sm text-gray-600 mb-2">{job.shortDescription}</p>
              <p className="mt-2 font-semibold text-green-600">Plata: {job.jobSalary} RSD/satu</p>
              <p className="text-sm text-gray-700">{job.homeNumber}, {job.street}, {job.city}</p>
              <p className="text-sm text-gray-500 mt-1">Datum obavljanja: {job.dateOfExecution}</p>

              {hasEvaluation ? (
                <p className="mt-3 text-sm font-semibold text-blue-600">Ocena poslodavca: {hasEvaluation}/5</p>
              ) : (
                <div className="mt-3 flex items-center gap-2">
                  <input
                    type="number"
                    min="1"
                    max="5"
                    value={evaluations[job.finishedJobId] || ""}
                    onChange={(e) => handleEvaluationChange(job.finishedJobId, e.target.value)}
                    className="w-16 p-1 border rounded text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                    placeholder="1-5"
                    disabled={isLoading}
                  />
                  <button
                    onClick={() => evaluateEmployer(job.finishedJobId)}
                    disabled={isLoading || !evaluations[job.finishedJobId]}
                    className={`px-3 py-1 rounded text-white font-semibold transition-colors ${
                      isLoading || !evaluations[job.finishedJobId]
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