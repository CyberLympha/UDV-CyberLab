import {HttpClient} from "./httpClient";
import {ApiService} from "./apiService";

export const httpClient = new HttpClient();


export const apiService = new ApiService(httpClient);


export {HttpClient}
