import { useState, useEffect } from "react";
import { useSignalR } from "../../context/SignalRContext";

const NotificationsWorker = () => {
  const [notifications, setNotifications] = useState([]);
  const { connection } = useSignalR();

  useEffect(() => {
      if (!connection) return;
      const handleNotification = (notification) => {
        console.log(notification);
        setNotifications((prev) => [notification, ...prev]);
      };
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
    <div className="max-w-2xl mx-auto p-4">
      <h2 className="text-2xl font-bold mb-4">Vaše notifikacije</h2>
      {notifications.length === 0 ? (
        <p>Nemate novih notifikacija</p>
      ) : (
        <ul>
          {notifications.map((n, index) => (
            <li key={index} className="border-b py-2">
              {n.message}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};
export default NotificationsWorker;
