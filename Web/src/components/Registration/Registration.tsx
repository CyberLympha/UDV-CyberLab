import {Center, Input, VStack} from "@chakra-ui/react";
import React from "react";
import {useNavigate} from "react-router-dom";

import {apiService} from "../../services";
import {Button} from "../Button/Button";

export function Registration() {
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
        navigate("/login");
    }


    return (
        <Center w={"100%"}>
            <VStack>
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