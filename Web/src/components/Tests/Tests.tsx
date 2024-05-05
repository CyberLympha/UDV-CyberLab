import React from "react";

import {Test as TestsItem} from "../../../api"
import {apiService} from "../../services";
import {NewTest} from "../NewTest/NewTest"

import style from "./Tests.module.scss"
import {Button} from "../Button/Button";
import {AiOutlinePlus} from "react-icons/all";
import {useNavigate} from "react-router-dom";


export function Tests() {
    const [tests, setTests] = React.useState<TestsItem[]>([]);
    const navigate = useNavigate();

    React.useEffect(() => {
        const fetch = async () => {
            const response = await apiService.getTests();

            if (response instanceof Error) {
                return;
            }
            setTests(response)
        }
        void fetch();
    }, [])

    const createNewItem = async () => {
        navigate("/tests/add")
    }

    const addNewItemButton = <div className={style.addNewItem}>
        <Button rightIcon={<AiOutlinePlus/>} onClick={createNewItem}>Добавить тест</Button>
    </div>

    return (
        <div id={"news"} className={style.container}>
            {addNewItemButton}
            {tests?.map(newItem => <NewTest key={`${newItem.id}`} {...newItem} />)}
        </div>
    )
}
