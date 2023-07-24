import {Checkbox} from "@chakra-ui/react";
import React from "react";

import {User} from "../../../api";
import {apiService} from "../../services";
import {getReadableUserRole} from "../../helpers/helpers";
import {Button} from "../Button/Button";

import style from "./Admin.module.scss"

export function Admin() {
    const [users, setUsers] = React.useState<User[]>([])
    const [toApprove, setToApprove] = React.useState<string[]>([])

    const getUsers = async () => {
        const response = await apiService.getUsers();

        if (response instanceof Error) {
            return;
        }
        setUsers(response);
    }

    React.useEffect(() => {
        void getUsers();
    }, [])

    const approveUsers = async () => {
        const response = await apiService.approveUsers(toApprove)
        if (response instanceof Error) {
            return;
        }
        void getUsers()
    }

    return <div className={style.wrapper}>
        <Button className={style.approveButton} isDisabled={!toApprove.length} onClick={approveUsers}>Подтвердить
            учетную запись</Button>
        <div className={style.table}>
            <div className={style.header}>
                <span>Имя</span>
                <span>Фамилия</span>
                <span>Роль</span>
                <span>Подтвержденынй</span>
                <span></span>
            </div>
            <div>
                {users.map(user => {
                    return (
                        <div key={user.id} className={style.item}>
                            <div>{user.firstName}</div>
                            <div>{user.secondName}</div>
                            <div>{getReadableUserRole(user.role)}</div>
                            <div
                                style={{fontWeight: !user.isApproved ? "bold" : "normal"}}>{user.isApproved ? "Да" : "Нет"}</div>
                            <div>{!user.isApproved && <Checkbox
                                onChange={e => e.target.checked ?
                                    setToApprove([...toApprove, user.id]) :
                                    setToApprove(toApprove.filter(id => id != user.id))}/>}
                            </div>
                        </div>
                    )
                })}

            </div>
        </div>
    </div>

}