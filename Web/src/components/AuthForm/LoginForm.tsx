import React from "react";
import {
    Input,
    Modal,
    ModalBody, ModalCloseButton,
    ModalContent,
    ModalFooter,
    ModalHeader,
    ModalOverlay,
    VStack
} from "@chakra-ui/react";

import {apiService} from "../../services";
import {Button} from "../Button/Button";
import {userStore} from "../../stores";

interface LoginFormProps {
    onClose: () => void
    isOpen: boolean
}

export function LoginForm({onClose, isOpen}: LoginFormProps) {
    const [email, setEmail] = React.useState<string>('')
    const [password, setPassword] = React.useState<string>('')
    const [loading, setLoading] = React.useState(false)
    const handleClickPrimaryButton = async () => {
        setLoading(true);
        const response = await apiService.login({email, password});

        if (response instanceof Error) {
            setLoading(false);
            return;
        } else {
            userStore.setUser(response.user)
            localStorage.setItem("access_token", response.token)
            onClose()
        }
        setLoading(false)
    }


    return (
        <Modal isOpen={isOpen} onClose={onClose} blockScrollOnMount>
            <ModalOverlay/>
            <ModalContent>
                <ModalHeader>Войти</ModalHeader>
                <ModalCloseButton/>
                <ModalBody>
                    <VStack>
                        <Input
                            width={"350px"}
                            onChange={e => setEmail(e.target.value)}
                            value={email}
                            type="email"
                            placeholder='Почта'
                        />
                        <Input
                            width={"350px"}
                            onChange={e => setPassword(e.target.value)}
                            value={password}
                            type="password"
                            placeholder='Пароль'
                        />
                    </VStack>
                </ModalBody>
                <ModalFooter>
                    <Button onClick={handleClickPrimaryButton} isLoading={loading}>
                        Логин
                    </Button>
                </ModalFooter>
            </ModalContent>
        </Modal>
    );
}
