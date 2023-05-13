import {Navigate, Route, Routes} from "react-router-dom";
import React from "react";
import {observer} from "mobx-react-lite";

import {Login} from "./Login";
import {Registration} from "./Registration";
import {SideMenu} from "./SideMenu/SideMenu";
import {News} from "./News/News";
import {Vms} from "./Vms/Vms";
import {NewVm} from "./NewVm";
import {userStore} from "../stores";
import {Vm} from "./Vm";


const Protected = observer(({children}: { children: JSX.Element }) => {
    console.log(userStore.isLogined)
    if (userStore.isLogined) {
        //return <Navigate to="/login" replace/>;
    }
    return children;
});

Protected.displayName = "PrivateRoute"

export function AppRoutes() {
    return (
        <Routes>
            <Route element={<Protected><SideMenu/></Protected>}>
                <Route path={"/news"} element={<Protected><News/></Protected>}/>
                <Route path={"/vms"} element={<Protected><Vms/></Protected>}/>
                <Route path={"/vms/new"} element={<Protected><NewVm/></Protected>}/>
                <Route path={"/vms/:vmid"} element={<Protected><Vm/></Protected>}/>
                <Route path="*" element={<Navigate to={"/news"} replace/>}/>
            </Route>

        </Routes>
    );
}