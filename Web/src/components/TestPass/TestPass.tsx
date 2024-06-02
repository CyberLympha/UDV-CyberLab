import React from "react";
import style from "../TestOpen/TestOpen.module.scss";
import {Question, Test} from "../../../api";
import {useParams} from "react-router-dom";
import {apiService} from "../../services";

export function TestPass() {
    const {id} = useParams<{ id: string }>();
    const [newQuestions, setNewQuestions] = React.useState<Question[]>([]);
    const [test, setTest] = React.useState<Test>();

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

        const fetchQuestions = async (questionId: Question, copy: Question[]) => {
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
        console.log(question.questionData);
        return question;
    });

    return (
        <div>
            {questions.map((question, index) => (
                <div className={style.container} key={index}>
                    <div className={style.title}>{question.text}</div>
                </div>
            ))}
        </div>
    );
}