import { useEffect, useState } from "react";
import { useSignalR } from "../../context/SignalRContext";

const NotificationsEmployer=()=>{
    const [notifications, setNotifications] = useState([]);
    const {connection} = useSignalR();

    useEffect(() => {
    if (!connection) return;
    const handleNotification = (notification) => {
      console.log(notification);
      setNotifications((prev) => [notification, ...prev]);
    };
    connection.on("EmployerFollowed", handleNotification);
    connection.on("EmployerUnfollowed", handleNotification);
    connection.on("WorkerApplication", handleNotification);
    connection.on("ApplicationDeclined", handleNotification);
    return () => {
      connection.off("EmployerFollowed", handleNotification);
      connection.off("EmployerUnfollowed", handleNotification);
      connection.off("WorkerApplication", handleNotification);
      connection.off("applicationDeclined", handleNotification);
    };
  }, [connection]); 

    return(<div className="max-w-2xl mx-auto p-4">
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
    </div>)
}

export default NotificationsEmployer;