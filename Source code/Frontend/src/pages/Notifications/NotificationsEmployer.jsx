import { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { useSignalR } from "../../hooks/useSignalR";
import { signalRService } from "../../services/SignalRService";

const NotificationsEmployer=()=>{
    const {token}=useAuth();
    const [notifications, setNotifications]=useState([]);
    useSignalR(token);
    useEffect(()=>{
        const handleNotification = (message)=>{
            setNotifications((prev)=>[
                {id:prev.length+1, message},
                ...prev
            ]);
        };
        signalRService.onReceiveNotification(handleNotification, "EmploterFollowed");
        signalRService.onReceiveNotification(handleNotification, "EmployerUnfollowed");

        return()=>{
            signalRService.offReceiveNotification("EmployerFollowed");
            signalRService.offReceiveNotification("EmployerUnfollowed");
        }
    }, [token]);
    return(<div className="max-w-2xl mx-auto p-4">
      <h2 className="text-2xl font-bold mb-4">Vaše notifikacije</h2>
      {notifications.length === 0 ? (
        <p>Nemate novih notifikacija</p>
      ) : (
        <ul>
          {notifications.map((n) => (
            <li key={n.id} className="border-b py-2">
              {n.message}
            </li>
          ))}
        </ul>
      )}
    </div>)
}

export default NotificationsEmployer;