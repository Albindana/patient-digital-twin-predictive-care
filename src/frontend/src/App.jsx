import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { ENDPOINTS } from "./apiConfig";
import { Activity, Heart, Droplet, AlertTriangle, User } from "lucide-react";

export default function App() {
  const [telemetry, setTelemetry] = useState(null);
  const [alert, setAlert] = useState(null);
  const [status, setStatus] = useState("Disconnected");

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(ENDPOINTS.SIGNALR_HUB)
      .withAutomaticReconnect()
      .build();

    const startConnection = async () => {
      try {
        await connection.start();
        setStatus("Connected");
      } catch (err) {
        if (err.message && err.message.includes("stopped during negotiation")) {
          console.warn("SignalR aborted due to unmount.");
        } else {
          setStatus("Error");
        }
      }
    };

    startConnection();

    connection.on("ReceiveTelemetryUpdate", (data) => {
      setTelemetry(data);
      if (data.heartRate <= 120 && data.oxygenLevel >= 90.0) {
        setAlert(null);
      }
    });

    connection.on("ReceiveCriticalAlert", (alertData) => {
      setAlert(alertData);
    });

    return () => {
      connection.stop();
    };
  }, []);

  return (
    <div className="min-h-screen bg-slate-50 p-8 font-sans text-slate-900">
      {/* Header */}
      <header className="mb-8 flex items-center justify-between pb-6 border-b border-slate-200">
        <div className="flex items-center gap-3">
          <div className="bg-blue-600 p-2 rounded-lg">
            <Activity className="text-white w-6 h-6" />
          </div>
          <h1 className="text-2xl font-bold text-slate-800">CareFlow Digital Twin</h1>
        </div>
        <div className="flex items-center gap-2">
          <span className="relative flex h-3 w-3">
            {status === "Connected" && (
              <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-green-400 opacity-75"></span>
            )}
            <span className={`relative inline-flex rounded-full h-3 w-3 ${status === "Connected" ? 'bg-green-500' : 'bg-red-500'}`}></span>
          </span>
          <span className="text-sm font-medium text-slate-600">{status}</span>
        </div>
      </header>

      {/* Critical Alert Banner */}
      {alert && (
        <div className="mb-8 animate-pulse-fast bg-red-100 border-l-4 border-red-600 p-4 rounded-r-lg shadow-sm flex items-start gap-4">
          <AlertTriangle className="text-red-600 w-6 h-6 shrink-0 mt-1" />
          <div>
            <h3 className="text-red-800 font-bold text-lg">{alert.message}</h3>
            <p className="text-red-700 mt-1">
              Patient <span className="font-mono bg-red-200 px-1 rounded">{alert.patientId?.substring(0,8)}...</span> is experiencing abnormal vitals.
            </p>
          </div>
        </div>
      )}

      {/* Vitals Grid */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        
        {/* Heart Rate Card */}
        <div className="bg-white rounded-xl shadow-sm border border-slate-200 p-6 flex flex-col justify-between">
          <div className="flex justify-between items-start mb-4">
            <h2 className="text-slate-500 font-semibold uppercase tracking-wider text-sm">Heart Rate</h2>
            <div className={`p-2 rounded-lg ${telemetry?.heartRate > 100 ? 'bg-red-100 text-red-600' : 'bg-rose-100 text-rose-500'}`}>
              <Heart className={`w-6 h-6 ${telemetry?.heartRate > 100 ? 'animate-pulse-fast' : ''}`} />
            </div>
          </div>
          <div className="flex items-baseline gap-2">
            <span className="text-5xl font-extrabold text-slate-800">
              {telemetry ? telemetry.heartRate : "--"}
            </span>
            <span className="text-slate-500 font-medium">BPM</span>
          </div>
        </div>

        {/* Oxygen Card */}
        <div className="bg-white rounded-xl shadow-sm border border-slate-200 p-6 flex flex-col justify-between">
          <div className="flex justify-between items-start mb-4">
            <h2 className="text-slate-500 font-semibold uppercase tracking-wider text-sm">SpO2 Level</h2>
            <div className={`p-2 rounded-lg ${telemetry?.oxygenLevel < 92 ? 'bg-red-100 text-red-600' : 'bg-blue-100 text-blue-500'}`}>
              <Droplet className="w-6 h-6" />
            </div>
          </div>
          <div className="flex items-baseline gap-2">
            <span className="text-5xl font-extrabold text-slate-800">
              {telemetry ? telemetry.oxygenLevel.toFixed(1) : "--"}
            </span>
            <span className="text-slate-500 font-medium">%</span>
          </div>
        </div>

        {/* Patient Info Card */}
        <div className="bg-slate-800 rounded-xl shadow-sm border border-slate-700 p-6 text-white flex flex-col justify-between">
          <div className="flex justify-between items-start mb-4">
            <h2 className="text-slate-400 font-semibold uppercase tracking-wider text-sm">Patient Monitor</h2>
            <div className="p-2 rounded-lg bg-slate-700 text-slate-300">
              <User className="w-6 h-6" />
            </div>
          </div>
          <div>
            <p className="text-sm text-slate-400 mb-1">Active ID</p>
            <p className="font-mono text-sm break-all bg-slate-900 p-2 rounded">
              {telemetry ? telemetry.patientId : "Waiting for stream..."}
            </p>
            <p className="text-xs text-slate-500 mt-4">
              Last Updated: {telemetry ? new Date(telemetry.timestamp).toLocaleTimeString() : "--:--:--"}
            </p>
          </div>
        </div>

      </div>
    </div>
  );
}