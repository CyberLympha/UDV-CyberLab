import {useParams} from "react-router-dom";
import React from "react";
import {apiService} from "../services";
import {VmBaseStatusCurrent} from "../../api";
import {Button} from "./Button/Button";
import {Input} from "@chakra-ui/react";
import {getReadableVmStatus} from "../helpers/helpers";

export function Vm() {
    const {vmid} = useParams()
    const [vm, setVm] = React.useState<VmBaseStatusCurrent>();
    const [loading, setLoading] = React.useState(false);
    const [username, setUsername] = React.useState("");
    const [password, setPassword] = React.useState("");
    const [ssh, setSsh] = React.useState("")

    const startVm = async () => {
        setLoading(true);
        await apiService.startVm({vmid: Number(vmid!)});
        setLoading(false)
    }

    const stopVm = async () => {
        setLoading(true);
        await apiService.stopVm({vmid: Number(vmid!)});
        setLoading(false)
        void getVmStatus()
    }

    const changePassword = async () => {
        setLoading(true);
        await apiService.setPassword(Number(vmid!), username, password, ssh);
        setLoading(false)
        void getVmStatus()
    }

    const getVmStatus = React.useCallback(async () => {
        const response = await apiService.getVmStatus({vmid: Number(vmid)})
        if (!(response instanceof Error)) {
            console.log(response)

            setVm(response);
        }
    }, [vmid])

    React.useEffect(() => {
        void getVmStatus();
        const timer = setInterval(getVmStatus, 5000);
        return () => {
            clearInterval(timer);
        }

    }, [getVmStatus])

    // @ts-ignore
    const vmStatus = vm?.lock! === "clone" ? "Создаётся" : getReadableVmStatus(vm?.status);

    return <div>
        {vm?.status === "running" ? <Button isLoading={loading} onClick={stopVm} children={"Остановить машину"}/>
            : <Button isLoading={loading} onClick={startVm} children={"Запустить машину"}/>}
        <div style={{fontSize: "24px"}}>
            Уникальный номер машины: {vmid}< br/>
            Имя машины:{vm?.name}< br/>
            Статус: {vmStatus}< br/>

        </div>
        <Input style={{width: "250px"}} placeholder={"Имя"} value={username}
               onChange={(e) => setUsername(e.target.value)}/>
        <Input style={{width: "250px"}} placeholder={"Пароль"} value={password}
               onChange={(e) => setPassword(e.target.value)}/>
        <Input style={{width: "250px"}} placeholder={"SSH ключ"} value={ssh}
               onChange={(e) => setSsh(e.target.value)}/>
        <Button isLoading={loading} onClick={changePassword} children={"Поменять данные"}/>
    </div>
}