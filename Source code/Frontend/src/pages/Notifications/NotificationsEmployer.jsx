import { useEffect, useState } from "react";
import { useSignalR } from "../../context/SignalRContext";

const NotificationsEmployer = () => {
  const [notifications, setNotifications] = useState([]);
  const { connection } = useSignalR();

  useEffect(() => {
    if (!connection) return;
    const handleNotification = (notification) => {
      console.log(notification);
      setNotifications((prev) => [notification, ...prev]);
    };
    const handleSavedNotifications = (notifications) => {
      if (notifications !== null) {
        setNotifications(notifications);
      } else {
        setNotifications([]);
      }
    };

    connection.on("savednotifications", handleSavedNotifications);
    connection.on("EmployerFollowed", handleNotification);
    connection.on("EmployerUnfollowed", handleNotification);
    connection.on("WorkerApplication", handleNotification);
    connection.on("ApplicationDeclined", handleNotification);
    return () => {
      connection.off("SavedNotifications", handleSavedNotifications);
      connection.off("EmployerFollowed", handleNotification);
      connection.off("EmployerUnfollowed", handleNotification);
      connection.off("WorkerApplication", handleNotification);
      connection.off("applicationDeclined", handleNotification);
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

export default NotificationsEmployer;
