import axios from 'axios';
import { CpuUsage, RamUsage, ServerInfo } from '../types/global';

export const Api = axios.create({
  baseURL: process.env.REACT_APP_BACKEND_BASE_URL,
});

export const GETAllServerInfo = async (
  count: number = -1,
): Promise<ServerInfo[]> => (await Api.get(`/server/info/${count}`)).data;

export const GETRamUsage = (): RamUsage[] => {
  return [
    { timestamp: '00:00', amount: 500 },
    { timestamp: '03:00', amount: 550 },
    { timestamp: '06:00', amount: 589 },
    { timestamp: '09:00', amount: 469 },
  ];
};

export const GETCpuUsage = (): CpuUsage[] => {
  return [
    { name: 'Server (Test)', value: 25 },
    { name: 'Server (McWrapper)', value: 45 },
    { name: 'Server (Survival)', value: 10 },
    { name: 'Server (Hardcore)', value: 20 },
  ];
};

export default Api;
