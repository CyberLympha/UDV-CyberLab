import {Center, HStack, Input, useToast, VStack} from "@chakra-ui/react";
import React from "react";
import {useNavigate} from "react-router-dom";

import {Button} from "../Button/Button";
import {apiService} from "../../services";
import {userStore} from "../../stores";

import style from "./Login.module.scss"

export function Login() {
    const [email, setEmail] = React.useState<string>('');
    const [password, setPassword] = React.useState<string>('');
    const [loginLoading, setLoginLoading] = React.useState(false);
    const [loading, setLoading] = React.useState(false);
    const toast = useToast()

    const navigate = useNavigate();

    const handleClickLogin = async () => {
        setLoginLoading(true);
        const response = await apiService.login({email, password});

        if (response instanceof Error) {
            if (response.code === 403) {
                toast({
                    title: 'Ваш аккаунт пока не подтвердили',
                    status: "info",
                    duration: 9000,
                    isClosable: true,
                    position: "top"
                })
            }
        } else {
            userStore.setUser(response.user)
            localStorage.setItem("access_token", response.token);
            navigate("/");

        }
        setLoginLoading(false)
    }


    return (
        <Center width={"100%"}>
            <div className={style.container}>
                <VStack>
                    <Input
                        width={"350px"}
                        onChange={e => setEmail(e.target.value)}
                        value={email}
                        type="email"
                        placeholder="Логин"
                        focusBorderColor={"black"}
                    />
                    <Input
                        width={"350px"}
                        onChange={e => setPassword(e.target.value)}
                        value={password}
                        type="password"
                        placeholder="Пароль"
                        focusBorderColor={"black"}
                    />
                    <HStack>
                        <Button onClick={() => navigate("/registration")} isLoading={loading} isDisabled={loginLoading}>
                            Регистрация
                        </Button>
                        <Button colorScheme={"blue"} onClick={handleClickLogin} isLoading={loginLoading}
                                isDisabled={loading}>
                            Войти
                        </Button>
                    </HStack>
                </VStack>
            </div>
        </Center>
    )
}