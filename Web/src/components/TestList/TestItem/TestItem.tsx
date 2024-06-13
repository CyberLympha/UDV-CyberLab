import { Link } from "react-router-dom";
import { Attempt, UserRole } from "../../../../api";
import  "./TestItem.css";
import { useEffect, useState } from "react";
import { apiService } from "../../../services";
import { userStore } from "../../../stores";
import { TestResultItem } from "../TestResultsItem";

const DELAY = 100;

interface ITestItem{
    id: string,
    name: string,
    description: string,
    onDeleteTest: () => void
}

export function TestItem({id, name, description, onDeleteTest }: ITestItem){
    const [isActionActive, setActionActive] = useState(false);
    const [isResultActive, setResultActive] = useState(false);
    const [resultTest, setResultTest] = useState<string>("");
    const [listAttempts, setListAttempts] = useState<Attempt[]>([]);
    const [idTestsAttempts, setIdTestsAttempts] = useState<{[key: string]: any}>({});
    const openAction = () => {
        setActionActive(!isActionActive);
    };
    const openResult = () => {
        setResultActive(!isResultActive);
    };

    useEffect(() => {
        listAttempts.map(attempt => {
            setIdTestsAttempts(prevDictionary => ({
                ...prevDictionary,
                [attempt.testId]: attempt
            }));
        });

    }, [listAttempts])

    useEffect(() => {
        const fetchAttempts = async () => {
            const response = await apiService.getUserAttempts(`${userStore.user?.id}`);

            if (response instanceof Error) {
                return;
            }
            setListAttempts(response);
        }
        void fetchAttempts();
    }, [])

    useEffect(() => {
        const fetchTestResult = async () => {
            if (idTestsAttempts[id] === undefined) {
                setResultTest("Тест не пройден");
                return;
            } else if (idTestsAttempts[id].status == `InProcess`) {
                setResultTest("Тест решается");
                return;
            }

            const response = await apiService.getAttemptResult(idTestsAttempts[id].id);

            if (response instanceof Error) {
                return;
            }

            setResultTest(`Результат теста: ${response.totalScore.slice(8)}`);
        }
        void fetchTestResult();

    }, [idTestsAttempts])

    const startTest = async () => {
        const response = await apiService.startAttempt({testId: id, 
            examineeId: userStore.user!.id});
 
        if (response instanceof Error) {
            return;
        }

        userStore.setAttempt({idTest: id, idAttempt: response});
    };

    const deleteTest = async () => {
        const response = await apiService.deleteTest(id);

        if (response instanceof Error) {
            return;
        }
        onDeleteTest();
    };

    const listResults = <TestResultItem
                            idTest={`${id}`}
                        />;

    return (
        <tr className="test_item">
        <td className="test_item_info">
        <img className="test_item_img" src="public/img/test.png" />
        <div className="test_item_name">
            <Link to={`/tests/${id}/questions`} onClick={startTest}>
                {name}
            </Link>
        </div>
        </td>
        <td className="test_item_owner">{description}</td>
        <td className="test_item_date_view">{resultTest}</td>
        <td className="test_item_other">
            <div className="dropdown">
                {
                    (userStore.user?.role === UserRole.Teacher ||
                        userStore.user?.role === UserRole.Admin) &&
                    <button className="test_item_other_button" type="button" onClick={openAction}>
                        <img className="test_item_other_img" src="public/img/other.png"/>
                    </button>
                }
                {
                    isActionActive && 
                    <div id="myDropdown" className="dropdown-actions" onClick={openResult}>
                        <button className="test_item_action rename">
                            <img className="test_action_img" src="public/img/rename.png" />
                            Результаты
                        </button>
                        <button className="test_item_action delete" onClick={deleteTest}>
                            <img className="test_action_img" src="public/img/delete.png" />
                            Удалить
                        </button>
                    </div>
                }    
                {
                    isResultActive &&
                    listResults
                }            
            </div>
        </td>
        </tr>
    );
}