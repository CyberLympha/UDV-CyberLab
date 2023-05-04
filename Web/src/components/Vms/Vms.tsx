import {observer} from "mobx-react-lite";
import {userStore} from "../../stores";
import {Button} from "../Button/Button";
import {useNavigate} from "react-router-dom";
import style from "./Vms.module.scss"


export const Vms = observer(() => {
    const usersVm = userStore.user?.vms;

    const navigate = useNavigate()

    const renderVm = (vms: number[] | undefined) => {
        return vms?.map((vm) => {
            return <div key={vm} className={style.vmItem} onClick={() => navigate(`/vms/${vm}`)}>{vm}</div>
        })
    }


    return <div style={{padding: "16px"}}>
        <div>
            <div><Button onClick={() => navigate("/vms/new")}>Создать машину</Button></div>
        </div>
        <div>Список вм</div>
        <div>{renderVm(usersVm)}</div>
    </div>
})
