import React from "react";
import {
    Input,
    Modal,
    ModalBody, ModalCloseButton,
    ModalContent,
    ModalFooter,
    ModalHeader,
    ModalOverlay, VStack,
} from "@chakra-ui/react";

import {apiService} from "../../services";
import {Button} from "../Button/Button"

interface RegistrationFormProps {
    onClose: () => void
    isOpen: boolean
}

export function RegistrationForm({onClose, isOpen}: RegistrationFormProps) {
    const [email, setEmail] = React.useState<string>('')
    const [password, setPassword] = React.useState<string>('')
    const [firstName, setFirstName] = React.useState<string>('')
    const [secondName, setSecondName] = React.useState<string>('')
    const [loading, setLoading] = React.useState(false)
    const handleClickPrimaryButton = async () => {
        setLoading(true);
        const response = await apiService.registration({email, password, firstName, secondName});
        if (response instanceof Error) {
            setLoading(false)
            return;

        }
        setLoading(false)
    }


    return (
        <Modal isOpen={isOpen} onClose={onClose} blockScrollOnMount>
            <ModalOverlay/>
            <ModalContent>
                <ModalHeader>Регистрация</ModalHeader>
                <ModalCloseButton/>
                <ModalBody>
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
                    </VStack>
                </ModalBody>
                <ModalFooter>
                    <Button onClick={handleClickPrimaryButton} isLoading={loading}>
                        Зарегистрироваться
                    </Button>
                </ModalFooter>
            </ModalContent>
        </Modal>
    );
}
