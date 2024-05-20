import {
    ChangeCredentialsRequest,
    RegistrationRequest,
    User,
    LoginRequest,
    News, Test, Lab, VmQemuAgentNetworkGetInterfaces,
    LabReservation, CreateLabReservationRequest, UpdateLabReservationRequest, Question
} from "../../api";

import {HttpClient} from "./httpClient";


export class ApiService {
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this.httpClient = httpClient
    }

    public getTests() {
        return this.httpClient.get<Test[]>('/tests')
    }

    public getTest(id : string) {
        return this.httpClient.get<Test>(`/tests/${id}`)
    }

    public postTest(request: {name: string, description: string, questions: string[]}) {
        return this.httpClient.post<{
            name: string,
            description: string,
            questions: string[]
        }, void>('/tests', request)
    }

    public postQuestion(request: {text: string, description: string,
        questionType: string, correctAnswer: string, questionData: string[]}) {
        return this.httpClient.post<{
            text: string,
            description: string,
            questionType: string,
            questionData: string[],
            correctAnswer: string
        }, void>('/questions', request)
    }

    public getQuestions() {
        return this.httpClient.get<Question[]>(`/Questions`)
    }

    public getQuestion(questionId: string) {
        return this.httpClient.get<Question>(`/Questions/${questionId}`)
    }

    public deleteQuestion(questionId: string) {
        return this.httpClient.delete<string>(`/Questions/delete/${questionId}`)
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
}
