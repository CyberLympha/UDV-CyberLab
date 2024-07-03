import { useEffect, useState } from "react";
import { Test , UserRole} from "../../../../api"
import {apiService} from "../../../services";
import style from "./TestList.module.scss"
import {Button} from "../../Button/Button";
import {AiOutlinePlus} from "react-icons/all";
import {useNavigate} from "react-router-dom";
import { TestItem } from "../TestItem/TestItem";
import { userStore } from "../../../stores";


export function TestList() {
    const [tests, setTests] = useState<Test[]>([]);
    const [testIsDelete, setTestIsDelete] = useState<boolean>(false);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchTests = async () => {
            const response = await apiService.getTests();

            if (response instanceof Error) {
                return;
            }
            setTests(response)
        }
        void fetchTests();
    }, [testIsDelete])

    const createNewItem = async () => {
        navigate("/tests/add");
    };

    const addNewItemButton = <div className={style.addNewItem}>
        <Button rightIcon={<AiOutlinePlus/>} onClick={createNewItem}>Добавить тест</Button>
    </div>

    const deleteTest = () =>{
        setTestIsDelete(!testIsDelete);
    };

    const listTests = tests.map(test => 
        <TestItem 
            key={test.id }
            {...test}
            onDeleteTest={deleteTest}
        />
    );

    return (
    <div>
        <table className="test_table">
        <thead>
        <tr>
        <th>Имя теста</th>
        <th>Описание теста</th>
        <th>Результат теста</th>
        <th className="head_sort">
            <img className="test_order" src="public/img/order.png" />
            <img className="test_title" src="public/img/sort.png" />
        </th>
        </tr>
        </thead>
        <tbody>
            {listTests}
        </tbody>
        </table>
        {(userStore.user?.role === UserRole.Admin || 
        userStore.user?.role === UserRole.Teacher) && addNewItemButton}
    </div>
    )
}
