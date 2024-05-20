export interface Test {
  id: string;
  name: string;
  description: string;
  questions: string[];
}

export interface Question {
  id?: string;
  text?: string;
  description?: string;
  questionType?: string;
  correctAnswer?: string;
  questionData?: string[];
}

export interface BalloonInfoInt {
  /** @format int64 */
  actual?: number;
  /** @format int64 */
  maxMem?: number;
  /** @format int64 */
  lastUpdate?: number;
}

export interface BlockstatInt {
  /** @format int64 */
  unmapBytes?: number;
  /** @format int64 */
  flushOperations?: number;
  accountInvalid?: boolean;
  accountFailed?: boolean;
  /** @format int64 */
  flushTotalTimeNs?: number;
  /** @format int64 */
  failedWrOperations?: number;
  /** @format int64 */
  wrMerged?: number;
  /** @format int64 */
  rdTotalTimeNs?: number;
  /** @format int64 */
  rdOperations?: number;
  /** @format int64 */
  rdBytes?: number;
  /** @format int64 */
  invalidRdOperations?: number;
  /** @format int64 */
  failedFlushOperations?: number;
  /** @format int64 */
  failedRdOperations?: number;
  /** @format int64 */
  unmapTotalTimeNs?: number;
  timedStats?: any[] | null;
  /** @format int64 */
  invalidFlushOperations?: number;
  /** @format int64 */
  unmapOperations?: number;
  /** @format int64 */
  invalidUnmapOperations?: number;
  /** @format int64 */
  invalidWrOperations?: number;
  /** @format int64 */
  wrHighestOffset?: number;
  /** @format int64 */
  unmapMerged?: number;
  /** @format int64 */
  rdMerged?: number;
  /** @format int64 */
  wrOperations?: number;
  /** @format int64 */
  wrTotalTimeNs?: number;
  /** @format int64 */
  wrBytes?: number;
  /** @format int64 */
  failedUnmapOperations?: number;
}

export interface ChangeCredentialsRequest {
  /** @minLength 1 */
  vmid: number;
  /** @minLength 1 */
  password: string;
  /** @minLength 1 */
  username: string;
  /** @minLength 1 */
  sshKey: string;
}

export interface CreateLabRequest {
  /** @minLength 1 */
  id: string;
}

export interface CreateNewItem {
  /** @minLength 1 */
  title: string;
  /** @minLength 1 */
  text: string;
  /** @minLength 1 */
  createdAt: string;
}

export interface HaInt {
  /** @format int32 */
  managed?: number;
}

export interface Ip {
  ipAddressType?: string | null;
  ipAddress?: string | null;
  /** @format int32 */
  prefix?: number;
}

export interface Lab {
  /** @minLength 1 */
  id: string;
  title?: string;
  shortDescription?: string;
  description?: string;
  labsEntitys?: string[];
}

export interface LoginRequest {
  /** @minLength 1 */
  email: string;
  /** @minLength 1 */
  password: string;
}

export interface News {
  /** @minLength 1 */
  id: string;
  /** @minLength 1 */
  title: string;
  /** @minLength 1 */
  text: string;
  /** @minLength 1 */
  createdAt: string;
}

export interface NicsInt {
  /** @format int64 */
  netIn?: number;
  /** @format int64 */
  netOut?: number;
}

export interface NodeTask {
  node?: string | null;
  status?: string | null;
  statusOk?: boolean;
  vmId?: string | null;
  /** @format int64 */
  startTime?: number;
  /** @format date-time */
  startTimeDate?: string;
  /** @format date-time */
  endTimeDate?: string;
  duration?: TimeSpan;
  durationInfo?: string | null;
  /** @format int64 */
  endTime?: number;
  type?: string | null;
  description?: string | null;
  descriptionFull?: string | null;
  uniqueTaskId?: string | null;
  user?: string | null;
  /** @format int32 */
  pid?: number;
}

export interface ProxmoxSupportInt {
  pbsDirtyBitmapSaveVm?: boolean;
  pbsMasterKey?: boolean;
  queryBitmapInfo?: boolean;
  pbsDirtyBitmapMigration?: boolean;
  pbsDirtyBitmap?: boolean;
  pbsLibraryVersion?: string | null;
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

export interface ResultInt {
  ipAddresses?: Ip[] | null;
  statistics?: Statistics;
  name?: string | null;
  hardwareAddress?: string | null;
}

export interface Statistics {
  /** @format int64 */
  rxPackets?: number;
  /** @format int64 */
  rxBytes?: number;
  /** @format int64 */
  txErrors?: number;
  /** @format int64 */
  txDropped?: number;
  /** @format int64 */
  rxDropped?: number;
  /** @format int64 */
  rxErrors?: number;
  /** @format int64 */
  txBytes?: number;
  /** @format int64 */
  txPackets?: number;
}

export interface TimeSpan {
  /** @format int64 */
  ticks?: number;
  /** @format int32 */
  days?: number;
  /** @format int32 */
  hours?: number;
  /** @format int32 */
  milliseconds?: number;
  /** @format int32 */
  microseconds?: number;
  /** @format int32 */
  nanoseconds?: number;
  /** @format int32 */
  minutes?: number;
  /** @format int32 */
  seconds?: number;
  /** @format double */
  totalDays?: number;
  /** @format double */
  totalHours?: number;
  /** @format double */
  totalMilliseconds?: number;
  /** @format double */
  totalMicroseconds?: number;
  /** @format double */
  totalNanoseconds?: number;
  /** @format double */
  totalMinutes?: number;
  /** @format double */
  totalSeconds?: number;
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
  /** @minLength 1 */
  labs: string;
  isApproved: boolean;
}

export enum UserRole {
  Anon = "Anon",
  User = "User",
  Admin = "Admin",
  SuperUser = "SuperUser",
  Teacher = "Teacher",
}

export interface VmQemuAgentNetworkGetInterfaces {
  result?: ResultInt[] | null;
}

export interface VmQemuStatusCurrent {
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
  vmid?: number;
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
  spice?: boolean;
  agent?: boolean;
  runningQemu?: string | null;
  runningMachine?: string | null;
  qmpstatus?: string | null;
  nics?: Record<string, NicsInt>;
  blockStat?: Record<string, BlockstatInt>;
  /** @format int64 */
  balloon?: number;
  ballooninfo?: BalloonInfoInt;
  proxmoxSupport?: ProxmoxSupportInt;
}

export interface LabReservation {
  /** @minLength 1 */
  id: string;
  /** @minLength 1 */
  timeStart: string;
  /** @minLength 1 */
  timeEnd: string;
  /** @minLength 1 */
  theme: string;
  /** @minLength 1 */
  description: string;
  reservor: User;
  lab: Lab;
}

export interface CreateLabReservationRequest {
  timeStart: number;
  timeEnd: number;
  /** @minLength 1 */
  theme: string;
  /** @minLength 1 */
  description: string;
  reservorId: string|undefined;
  lab: Lab|null;
}

export interface UpdateLabReservationRequest {
  /** @minLength 1 */
  id: string;
  timeStart: number;
  timeEnd: number;
  /** @minLength 1 */
  theme: string;
  /** @minLength 1 */
  description: string;
  reservorId: string|undefined;
  lab: Lab|null;
  currentUserId: string|undefined;
}