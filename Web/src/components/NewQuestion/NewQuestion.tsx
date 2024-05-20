import React from "react";
import style from "../TestOpen/TestOpen.module.scss";
import {Question as QuestionsItem, Test as TestsItem} from "../../../api";
import {useParams} from "react-router-dom";
import {apiService} from "../../services";

export function NewQuestion() {
    const {id} = useParams<{ id: string }>();
    const [newQuestions, setNewQuestions] = React.useState<QuestionsItem[]>([]);
    const [test, setTest] = React.useState<TestsItem>();

    React.useEffect(() => {
        const fetch = async () => {
            if (!id) return;

            const response = await apiService.getTest(id);

            if (response instanceof Error) {
                return;
            }
            setTest(response)
        }
        void fetch();

    }, [id]);

    React.useEffect(() => {
        const copy = Object.assign([], newQuestions);

        const fetchQuestions = async (questionId: string, copy: QuestionsItem[]) => {
            const response = await apiService.getQuestion(questionId);

            if (response instanceof Error) {
                return;
            }
            copy.push(response);
            setNewQuestions([...copy]);
        }

        test?.questions?.map(
            question =>
                void fetchQuestions(question, copy)
        );
    }, [test]);

    const questions = newQuestions.map((question) => {
        return question;
    });

    return (
        <div>
            {questions.map((question) => (
                <div className={style.container} key={question.id}>
                    <div className={style.title}>{question.text}</div>
                    <div className={style.text}>{question.description}</div>
                </div>
            ))}
        </div>
    );
}