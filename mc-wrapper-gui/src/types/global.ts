export interface ServerInfo {
  id: string;
  name: string;
  jarFile: string;
  javaArguments: string;
  enablePlugins: boolean;
  enabledPlugins: Plugin[];
  createdAt: Date;
  isRunning: boolean;
}

export interface Plugin {
  id: string;
  name: string;
  version: string;
  fileName: string;
}

export interface RamUsage {
  //TODO: change timestamp type from string to Date
  timestamp: string;
  amount: number;
}

export interface CpuUsage {
  name: string;
  value: number;
}
