import React from "react";
import {Button} from "../Button/Button";
import {Radio, RadioGroup, Stack, useToast} from "@chakra-ui/react";

import style from "./Tests.module.scss"

export function Tests() {
    const [loading, setLoading] = React.useState(false);
    const [answer, setAnswer] = React.useState(0);
    const [rightAnswer, setRightAnswer] = React.useState(true);
    const toast = useToast();

    const getAnswer = () => {
        setLoading(true);
        setTimeout(() => {
            setLoading(false);
            if (answer === 4) {
                setRightAnswer(true)
                toast({
                    title: 'Правильно!',
                    status: 'success',
                    position:"top",
                    duration: 3000,
                    isClosable: true,
                })
            } else {
                setRightAnswer(false)
                toast({
                    title: 'Попробуйте ещё раз!',
                    status: 'error',
                    position:"top",
                    duration: 3000,
                    isClosable: true,
                })
            }
        }, 1000)
    };

    return (
        <div className={style.wrapper}>
            <div style={{width: "420px", fontSize: "24px"}}>Какой протокол уровня приложений в основном используется
                для передачи файла
                между клиентом и сервером?
            </div>
            <RadioGroup>
                <Stack>
                    <Radio size='lg' value='1' isInvalid={!rightAnswer} colorScheme={'blue'} onChange={() => setAnswer(1)}>
                        HTML
                    </Radio>
                    <Radio size='lg' value='2' isInvalid={!rightAnswer} colorScheme={'blue'} onChange={() => setAnswer(2)}>
                        HTTP
                    </Radio>
                    <Radio size='lg' value='3'  isInvalid={!rightAnswer} colorScheme={'blue'} onChange={() => setAnswer(3)}>
                        Telnet
                    </Radio>
                    <Radio size='lg' value='4' isInvalid={!rightAnswer} colorScheme={ 'blue'}
                           onChange={() => setAnswer(4)}>
                        FTP
                    </Radio>
                </Stack>
            </RadioGroup>
            <div>
                <Button onClick={() => getAnswer()} disabled={answer === 0} isLoading={loading}
                        style={{marginTop: "8px"}}
                        size={"lg"}>Ответить</Button>
            </div>
        </div>
    )
}
