import React from 'react'
import {Spinner} from "@chakra-ui/react";
import {Route, Routes, Navigate} from "react-router-dom";

import {SideMenu} from "./components/SideMenu/SideMenu";
import {News} from "./components/News/News";
import {apiService} from "./services";
import {userStore} from "./stores";
import {Vms} from "./components/Vms/Vms";
import {NewVm} from "./components/NewVm/NewVm";
import {Vm} from "./components/Vm";


import style from "./App.module.scss"

export function App() {
    const [loading, setLoading] = React.useState(false)

    React.useEffect(() => {
        setLoading(true);

        const fetchUser = async () => {
            const response = await apiService.getCurrentUser();

            if (response instanceof Error) {
                setLoading(false)
                return;
            }
            userStore.setUser(response)
            setLoading(false)
        }

        void fetchUser()
    }, [])

    if (loading) return <Spinner className={style.spinner} size="xl" speed="1s" thickness="4px"/>

    return (
        <Routes>
            <Route element={<SideMenu/>}>
                <Route path={"/news"} element={<News/>}/>
                <Route path={"/vms"} element={<Vms/>}/>
                <Route path={"/vms/new"} element={<NewVm/>}/>
                <Route path={"/vms/:vmid"} element={<Vm/>}/>
                <Route
                    path="*"
                    element={<Navigate to="/news" replace/>}
                />
            </Route>

        </Routes>
    )
}

