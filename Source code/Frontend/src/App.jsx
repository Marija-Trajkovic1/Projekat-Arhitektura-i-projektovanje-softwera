import { HubConnectionBuilder } from "@microsoft/signalr";

function App() {
  const [connection, setConnection] = useState();

  const notifyNewUser = async (username, group)=>{
    try {
      //initiate connection 
      const connection = new HubConnectionBuilder()
        .withURL("https://localhost:5170/newjobadvertisementHub")
        .configureLogging(LogLevel.Information)
        .build();

        connection.on("ReceiveMessage", (username, message) => {
          console.log(`New message from ${username}: ${message}`);
        })

        await connection.start();

        await connection.invoke()

    } catch (error) {
      
    }
  }


  return (
    <div>

    </div>
  )
}

export default App
