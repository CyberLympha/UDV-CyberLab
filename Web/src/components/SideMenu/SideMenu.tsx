import {NavLink, Outlet} from "react-router-dom";
import {observer} from "mobx-react-lite";

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
    {to: "/news", name: "Новости", roles: [UserRole.Anon, UserRole.User]},
    {to: "/vms", name: "Виртуалки", roles: [UserRole.User]},

]

export const SideMenu = observer(() => {
    const user = userStore.user;
    const renderNavLinks = (navLink: NavLink) => {

        if (!navLink.roles.includes(user?.role ?? UserRole.Anon)) return;
        return (<NavLink style={({isActive}) => {
            return {
                fontWeight: isActive ? "bold" : "",
            };
        }} className={style.link} to={navLink.to}>{navLink.name}</NavLink>)
    }

    const logout = () => {
        localStorage.clear();
        userStore.deleteUser()
    };

    return (
        <>
            <div className={style.sidePageWrapper}>
                <UserProfile/>
                {navLinks.map(link => renderNavLinks(link))}
                <div style={{flex: 1}}></div>
                {userStore.user && <Button onClick={logout}>Выйти</Button>}
            </div>
            <div style={{
                width: "100%",
                margin: "10px",
                marginLeft: 0,
                borderRadius: "24px",
                backgroundColor: "#f6f6f6",
                padding: "16px"
            }}>
                <Outlet/>
            </div>
        </>
    )
})
