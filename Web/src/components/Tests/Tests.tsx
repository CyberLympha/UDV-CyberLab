import React from "react";

import {Test as TestsItem} from "../../../api"
import {apiService} from "../../services";
import {NewTest} from "../NewTest/NewTest"

import style from "./Tests.module.scss"


export function Tests() {
    const [tests, setTests] = React.useState<TestsItem[]>([]);

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

    return (
        <div id={"news"} className={style.container}>
            {tests?.map(newItem => <NewTest key={`${newItem.id}`} {...newItem} />)}
        </div>
    )
}
