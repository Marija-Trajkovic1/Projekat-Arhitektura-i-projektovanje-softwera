import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

class SignalRService {
  constructor() {
    this.connection = null;
  }

  startConnection = async (accessToken) => {
    try {
      this.connection = new HubConnectionBuilder()
        .withUrl("https://localhost:7260/taskItHub", {
          accessTokenFactory: () => accessToken,
        })
        .configureLogging(LogLevel.Information)
        .withAutomaticReconnect()
        .build();

      await this.connection.start();
      console.log("SignalR povezan");
    } catch (error) {
      console.error("Greška pri povezivanju na SignalR:", error);
    }
  };

  stopConnection = async () => {
    if (this.connection) {
      await this.connection.stop();
      console.log("SignalR isključen");
    }
  };

  subscribeToEmployer = async (employerId) => {
    if (this.connection) {
      await this.connection.invoke("SubscribeToEmployer", employerId);
    }
  };

  unsubscribeFromEmployer = async (employerId) => {
    if (this.connection) {
      await this.connection.invoke("UnsubscribeFromEmployer", employerId);
    }
  };

  subscribeToJobType = async (jobType) => {
    if (this.connection) {
      await this.connection.invoke("SubscribeToJobType", jobType);
    }
  };

  unsubscribeFromJobType=async(jobType)=>{
    if(this.connection){
        await this.connection.invoke('UnsubscribeFromJobType', jobType);
    }
  };

  subscribeToJob = async(jobId)=>{
    if(this.connection){
        await this.connection.invoke('SubscribeToJob', jobId);
    }
  };

  unsubscribeFromJob = async(jobId)=>{
    if(this.connection){
        await this.connection.invoke('UnsubscribeFromJob', jobId);
    }
  };

  onReceiveNotification = (callback, eventName)=>{
    if(this.connection){
        this.connection.on(eventName, callback);
    }
  };

  offReceiveNotification = (eventName)=>{
    if(this.connection){
        this.connection.off(eventName);
    }
  };

}

export const signalRService = new SignalRService();
