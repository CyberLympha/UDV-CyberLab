import React from "react";
import {observer} from "mobx-react-lite";
import {userStore} from "../../stores";

export const UserProfile = observer(() => {
    const user = userStore.user;
    return (
        <div>
            <div>{user?.firstName}</div>
            <div>{user?.secondName}</div>
        </div>
    )

})


