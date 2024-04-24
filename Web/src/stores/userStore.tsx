import {makeAutoObservable} from "mobx"
import {fromPromise,} from "mobx-utils";
import type {IPromiseBasedObservable} from "mobx-utils"

import {User, UserRole} from "../../api";
import {apiService} from "../services";



export class UserStore {
    user?: User | null


    constructor() {
        makeAutoObservable(this)
    }


    setUser = (user: User) => {
        this.user = user;
    }

    setLab = (lab: string) =>{
        this.user!.labs = lab;
    }

    deleteUser = () => {
        this.user = null;

    }

    get isLogined() {
        return !!this.user;
    }

}
