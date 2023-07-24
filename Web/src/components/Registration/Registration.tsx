import {Center, Input, useToast, VStack} from "@chakra-ui/react";
import React from "react";
import {useNavigate} from "react-router-dom";

import {apiService} from "../../services";
import {Button} from "../Button/Button";

import style from "./Registration.module.scss"

export function Registration() {
    const toast = useToast();
    const [email, setEmail] = React.useState<string>('');
    const [password, setPassword] = React.useState<string>('');
    const [firstName, setFirstName] = React.useState<string>('');
    const [secondName, setSecondName] = React.useState<string>('');
    const [loading, setLoading] = React.useState(false);
    const navigate = useNavigate();

    const handleClickPrimaryButton = async () => {
        setLoading(true);

        const response = await apiService.registration({email, password, firstName, secondName});

        if (response instanceof Error) {
            setLoading(false);
            return;
        }

        setLoading(false);
        toast({
            title: 'Регистрация прошла успешно',
            description: "Дождитесь, пока вашу заявку одобрит администратор",
            status: "success",
            duration: 9000,
            isClosable: true,
            position: "top"
        })
        navigate("/login");
    }


    return (
        <Center w={"100%"}>
            <VStack className={style.wrapper}>
                <Input
                    width={"350px"}
                    onChange={e => setFirstName(e.target.value)}
                    value={firstName}
                    type="text"
                    placeholder='Имя'
                />
                <Input
                    width={"350px"}
                    onChange={e => setSecondName(e.target.value)}
                    value={secondName}
                    type="text"
                    placeholder='Фамилия'
                />
                <Input
                    width={"350px"}
                    onChange={e => setEmail(e.target.value)}
                    value={email}
                    type="email"
                    placeholder='Логин'
                />
                <Input
                    width={"350px"}
                    onChange={e => setPassword(e.target.value)}
                    value={password}
                    type="password"
                    placeholder='Пароль'
                />
                <Button isLoading={loading} onClick={handleClickPrimaryButton} colorScheme={"blue"}
                        children={"Зарегистрироваться"}/>
            </VStack>
        </Center>
    );
}