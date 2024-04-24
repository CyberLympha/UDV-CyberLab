import {useParams} from "react-router-dom";
import React, { useState, useEffect } from "react";

import {apiService} from "../../services";
import {LabWork} from "../../../api";
import {Button} from "../Button/Button";
import {userStore} from "../../stores";
import {AiOutlinePlus} from "react-icons/all";
import style from "./LabWork.module.scss"
import {useToast} from "@chakra-ui/react";

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
        if (!userStore.user?.vmId){
            userStore.setVmId("");
        }
        if (!userStore.user?.vmId){
            setIsLoading(false);
            return;
        }
        const vmIdResponse = await apiService.getVmId(userStore.user?.vmId);
        if (vmIdResponse instanceof Error) {
            setIsLoading(false);
            return;
        }
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
            title: 'Вы успешно закрнчили выполнение лабораторной работы',
            status: "success",
            duration: 5000,
            isClosable: true,
            position: "top"
        })

        setLabStarted(false);
        setIsLoading(false);
    }

    return <div className={style.container}>
        <div className={style.title}>{labWork?.title}</div>
        <div className={style.description}>{labWork?.description}</div>
        <div style={{flexShrink: 0}}>
            <Button rightIcon={<AiOutlinePlus size={"20px"}/>} isLoading={isLoading}
             onClick={()=>startLabWork(userStore.user?.id, labWorkId)}>Начать выполение</Button>
             </div>

             {labStarted && (
                    <Button
                        isLoading={isLoading}
                        onClick={() => stopLabWork(userStore.user?.id)}
                        disabled={!stopButtonActive}
                    >
                        Завершить выполнение
                    </Button>
                )}
                
             {labStarted && (
            <iframe
                src="/libs/noVNC/vnc_lite.html"
                style={{ width: "100%", height: "500px", border: "1px solid #ccc", marginTop: "20px" }}
            ></iframe>
        )}
    </div>
}