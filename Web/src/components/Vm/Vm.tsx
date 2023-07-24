import React from "react";
import {Skeleton} from "@chakra-ui/react";

import {VmQemuStatusCurrent} from "../../../api";
import {apiService} from "../../services";
import {getReadableVmStatus} from "../../helpers/helpers";

import style from "./Vm.module.scss"

export interface VmProps {
    status: VmQemuStatusCurrent,
    stopped: boolean;

}

export function Vm({status, stopped}: VmProps) {
    const [loading, setLoading] = React.useState(true);
    const [ip, setIp] = React.useState<string | null | undefined>();


    const getVmIp = React.useCallback(async () => {
        const response = await apiService.getVmIp({vmid: `${status.vmid!}`});
        if (response instanceof Error) {
            setIp(null);
            return;
        }

        // @ts-ignore
        const ip = response.result?.[1]?.["ip-addresses"]?.[0]?.["ip-address"];
        setIp(ip)
        setLoading(false);

    }, [status.vmid])


    React.useEffect(() => {
        const getIp = async () => {
            setLoading(true);
            await getVmIp()


        }
        let interval: number;
        if (status.status === "running") {
            void getIp()
            interval = setInterval(() => getIp(), 30000);
        }

        return () => {
            clearInterval(interval);
        }

    }, [getVmIp, status.status])


    //@ts-ignore
    const vmStatus = status?.lock === "clone" ? "Создаётся" : getReadableVmStatus(status?.status);

    if (status.status === "stopped" || stopped) {
        return <></>;
    }

    return (
        <div className={style.container}>

            <div className={style.vmInfo}>
                <div>{loading ? "Узнаём реквизиты" : "Реквизиты"}</div>
                <Skeleton isLoaded={!loading} fadeDuration={2}>
                    <div>
                        <span>Имя машины: {status?.name}</span>
                        <div>Статус: {vmStatus}</div>
                        <div>Адрес: {ip}</div>
                        <div>Имя пользователя: test</div>
                        <div>Пароль: test</div>
                    </div>
                </Skeleton>
            </div>

        </div>
    )
}