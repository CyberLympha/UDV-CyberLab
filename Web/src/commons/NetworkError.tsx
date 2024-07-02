import type {AxiosResponse} from "axios"


export class NetworkError extends Error {
    public readonly code?: number;

    constructor(error: AxiosResponse) {

        super();
        this.name = "Network Error";
        this.code = error.status;
    }
}
