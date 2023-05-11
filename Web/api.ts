export interface ChangeCredentialsRequest {
  /** @format int32 */
  vmid: number;
  /** @minLength 1 */
  password: string;
  /** @minLength 1 */
  username: string;
  /** @minLength 1 */
  sshKey: string;
}

export interface CreateVmRequest {
  vm: Vm;
  type: VmType;
}

export interface HaInt {
  /** @format int32 */
  managed?: number;
}

export interface LoginRequest {
  /** @minLength 1 */
  email: string;
  /** @minLength 1 */
  password: string;
}

export interface LoginResponse {
  user: User;
  /** @minLength 1 */
  token: string;
}

export interface RegistrationRequest {
  /** @minLength 1 */
  firstName: string;
  /** @minLength 1 */
  secondName: string;
  /** @minLength 1 */
  email: string;
  /** @minLength 1 */
  password: string;
}

export interface User {
  /** @minLength 1 */
  id: string;
  /** @minLength 1 */
  firstName: string;
  /** @minLength 1 */
  secondName: string;
  /** @minLength 1 */
  email: string;
  role: UserRole;
  vms: number[];
}

export enum UserRole {
  Anon = "Anon",
  User = "User",
  Admin = "Admin",
  SuperUser = "SuperUser",
}

export interface Vm {
  /** @format int32 */
  vmid: number;
  /** @minLength 1 */
  name: string;
}

export interface VmBaseStatusCurrent {
  /** @format int64 */
  netIn?: number;
  /** @format int64 */
  netOut?: number;
  /** @format int64 */
  diskUsage?: number;
  /** @format int64 */
  diskSize?: number;
  /** @format double */
  diskUsagePercentage?: number;
  /** @format int64 */
  memoryUsage?: number;
  /** @format int64 */
  memorySize?: number;
  memoryInfo?: string | null;
  /** @format double */
  memoryUsagePercentage?: number;
  /** @format double */
  cpuUsagePercentage?: number;
  /** @format int64 */
  cpuSize?: number;
  cpuInfo?: string | null;
  name?: string | null;
  /** @format int64 */
  vmId?: number;
  /** @format int64 */
  diskRead?: number;
  /** @format int64 */
  diskWrite?: number;
  /** @format int64 */
  pid?: number;
  status?: string | null;
  isRunning?: boolean;
  isStopped?: boolean;
  isPaused?: boolean;
  isTemplate?: boolean;
  /** @format int64 */
  uptime?: number;
  ha?: HaInt;
}

export enum VmType {
  Kali = "Kali",
  Windows = "Windows",
  Ubuntu = "Ubuntu",
}
