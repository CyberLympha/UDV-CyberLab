import React from "react";
import {observer} from "mobx-react-lite";
import Moment from "react-moment";

import {userStore} from "../../stores";
import {getReadableUserRole} from "../../helpers/helpers";

import style from "./UserProfile.module.scss"

export const UserProfile = observer(() => {
    const [time, setTime] = React.useState(new Date())

    React.useEffect(() => {
        setInterval(() => setTime(new Date()), 1000)
    }, [])

    const user = userStore.user;

    return (
        <div className={style.wrapper}>
            <Moment className={style.time} format={"DD/MM/yyyy HH:mm"}>{time}</Moment>
            <div className={style.userProfile}>
                <div className={style.userItem}>{user?.firstName}</div>
                <div className={style.userItem}>{user?.secondName}</div>
                <div className={style.userItem}>{getReadableUserRole(user?.role)}</div>
            </div>
        </div>
    )
})


