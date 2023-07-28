import style from "./Vm.module.scss"

export interface VmProps {
    status: "Остановлена" | "Запущена",
    name: string;
    ip: string;
    show: boolean;

}

export function MockVm({status, name, ip, show}: VmProps) {

    if (!show) return <></>;

    return (
        <div className={style.container}>

            <div className={style.vmInfo}>
                <div>Реквизиты</div>
                <div>
                    <span>Имя машины: {name}</span>
                    <div>Статус: {status}</div>
                    <div>Адрес: {ip}</div>
                    <div>Имя пользователя: test</div>
                    <div>Пароль: test</div>
                </div>
            </div>

        </div>
    )
}