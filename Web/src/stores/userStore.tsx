import {action, makeObservable, observable} from "mobx"
import {User} from "../../api";

export class UserStore {
    user: User | null = null


    constructor() {
        makeObservable(this, {
            setUser: action,
            deleteUser: action,
            user: observable,
        })
    }

    addVm(vmid: number) {
        this.user?.vms.push(vmid)
    }

    setUser(user: User) {
        this.user = user;
    }

    deleteUser() {
        this.user = null;
    }

}
