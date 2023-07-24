import {NavLink, Outlet} from "react-router-dom";
import {observer} from "mobx-react-lite";
import React from "react";
import {VStack} from "@chakra-ui/react";
import {ImExit} from "react-icons/all";

import {UserProfile} from "../UserProfile/UserProfile";
import {userStore} from "../../stores";
import {Button} from "../Button/Button";
import {UserRole} from "../../../api";

import style from "./SideMenu.module.scss"


interface NavLink {
    to: string;
    name: string;
    roles: UserRole[]
}

const navLinks: NavLink[] = [
    {to: "/news", name: "Новости", roles: [UserRole.User, UserRole.Admin]},
    {to: "/labs", name: "Лабораторные работы", roles: [UserRole.User, UserRole.Admin]},
    {to: "/tests", name: "Тесты", roles: [UserRole.User, UserRole.Admin]},
    {to: "/ratings", name: "Оценки", roles: [UserRole.User, UserRole.Admin]},
    {to: "/ctf", name: "Соревнования", roles: [UserRole.User, UserRole.Admin]},
    {to: "/admin", name: "Администрирование", roles: [UserRole.Admin]},

]

export const SideMenu = observer(() => {
    const user = userStore.user;
    const [loading, setLoading] = React.useState(false);

    const renderNavLinks = (navLink: NavLink) => {

        if (!navLink.roles.includes(user?.role ?? UserRole.Anon)) return;
        return (<NavLink style={({isActive}) => {
            return {
                fontWeight: isActive ? "bold" : "",
                backgroundColor: isActive ? "#C8C8C8" : ""
            };
        }} className={style.link} to={navLink.to}>{navLink.name}</NavLink>)
    }

    const logout = () => {
        setLoading(true);
        localStorage.clear();
        userStore.deleteUser();
        setLoading(false);
    };
    return (
        <>
            <div className={style.sidePageWrapper}>
                <VStack>
                    {navLinks.map(link => renderNavLinks(link))}
                </VStack>
                <div style={{flex: 1}}></div>
                {userStore.user && <Button leftIcon={<ImExit/>} onClick={logout}>Выйти</Button>}
            </div>
            <div style={{
                width: "100%",
                borderRadius: "24px",
                minHeight: "100vh",
                height: "fit-content",
                display: "flex",
                flexDirection: "column",
            }}>
                <div className={style.header}>
                    <UserProfile/>
                </div>
                <Outlet/>
            </div>
        </>
    )
})
