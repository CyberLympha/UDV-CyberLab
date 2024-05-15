import {
    ChangeCredentialsRequest,
    RegistrationRequest,
    User,
    LoginRequest,
    News, Lab, VmQemuAgentNetworkGetInterfaces, LabReservation, CreateLabReservationRequest, UpdateLabReservationRequest,
    LabWork
} from "../../api";

import {HttpClient} from "./httpClient";


export class ApiService {
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this.httpClient = httpClient
    }

    public login(request: LoginRequest) {
        return this.httpClient.post<LoginRequest, { user: User, token: string }>('/auth/login', request)
    }

    public approveUsers(request: string[]) {
        return this.httpClient.post<string[], void>('/User/approve', request)
    }

    public getCurrentUser() {
        return this.httpClient.get<User>('/User/user');
    }

    public createLab(id: string) {
        return this.httpClient.post<{ id: string }, string>('/labs/create', {id})
    }

    public stopVm(request: { vmid: number }) {
        return this.httpClient.get<{ uuid: string }>('/vm/stop', request)
    }

    public getLabs() {
        return this.httpClient.get<Lab[]>('/labs/get')

    }

    public setPassword(vmid: number, username: string, password: string, ssh: string) {
        return this.httpClient.post<ChangeCredentialsRequest, void>('/vm/setPassword', {
            vmid,
            username,
            password,
            sshKey: encodeURIComponent(ssh)
        })

    }

    public startVm(request: { vmid: number }) {
        return this.httpClient.get<{ uuid: string }>('/vm/start', request)

    }

    public getLabStatus(request: { id: string }) {
        return this.httpClient.get<any[]>('/labs/status', request)

    }

    public getVmConfig(request: { id: string }) {
        return this.httpClient.get<any>('/vm/config', request)

    }

    public getTaskStatus(request: { uuid: string }) {
        return this.httpClient.get<any>('/vm/task', {uuid: "UPID:pve:00007903:001268CE:646E2134:qmstart:318:root@pam!aaaaaaaaa-bbb-cccc-dddd-ef0123456789:"})

    }

    public getVmIp(request: { vmid: string }) {
        return this.httpClient.get<VmQemuAgentNetworkGetInterfaces>('/vm/ip', request)

    }

    public getLabsUsers(request: { id: string }) {
        return this.httpClient.get<User[]>('/labs/users', request)
    }

    public getUsers() {
        return this.httpClient.get<User[]>('/User/users')

    }

    public registration(request: RegistrationRequest) {
        return this.httpClient.post<RegistrationRequest, void>('/auth/register', request)
    }

    public getNews(request: { offset: number }) {
        return this.httpClient.get<News[]>('/news', request)
    }

    public createNewItem(request: { title: string, text: string, createdAt: string }) {
        return this.httpClient.post<{ title: string, text: string, createdAt: string }, void>('/news', request)
    }

    public editNewItem(request: { title: string, text: string, createdAt: string, id: string }) {
        return this.httpClient.post<{ title: string, text: string, createdAt: string }, void>('/news/update', request)
    }

    public getNewItem(request: { id: string }) {
        return this.httpClient.get<News>('/news/item', request)
    }

    public createLabReservation(request: CreateLabReservationRequest) {
        return this.httpClient.post<CreateLabReservationRequest, void>('/schedule/create', request)
    }

    public getLabReservation(id: string) {
        return this.httpClient.get<LabReservation>(`/schedule/get/${id}`, id)
    }

    public getAllLabReservations(){
        return this.httpClient.get<LabReservation[]>('/schedule/get')
    }

    public updateLabReservation(request: UpdateLabReservationRequest) {
        return this.httpClient.post<UpdateLabReservationRequest, void>('/schedule/update', request)
    }

    public deleteLabReservation(reservationId: string, userId: string) {
        return this.httpClient.delete<string>(`/schedule/delete/${reservationId}/${userId}`)
    }

    public startVirtualDesktop(userId: string, labWorkId: string) {
        return this.httpClient.post<string, boolean>(`/virtual-desktop/start/${userId}/${labWorkId}`, "")
    }

    public stopVirtualDesktop(userId: string) {
        return this.httpClient.post<string, boolean>(`/virtual-desktop/stop/${userId}`, "")
    }

    public getLabWorks() {
        return this.httpClient.get<LabWork[]>('/lab-works/get')
    }

    public getLabWork(labWorkId: string) {
        return this.httpClient.get<LabWork>(`/lab-works/get/${labWorkId}`)
    }

    public createLabWork(id: string) {
        return this.httpClient.post<{ id: string }, string>('/lab-works/create', {id})
    }

    public getWebsocketUrl(userId: string, protocol: string) {
        return this.httpClient.get<string>(`/virtual-desktop/websocket-url/${userId}/${protocol}`)
    }

    public getStepInstruction(instructionId: string, number: string) {
        return this.httpClient.get<string>(`/lab-work-instruction/get/${instructionId}/${number}`)
    }

    public getInstructionStepAmount(instructionId: string) {
        return this.httpClient.get<number>(`/lab-work-instruction/get-amount/${instructionId}`)
    }

    public getInstructionStepHint(instructionId: string, number: string) {
        return this.httpClient.get<string>(`/lab-work-instruction/get-hint/${instructionId}/${number}`)
    }
    
    public checkIfAnswerCorrect(userId: string, labId: string, number: string) {
        return this.httpClient.get<boolean>(`/lab-work-instruction/check-answer/${userId}/${labId}/${number}`)
    }
}
