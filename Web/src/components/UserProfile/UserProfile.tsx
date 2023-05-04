import {observer} from "mobx-react-lite";
import {userStore} from "../../stores";
import {Button} from "../Button/Button";
import {LoginForm} from "../AuthForm/LoginForm";
import React from "react";
import {RegistrationForm} from "../AuthForm/RegistrationForm";

export const UserProfile = observer(() => {
    const user = userStore.user;
    const [isOpenLogin, setIsOpenLogin] = React.useState(false)
    const [isOpenRegister, setIsOpenRegister] = React.useState(false)
    const isLogined = !!userStore.user;
    return (
        <div>
            <div>{user?.firstName}</div>
            <div>{user?.secondName}</div>
            {!isLogined &&
                <>
                    <Button onClick={() => setIsOpenLogin(true)}>Войти</Button>
                    {isOpenLogin && <LoginForm isOpen={isOpenLogin} onClose={() => setIsOpenLogin(false)}/>}
                    <Button onClick={() => setIsOpenRegister(true)}>Регистрация</Button>
                    {isOpenRegister &&
                        <RegistrationForm isOpen={isOpenRegister} onClose={() => setIsOpenRegister(false)}/>}

                </>}
        </div>
    )

})


