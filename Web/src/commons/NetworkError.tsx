import {AxiosError,} from "axios";
import type {AxiosResponse} from "axios"
import {Exception} from "../../api";


export class NetworkError extends Error {
    public readonly code?: number;

    constructor(error: AxiosResponse) {

        super();
        this.name = "Network Error";
        this.code = error.status;
    }
}
