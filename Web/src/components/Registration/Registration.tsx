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
    const [role, setRole] = React.useState<string>(`User`);
    const [loading, setLoading] = React.useState(false);
    const navigate = useNavigate();

    const roles = [
        { value: 'User', label: `👨‍🎓 Ученик` },
        { value: 'Teacher', label: `👨‍🏫 Учитель` }
    ];

    const handleClickPrimaryButton = async () => {
        setLoading(true);
        const response = await apiService.registration({email, password, firstName, secondName, role});

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

    const chengeSelect = (value : any) => {
        setRole(value);
    };

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
                <body>
                    <div className={"selectdiv"}>
                        <label>
                            <select onChange={(e) => chengeSelect(e.target.value)}>                            
                                {roles.map((type, index) => (
                                    <option key={index} value={type.value}>
                                        {type.label}
                                    </option>
                                ))}
                            </select>
                        </label>
                    </div>      
                </body>
                <Button isLoading={loading} onClick={handleClickPrimaryButton} colorScheme={"blue"}
                        children={"Зарегистрироваться"}/>
            </VStack>
        </Center>
    );
}