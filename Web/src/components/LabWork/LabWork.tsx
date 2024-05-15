import {useParams} from "react-router-dom";
import React, { useState, useEffect } from "react";

import {apiService} from "../../services";
import {LabWork} from "../../../api";
import {Button} from "../Button/Button";
import {userStore} from "../../stores";
import {AiOutlinePlus} from "react-icons/all";
import style from "./LabWork.module.scss"
import {useToast} from "@chakra-ui/react";
import {Instruction} from "./Instruction/Instruction";

export function LabWorkPage() {
    const [labStarted, setLabStarted] = useState<boolean>(false);
    const [isLoading, setIsLoading] = useState(false);
    const {labWorkId, userId} = useParams()
    const [labWork, setLabWork] = useState<LabWork | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null); 
    const [stopButtonActive, setStopButtonActive] = useState<boolean>(false);
    const toast = useToast();

    useEffect(() => {
        if (labWorkId) {
            const fetchLabWork = async () => {
                try {
                    setLoading(true);
                    const response = await apiService.getLabWork(labWorkId);
                    
                    if (response instanceof Error) {
                        setError("Ошибка при загрузке данных о лабораторной работе");
                        return;
                    }
                    setLabWork(response);
                    setLoading(false);
                } catch (error) {
                    
                    setError("Ошибка при загрузке данных о лабораторной работе");
                    setLoading(false); 
                }
            };
            fetchLabWork();
        }
    }, [labWorkId]);

    useEffect(() => {
        setStopButtonActive(labStarted);
    }, [labStarted]);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    const startLabWork = async (userId: string | undefined, labWorkId: string | undefined) => {
        setIsLoading(true);
        if (!userId){
            setIsLoading(false);
            return;
        }
        const response = await apiService.startVirtualDesktop(userId, labWorkId!);
        if (response instanceof Error) {
            setIsLoading(false);
            return;
        }
        if (!response){
            toast({
                title: 'Не удалось начать лабораторную работу, повторите попытку',
                status: "error",
                duration: 5000,
                isClosable: true,
                position: "top"
            })
            setLabStarted(false);
            setIsLoading(false);
            return;
        }
        toast({
            title: 'Вы успешно начали выполнение лабораторной работы',
            status: "success",
            duration: 5000,
            isClosable: true,
            position: "top"
        })

        const responseWebsocketUrl = await apiService.getWebsocketUrl(userId, labWorkId!);
        if (responseWebsocketUrl instanceof Error) {
            setIsLoading(false);
            //TODO
            return;
        }
        localStorage.setItem('websocketUrl', responseWebsocketUrl);
        setIsLoading(false);
        setLabStarted(true);
    }

    const stopLabWork = async (userId: string | undefined) => {
        setIsLoading(true);
        if (!userId){
            return;
        }
        const response = await apiService.stopVirtualDesktop(userId);
        if (response instanceof Error) {
            setIsLoading(false);
            return;
        }
        if (!response){
            toast({
                title: 'Не удалось закончить лабораторную работу, повторите попытку',
                status: "error",
                duration: 5000,
                isClosable: true,
                position: "top"
            })
            setLabStarted(false);
            setIsLoading(false);
            return;
        }
        toast({
            title: 'Вы успешно закончили выполнение лабораторной работы',
            status: "success",
            duration: 5000,
            isClosable: true,
            position: "top"
        })

        setLabStarted(false);
        setIsLoading(false);
    }

    return (
        <div className={style.labWorkContainer}>
            <div className={style.virtualDesktopContainer}>
                <div className={style.title}>{labWork?.title}</div>
                <div className={style.description}>{labWork?.description}</div>

                {!labStarted &&(
                    <div style={{flexShrink: 0}}>
                    <Button rightIcon={<AiOutlinePlus size={"20px"}/>} isLoading={isLoading}
                    onClick={()=>startLabWork(userStore.user?.id, labWorkId)}>Начать выполнение</Button>
                </div>
                )}
                
                {labStarted && (
                    <div className={style.contentContainer}>
                        <div className={style.instruction}>
                            <Instruction labWork={labWork!} stopLabWork={stopLabWork}/>
                        </div>

                        <div className={style.virtualDesktop}>
                            <iframe
                                src="/libs/noVNC/vnc_lite.html"
                                style={{ width: "100%", height: "100%", border: "1px solid #ccc"}}
                            ></iframe>
                        </div>
                    </div>
                )}

                {labStarted && (
                    <Button
                        isLoading={isLoading}
                        onClick={() => stopLabWork(userStore.user?.id)}
                        disabled={!stopButtonActive}> 
                        Завершить выполнение
                    </Button>
                    )
                }

            </div>
        </div>
    );
}