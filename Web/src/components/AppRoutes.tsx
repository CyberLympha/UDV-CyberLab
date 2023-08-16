import {Navigate, Route, Routes} from "react-router-dom";
import React from "react";
import {observer} from "mobx-react-lite";

import {userStore} from "../stores";

import {SideMenu} from "./SideMenu/SideMenu";
import {News} from "./News/News";
import {Labs} from "./Labs/Labs";
import {Lab} from "./Lab/Lab";
import { NewsAdd } from "./NewsAdd/NewsAdd";
import {NewsEdit} from "./NewsEdit/NewsEdit";
import {Admin} from "./Admin/Admin";
import {Tests} from "./Tests/Tests";


const Protected = observer(({children}: { children: JSX.Element }) => {
    if (!userStore.isLogined) {
        return <Navigate to="/login" replace/>;
    }
    return children;
});

Protected.displayName = "PrivateRoute"

export function AppRoutes() {
    return (
        <Routes>
            <Route element={<Protected><SideMenu/></Protected>}>
                <Route path={"/news"} element={<Protected><News/></Protected>}/>
                <Route path={"/news/add"} element={<Protected><NewsAdd/></Protected>}/>
                <Route path={"/news/:id/edit"} element={<Protected><NewsEdit/></Protected>}/>
                <Route path={"/labs"} element={<Protected><Labs/></Protected>}/>
                <Route path={"/tests"} element={<Protected><Tests/></Protected>}/>
                <Route path={"/labs/:labId/:id"} element={<Protected><Lab/></Protected>}/>
                <Route path={"/admin"} element={<Protected><Admin/></Protected>}/>
                <Route path="*" element={<Navigate to={"/news"} replace/>}/>
            </Route>

        </Routes>
    );
}