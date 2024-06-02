import React from "react";
import {Test as TestsItem} from "../../../api"
import {apiService} from "../../services";
import style from "./TestList.module.scss"
import {Button} from "../Button/Button";
import {AiOutlinePlus} from "react-icons/all";
import {useNavigate} from "react-router-dom";
import { TestItem } from "./TestItem";


export function TestList() {
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
    <div>
        <table className="test_table">
        <thead>
        <tr>
        <th>Имя теста</th>
        <th>Описание теста</th>
        <th>Владелец теста</th>
        <th className="head_sort">
            <img className="test_order" src="public/img/order.png" />
            <img className="test_title" src="public/img/sort.png" />
        </th>
        </tr>
        </thead>
        <tbody>
            {tests.map(test => <TestItem key={test.id } {...test} ></TestItem>)}
        </tbody>
        </table>
        {addNewItemButton}
    </div>
    )
}
