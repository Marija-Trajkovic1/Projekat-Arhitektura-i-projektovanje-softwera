import { useState, useEffect } from "react";
import { useSignalR } from "../../context/SignalRContext";

const NotificationsWorker = () => {
  const [notifications, setNotifications] = useState([]);
  const { connection } = useSignalR();

  useEffect(() => {
    if (!connection) return;
    const handleNotification = (notification) => {
      console.log(notification);
      const text =
        typeof notification === "string" ? notification : notification.message;
      setNotifications((prev) => [text, ...prev]);
    };
    const handleSavedNotifications = (notifications) => {
      if (notifications !== null) {
        console.log(notifications);
        const normalized = notifications.map((n) =>
          typeof n === "string" ? n : n.message
        );
        setNotifications(normalized);
      } else {
        setNotifications([]);
      }
    };
    connection.on("SavedNotifications", handleSavedNotifications);
    connection.on("WorkerApplication", handleNotification);
    connection.on("ApplicationDeclined", handleNotification);
    connection.on("NewJobPosted", handleNotification);
    connection.on("JobUpdated", handleNotification);
    connection.on("ApplicationAccepted", handleNotification);
    connection.on("ApplicationRejected", handleNotification);
    return () => {
      connection.off("WorkerApplication", handleNotification);
      connection.off("ApplicationDeclined", handleNotification);
      connection.off("NewJobPosted", handleNotification);
      connection.off("JobUpdated", handleNotification);
      connection.off("ApplicationAccepted", handleNotification);
      connection.off("ApplicationRejected", handleNotification);
    };
  }, [connection]);
  return (
    <div className="p-6 max-w-3x1 mx-auto">
      <h1 className="text-2xl font-bold mb-4text-3xl font-bold mb-4">
        Vaša obaveštenja
      </h1>
      {notifications.length === 0 ? (
        <p>Nemate nova obaveštenja.</p>
      ) : (
        <ul>
          {notifications.map((n, index) => (
            <li key={index} className="border-b py-2">
              {n}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};
export default NotificationsWorker;
