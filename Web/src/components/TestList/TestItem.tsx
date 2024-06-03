import { Link } from "react-router-dom";
import { Test } from "../../../api";
import  "./TestItem.css";
import { useEffect, useState } from "react";
import { apiService } from "../../services";
import { userStore } from "../../stores";

const DELAY = 100;

export function TestItem({id, name, description}: Test){
    const [isActive, setActive] = useState(false);
    const [resultTest, setResultTest] = useState<string>("");
    const handleClick = () => {
        setActive(!isActive);
    };

    useEffect(() => {
        const fetchTestResult = async () => {

            const response = await apiService.getAttemptResult(`${userStore.user?.tests}`);

            if (response instanceof Error) {
                setResultTest("Тест не пройден");
                return;
            }
            console.log(response);

            setResultTest(`${response}`);
        }
        void fetchTestResult();

    }, [])

    const startTest = async () => {
        const response = await apiService.startAttempt({testId: id, 
            examineeId: userStore.user!.id});
 
        if (response instanceof Error) {
            return;
        }

        console.log(`Start test: ${response}`);
        userStore.setTest(`${response}`);
    }

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
                <button className="test_item_other_button" type="button" onClick={handleClick}>
                    <img className="test_item_other_img" src="public/img/other.png"/>
                </button>
                {
                    isActive &&
                    <div id="myDropdown" className="dropdown-actions">
                        <button className="test_item_action rename">
                            <img className="test_action_img" src="public/img/rename.png" />
                            Переименовать
                        </button>
                        <button className="test_item_action delete">
                            <img className="test_action_img" src="public/img/delete.png" />
                            Удалить
                        </button>
                    </div>
                }
                
            </div>
        </td>
        </tr>
    );
}