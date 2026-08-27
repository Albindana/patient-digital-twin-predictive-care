export const API_BASE_URL = "http://localhost:5050";

export const ENDPOINTS = {
  IDENTITY: `${API_BASE_URL}/api/identity`,
  CARE: `${API_BASE_URL}/api/care`,
  TELEMETRY: `${API_BASE_URL}/api/telemetry`,
  BILLING: `${API_BASE_URL}/api/billing`,
  SIGNALR_HUB: `${API_BASE_URL}/hubs/telemetry`
};