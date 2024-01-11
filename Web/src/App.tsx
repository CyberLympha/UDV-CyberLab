import React from 'react'
import {Spinner} from "@chakra-ui/react";
import {Route, Routes} from "react-router-dom";
import {observer} from "mobx-react-lite";


import {apiService} from "./services";
import {userStore} from "./stores";
import {Login} from "./components/Login";
import {AuthApp} from "./components/Vm/AuthApp";
import {Registration} from "./components/Registration";
import style from "./App.module.scss"


export const App = observer(() => {
    const [loading, setLoading] = React.useState(false)


    React.useEffect(() => {
        setLoading(true);

        const fetchUser = async () => {
            const response = await apiService.getCurrentUser();

            if (response instanceof Error) {
                setLoading(false)
                return;
            }
            userStore.setUser(response);
            setLoading(false);
        }

        void fetchUser()
    }, [])

    if (loading) return <Spinner className={style.spinner} size="xl" speed="1s" thickness="4px"/>

    if (userStore.isLogined) {
        return (
            <AuthApp/>
        )
    }

    return (
        <Routes>
            <Route path={"/login"} element={<Login/>}/>
            <Route path={"/registration"} element={<Registration/>}/>
            <Route path="*" element={<Login/>}/>
        </Routes>
    )

});

App.displayName = "App"