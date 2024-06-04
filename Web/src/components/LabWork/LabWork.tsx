import {useParams, useNavigate, useLocation} from "react-router-dom";
import React, { useState, useEffect } from "react";

import {useToast} from "@chakra-ui/react";
import {AiOutlinePlus} from "react-icons/all";
import {apiService} from "../../services";
import {LabWork} from "../../../api";
import {Button} from "../Button/Button";
import {userStore} from "../../stores";
import style from "./LabWork.module.scss"
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
    const navigate = useNavigate();
    const location = useLocation();

    window.onbeforeunload = function (e) {
        e = e || window.event;
    
        if (e) {
            e.returnValue = 'Sure?';
        }

        return 'Sure?';
    };

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

    useEffect(() => {
        if (!labStarted) return;

        const handleRouteChange = (e: Event) => {
            if (labStarted) {
                e.preventDefault();
                if (window.confirm("Вы точно хотите покинуть страницу не завершив лабораторную работу? Лабораторная работа будет завершена автоматически.")) {
                    stopLabWork(userStore.user?.id).then(() => {
                        navigate(location.pathname);
                    });
                }
            }
        };

        window.addEventListener("beforeunload", handleRouteChange, {capture: true});
        window.addEventListener("popstate", handleRouteChange, {capture: true});
        window.addEventListener("unload", handleRouteChange, {capture: true});

        return () => {
            window.removeEventListener("beforeunload", handleRouteChange, {capture: true});
            window.removeEventListener("popstate", handleRouteChange, {capture: true});
            window.removeEventListener("unload", handleRouteChange, {capture: true});
        };
    }, [labStarted, navigate, location]);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    const startLabWork = async (userId: string | undefined, labWorkId: string | undefined) => {
        setIsLoading(true);
        if (!userId){
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
        const response = await apiService.startVirtualDesktop(userId, labWorkId!);
        if (response instanceof Error) {
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

        const responseWebsocketUrl = await apiService.getWebsocketUrl(userId, labWorkId!);
        if (responseWebsocketUrl instanceof Error) {
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

                        <div className={style.virtualDesktop}>
                            <iframe className={style.virtualDesktopScreen} src="/libs/noVNC/vnc_my.html"></iframe>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}