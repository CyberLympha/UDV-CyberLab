import { Link } from "react-router-dom";
import { StartTestRequest, Test } from "../../../api";
import  "./TestItem.css";
import { useEffect, useState } from "react";
import { apiService } from "../../services";
import { UserStore } from "../../stores/userStore";
import { userStore } from "../../stores";

const DELAY = 100;

export function TestItem({id, name, description, questions}: Test){
    const [isActive, setActive] = useState(false);
    const handleClick = () => {
        setActive(!isActive);
    };

    const startTest = async () => {
        const testInfo : StartTestRequest = {
            testId: id,
            examineeId: userStore.user?.id
        };

        const response = await apiService.startTest(testInfo);

        console.log(`Start test: ${response}`);
 
        if (response instanceof Error) {
            return;
        }
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
        <td className="test_item_date_view">{name}</td>
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