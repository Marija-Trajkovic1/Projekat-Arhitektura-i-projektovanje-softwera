import { useEffect } from "react"
import { signalRService } from "../services/SignalRService"

export const useSignalR = (accessToken)=>{
    useEffect(()=>{
        if(accessToken){
            signalRService.startConnection(accessToken).catch((error)=>{
                console.error('Greška pri pokretanju SignalR konekcije:', error);
            }); 
            return()=>signalRService.stopConnection().catch((error)=>{
                console.error('Greška pri zaustavljanju SignalR konekcije:', error);
            });
        }
    }, [accessToken]);
}