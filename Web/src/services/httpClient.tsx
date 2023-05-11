import axios from "axios";
import type {AxiosInstance, AxiosRequestConfig, AxiosResponse} from "axios"

export class HttpClient {
    private client: AxiosInstance;

    constructor() {
        this.client = axios.create({baseURL: "http://localhost:5221/api/", withCredentials: true,});
        this.client.interceptors.request.use(
            config => {
                config.headers['Authorization'] = `Bearer ${localStorage.getItem('access_token')}`;
                return config;
            },
            error => {
                return Promise.reject(error);
            }
        );
    }

    public get<T>(url: string, params?: any, config?: AxiosRequestConfig) {
        return this.client.get<T>(url, {params: params, ...config}).then(res => res.data).catch(reason => new Error(reason))
    }

    public post<R, T>(url: string, data: R, config?: AxiosRequestConfig<T>) {
        return this.client.post<R, AxiosResponse<T>>(url, data, config).then(res => res.data).catch(reason => new Error(reason))
    }

    public put<R, T>(url: string, data: T, config?: AxiosRequestConfig<T>) {
        return this.client.put<R, AxiosResponse<T>>(url, data, config).then(res => res.data).catch(reason => new Error(reason))
    }

    public delete<T>(url: string, config?: AxiosRequestConfig<T>) {
        return this.client.delete(url, config).then(res => res.data).catch(reason => new Error(reason))
    }
}
