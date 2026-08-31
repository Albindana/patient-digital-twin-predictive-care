import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { ENDPOINTS } from "./apiConfig";

export default function App() {
  const [telemetry, setTelemetry] = useState(null);
  const [alert, setAlert] = useState(null);

  useEffect(() => {
    // 1. Establish SignalR WebSocket connection to Gateway
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(ENDPOINTS.SIGNALR_HUB)
      .withAutomaticReconnect()
      .build();

//       const startConnection = async () => {
//     try {
//       await connection.start();
//       console.log("Connected to Telemetry SignalR Hub");
//     } catch (err) {
//       // Ignore the strict mode abort error during quick unmounts
//       if (err.message && err.message.includes("stopped during negotiation")) {
//         console.warn("SignalR connection aborted due to component unmount.");
//       } else {
//         console.error("SignalR Connection Error: ", err);
//       }
//     }
//   };

//   startConnection();

//   return () => {
//     // Ensure the connection stops when the component unmounts
//     connection.stop();
//   };
// }, []);
    connection.start()
      .then(() => {
        console.log("Connected to Telemetry SignalR Hub");
      })
      .catch((err) => console.error("SignalR Connection Error: ", err));

    // 2. Listen for live vital updates
    connection.on("ReceiveTelemetryUpdate", (data) => {
      setTelemetry(data);
    });

    // 3. Listen for critical health alerts
    connection.on("ReceiveCriticalAlert", (alertData) => {
      console.log("Normal update received:", data);
      setAlert(alertData);
    });

    return () => {
      connection.stop();
    };
  }, []);

  return (
    <div style={{ padding: "2rem", fontFamily: "sans-serif" }}>
      <h1>Patient Digital Twin Dashboard</h1>

      {alert && (
        <div style={{ background: "#ff4d4d", color: "white", padding: "1rem", borderRadius: "8px", marginBottom: "1rem" }}>
          <h2>⚠️ {alert.message}</h2>
          <p>Heart Rate: {alert.heartRate} BPM | Oxygen: {alert.oxygenLevel}%</p>
        </div>
      )}

      <div style={{ border: "1px solid #ccc", padding: "1.5rem", borderRadius: "8px" }}>
        <h3>Live Vitals Feed</h3>
        {telemetry ? (
          <ul>
            <li><strong>Patient ID:</strong> {telemetry.patientId}</li>
            <li><strong>Heart Rate:</strong> {telemetry.heartRate} BPM</li>
            <li><strong>Oxygen Level:</strong> {telemetry.oxygenLevel}%</li>
            <li><strong>Timestamp:</strong> {new Date(telemetry.timestamp).toLocaleTimeString()}</li>
          </ul>
        ) : (
          <p>Waiting for live IoT sensor stream...</p>
        )}
      </div>
    </div>
  );
}