import {Input, Select, VStack} from "@chakra-ui/react";
import {useNavigate} from "react-router-dom";
import React from "react";

import {Button} from "../Button/Button";
import {apiService} from "../../services";
import {userStore} from "../../stores";
import {VmType} from "../../../api";

import style from "./NewVm.module.scss"

export function NewVm() {
    const navigate = useNavigate()
    const [vmid, setVmid] = React.useState<string>("");
    const [name, setName] = React.useState("")
    const [type, setType] = React.useState<VmType>(VmType.Kali)
    const [loading, setLoading] = React.useState(false)
    const createVm = async () => {
        if (!Number(vmid) || !name) return;

        setLoading(true);
        const response = await apiService.createVm({vm: {vmid: Number(vmid), name}, type});
        if (!(response instanceof Error)) {
            userStore.addVm(Number(vmid));
            navigate(`/vms/${vmid}`);
        }
        setLoading(false)
    }

    return (
        <div className={style.container}>

            <VStack>
                <Select style={{width: "400px"}} placeholder='Выберите ОС'
                        onChange={e => setType(e.target.value as VmType)}>
                    <option value={VmType.Kali}>Kali</option>
                    <option value={VmType.Ubuntu}>Ubuntu</option>
                    <option value={VmType.Windows}>Windows</option>
                </Select>
                <Input style={{width: "400px"}} type='text' placeholder={"ID машины"} value={vmid}
                       onChange={e => setVmid(e.target.value)}/>
                <Input style={{width: "400px"}} type='text' placeholder={"Имя машины"} value={name}
                       onChange={e => setName(e.target.value)}/>
            </VStack>
            <Button isLoading={loading} onClick={createVm} className={style.createButton} variant={"solid"}>Создать
                вм</Button>
        </div>
    )
}