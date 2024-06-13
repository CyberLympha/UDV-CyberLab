import { useEffect, useState } from "react";
import { Attempt } from "../../../api";
import { apiService } from "../../services";
import { UserResultItem } from "./UserResultItem";
import  "./TestResultsItem.css";


export function TestResultItem({ idTest } : any) {
    const [testAttempts, setTestAttempts] = useState<Attempt[]>([]);

    useEffect(() => {
        const fetchTestAttempts = async () => {
            const response = await apiService.getTestAttempts(`${idTest}`);

            if (response instanceof Error) {
                return;
            }
            setTestAttempts(response);
        }
        void fetchTestAttempts();

    }, [])

    const resultsTable = testAttempts.map((testAttempt, index) => {
        return <UserResultItem
            key={index}
            examineeId={testAttempt.examineeId}
            testId={testAttempt.id}
        />
    });

    return (
        <body>
            <table className={"teacher_table"}>
                <thead>
                    <tr>
                        <th>Студент</th>
                        <th>Результат</th>
                    </tr>
                </thead>
                {resultsTable}
            </table>
        </body>
    )
}