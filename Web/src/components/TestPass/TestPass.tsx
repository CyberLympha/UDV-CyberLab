import React from "react";
import {Question, Test} from "../../../api";
import {useNavigate, useParams} from "react-router-dom";
import {apiService} from "../../services";
import { Variants } from "./Variants";
import { Button } from "../Button/Button";
import { userStore } from "../../stores";

export function TestPass() {
    const {id} = useParams<{ id: string }>();
    const [newQuestions, setNewQuestions] = React.useState<Question[]>([]);
    const [test, setTest] = React.useState<Test>();
    const [answers, setAnswers] = React.useState<{[index: string]: string[]}>({});
    const [answerLoaded, setAnswerLoaded] = React.useState<boolean>(true);
    const navigate = useNavigate();

    React.useEffect(() => {
        const fetchTest = async () => {
            if (!id) return;

            const response = await apiService.getTest(id);

            if (response instanceof Error) {
                return;
            }
            setTest(response)
        }
        void fetchTest();

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
        return question;
    });

    const parseVariants = (question : Question) => {
        const variants = `${question.questionData}`.substring(14).slice(0, -3).split(",");

        const variantsJSX = variants.map((v, index) => {
            return < Variants
            key={index}
            variantsType={`${question.questionType}`}
            variant={`${JSON.parse(`\"${v}\"`)}`.substring(1).slice(0, -1)}
            questionId={`${question.id}`}
            variantId={`${index}`}
            onChangeAnswer={writeAnswers}
            />
        });
        
        return variantsJSX;
    };
    
    React.useEffect(() => {
        setAnswerLoaded(false);

    }, [answers])

    const writeAnswers = (variant : string, questionId : string, variantsType : string) => {

        if (variantsType == "Radio" || !answers.hasOwnProperty(questionId)) {
            setAnswers(prevDictionary => ({
                ...prevDictionary,
                [questionId]: [`"${variant}"`]
            }));
        } else {
            if (answers[questionId].find((string) => string === `"${variant}"`) === undefined) {
                setAnswers(prevDictionary => ({
                    ...prevDictionary,
                    [questionId]: [...prevDictionary[`${questionId}`], `"${variant}"`]
                }));
            } else {
                const newAnswers = answers[questionId].filter(item => item !== `"${variant}"`);
                setAnswers(prevDictionary => ({
                    ...prevDictionary,
                    [questionId]: [...newAnswers]
                }));
            }
        }
    };
    
    const sendAnswer = async (questionId : string) => {
        
        const response = apiService.giveAnswerAttempt(
            {questionId : `${questionId}`, answer : `[${answers[questionId]}]`},
            `${userStore.user?.testAttempt.idAttempt}`);

        if (response instanceof Error) {
            return;
        }
    };

    const sendTest = async () => {
        const response = await apiService.endAttempt(`${userStore.user?.testAttempt.idAttempt}`);

        if (response instanceof Error) {
            return;
        }

        navigate("/tests");
    };
    
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
                                { parseVariants(question) }
                            </ul>
                            <Button isLoading={answerLoaded}
                            onClick={() => sendAnswer(question.id)}>Сохранить ответ</Button>
                        </li>
                    </ul>
                </div>
            ))}
            <Button onClick={sendTest}>Закончить тест</Button>
        </div>
    );
}