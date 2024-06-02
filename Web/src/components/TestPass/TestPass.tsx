import React from "react";
import style from "../TestOpen/TestOpen.module.scss";
import {Question, Test} from "../../../api";
import {useParams} from "react-router-dom";
import {apiService} from "../../services";
import { Variants } from "./Variants";

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
        // let decoder = new TextDecoder([label], [options]);
        // let str = decoder.decode([question.questionData], [options]);
        // console.log(new TextDecoder().decode(question.questionData));
        console.log(question.questionData);
        return question;
    });

    const variants = < Variants
        variantsType={"Radio"}
     />;

    return (
        <div>
            {questions.map((question, index) => (
                <div className="test__body" key={index}>
                    <ul className="list__questions">
                        <li className="question">
                            <nav className="question-type-nav">
                                <div className="question__title">
                                    <div className="question__title"> 
                                        { question.text }
                                    </div>
                                </div>
                            </nav>
                            <ul className="list__answers">
                                { question.questionType }
                                { question.questionData }
                                {/* { variants } */}
                            </ul>
                        </li>
                    </ul>
                </div>
            ))}
        </div>
    );
}