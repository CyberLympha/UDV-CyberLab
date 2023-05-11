import {HttpClient} from "./httpClient";
import {
    ChangeCredentialsRequest,
    CreateVmRequest,
    LoginResponse,
    RegistrationRequest,
    User,
    VmBaseStatusCurrent
} from "../../api";
import {LoginRequest} from "../../api";


export class ApiService {
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this.httpClient = httpClient
    }

    public login(request: LoginRequest) {
        return this.httpClient.post<LoginRequest, LoginResponse>('/auth/login', request)
    }

    public getCurrentUser() {
        return this.httpClient.get<User>('/User');
    }

    public createVm(request: CreateVmRequest) {
        return this.httpClient.post<CreateVmRequest, void>('/vm/create', request)
    }

    public stopVm(request: { vmid: number }) {
        return this.httpClient.get<void>('/vm/stop', request)

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
        return this.httpClient.get<void>('/vm/start', request)

    }

    public getVmStatus(request: { vmid: number }) {
        return this.httpClient.get<VmBaseStatusCurrent>('/vm/status', request)

    }

    public registration(request: RegistrationRequest) {
        return this.httpClient.post<RegistrationRequest, void>('/auth/register', request)
    }

}
