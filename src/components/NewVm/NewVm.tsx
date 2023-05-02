import {FormControl, FormLabel, Input, NumberInput, NumberInputField, VStack} from "@chakra-ui/react";

import style from "./NewVm.module.scss"
import {Button} from "../Button/Button";
import React from "react";
import {apiService} from "../../services";
import {useNavigate} from "react-router-dom";
import {userStore} from "../../stores";

export function NewVm() {
    const navigate = useNavigate()
    const [vmid, setVmid] = React.useState<string>("");
    const [name, setName] = React.useState("")
    const [loading, setLoading] = React.useState(false)
    const createVm = async () => {
        if (!Number(vmid) || !name) return;

        setLoading(true);
        const response = await apiService.createVm({vmid: Number(vmid), name});
        if (!(response instanceof Error)) {
            setLoading(false)
            userStore.addVm(Number(vmid));
            navigate(`/vms/${vmid}`);
        }
    }

    return (
        <div className={style.container}>
            <div className={style.form}>
                <VStack>
                    <Input type='text' placeholder={"ID машины"} value={vmid} onChange={e => setVmid(e.target.value)}/>
                    <Input type='text' placeholder={"Имя машины"} value={name} onChange={e => setName(e.target.value)}/>
                </VStack>
                <Button isLoading={loading} onClick={createVm} className={style.createButton} variant={"solid"}>Создать
                    вм</Button>
            </div>
        </div>
    )
}