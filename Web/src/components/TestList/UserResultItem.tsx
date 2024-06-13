import { useEffect, useState } from "react";
import { apiService } from "../../services";
import  "./TestResultsItem.css";

export function UserResultItem ( { examineeId, testId } : any ) {
    const [user, setUser] = useState<string>("");
    const [result, setResult] = useState<string>("");

    useEffect(() => {
        const fetchUser = async (examineeId : string) => {
            const response = await apiService.getUserById(`${examineeId}`);

            if (response instanceof Error) {
                return;
            }

            setUser(`${response.secondName} ${response.firstName}`);
        }
        const fetchResult = async (id : string) => {
            const response = await apiService.getAttemptResult(`${id}`);

            if (response instanceof Error) {
                return;
            }

            setResult(`${response.totalScore.slice(8)}`);
        }

        void fetchUser(examineeId);
        void fetchResult(testId);

    }, [])

    return (
        <tbody>
            <tr className="student_item">
                <td className="studen_item_info">
                    <img className="student_item_img" src="/public/img/user.png"/>
                    <div className="student_item_name">
                        {user}
                    </div>
                </td>
                <td className="student_item_result">
                    {result}
                </td>
            </tr>
        </tbody>
    )
}