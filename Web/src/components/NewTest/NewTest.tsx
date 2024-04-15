import React from "react";
import style from "./NewTest.module.scss"
import {Question as QuestionsItem} from "../../../api"
import {apiService} from "../../services";
import {NewQuestion} from "../NewQuestion/NewQuestion"

export interface NewItemProps {
    id?: string;
    name?: string;
    description?: string;
    questions?: string[];
}

export function NewTest({id, name, description, questions}: NewItemProps) {
    const [newQuestions, setNewQuestions] = React.useState<QuestionsItem[]>([]);

    const fetch = async (questionId: string, copy: QuestionsItem[]) => {
        const response = await apiService.getQuestion(questionId);

        if (response instanceof Error) {
            return;
        }
        copy.push(response);
        setNewQuestions([...copy]);

    }

    React.useEffect(() => {
        const copy = Object.assign([], newQuestions);

        questions?.map(
            question =>
                void fetch(question, copy)
        );
    }, []);

    const result = newQuestions.map((element, index) => {
        return <NewQuestion key={`${index}`}{...element!} />;
    });

    return (
        <div className={style.container}>
            <div className={style.title}>{name}</div>
            <div className={style.text}>{description}</div>
            {result}
        </div>
    );
}