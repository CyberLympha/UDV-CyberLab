import {useParams} from "react-router-dom";
import React from "react";

import {apiService} from "../../services";
import {User, UserRole, VmQemuStatusCurrent} from "../../../api";
import {Vm} from "../Vm/Vm";
import {Button} from "../Button/Button";
import {userStore} from "../../stores";
import {getReadableUserRole} from "../../helpers/helpers";
import {MockVm} from "../Vm/MockVm";

import style from "./Lab.module.scss"

export function Lab() {
    const {labId, id} = useParams()
    const [lab, setLab] = React.useState<VmQemuStatusCurrent[]>();
    const [loading, setLoading] = React.useState(false);
    const [error, setError] = React.useState(false);
    const [users, setUsers] = React.useState<User[]>([]);
    const [stopped, setStopped] = React.useState(false);
    const status = lab?.some(vm => vm.status === "stopped");
    // @ts-ignore
    const isCreating = lab?.some(vm => vm.lock === "clone");
    console.log(isCreating)
    const getVmStatus = React.useCallback(async () => {
        setLoading(true);
        const response = await apiService.getLabStatus({id: id!})
        setLoading(false);
        if (response instanceof Error) {
            setError(true);
            return;
        }

        setLab(response);
    }, [id])


    const startLab = async () => {

        setLoading(true);
        setStopped(false)
        const promises: Promise<{ uuid: string } | Error>[] = [];
        lab?.forEach(x => promises.push(apiService.startVm({vmid: x.vmid!})));
        setLoading(false)
    }

    const getStoppedStatus = () => {
        setStopped(true);
    }

    const stopLab = async () => {
        setLoading(true);
        const promises: Promise<{ uuid: string } | Error>[] = [];
        lab?.forEach(x => promises.push(apiService.stopVm({vmid: x.vmid!})));
        await Promise.all(promises);
        getStoppedStatus()
        setLoading(false)
    }

    React.useEffect(() => {
        void getVmStatus();
        const timer = setInterval(getVmStatus, 10000);
        return () => {
            clearInterval(timer);
        }

    }, [getVmStatus])

    React.useEffect(() => {

        const getUsers = async () => {
            const resp = await apiService.getLabsUsers({id: labId!});
            if (resp instanceof Error) {
                return;
            }
            setUsers(resp)
        }
        void getUsers()

    }, [labId])


    const button = (status || stopped) ?
        <Button isLoading={loading || isCreating} onClick={startLab}>Запустить лабораторную работу</Button>
        : <Button isLoading={loading} onClick={stopLab}>Остановить лабораторную работу</Button>


    return <div className={style.container}>
        <div className={style.title}>Лабораторная работа №1</div>
        <div className={style.description}>В данной лабораторной работе студенты проводят анализ уязвимостей в сетевых
            приложениях с целью обеспечения кибербезопасности. Студенты ознакамливаются с различными методами атак,
            включая перехват и изменение данных, инъекции кода и отказ в обслуживании. Они изучают различные инструменты
            и техники, используемые злоумышленниками для эксплуатации уязвимостей. Затем студенты проводят практические
            упражнения, включающие поиск и эксплуатацию уязвимостей в реальных сетевых приложениях.
        </div>
        {error ? <div>Произошла ошибка</div> :
            <div className={style.runLab}>
                {button}
            </div>}
        <div style={{
            display: "flex",
            justifyContent: "center"
        }}>
            {isCreating && <div>Создаём виртуальные машины</div>}
        </div>
        <div style={{display: "flex", justifyContent: "space-evenly"}}>
            {<Vm key={lab?.[0].vmid} status={lab?.[0] as unknown as VmQemuStatusCurrent} stopped={stopped}/>}
            {<MockVm name={"убунту"} key={2} status={"Запущена"} ip={"192.168.2.2"}/>}
            {<MockVm name={"виндоус"} key={3} status={"Запущена"} ip={"192.168.2.1"}/>}
        </div>
        {userStore.user?.role === UserRole.Admin &&
            <div className={style.users}>
                <div>Выполняющие лабораторные работы</div>
                <div className={style.tableHeader}>
                    <div>Имя</div>
                    <div>Фамилия</div>
                    <div>Роль</div>
                </div>
                <div>
                    {users.map(user => {
                        return (
                            <div key={user.id} className={style.tableItem}>
                                <div>{user.firstName}</div>
                                <div>{user.secondName}</div>
                                <div>{getReadableUserRole(user.role)}</div>
                            </div>)
                    })
                    }
                </div>
            </div>
        }
    </div>
}