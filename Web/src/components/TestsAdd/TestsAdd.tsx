import { useEffect, useState } from "react";

import {useNavigate} from "react-router-dom";

import {Question, Test} from "../../../api";

import {Button} from "../Button/Button";
import {QuestionAdd} from "../QuestionAdd/QuestionAdd";
import {apiService} from "../../services";
import { ReactJSXElement } from "@emotion/react/types/jsx-namespace";

import  "./TestsAdd.css";


export function TestsAdd({id, name, description} : Test) {
    const [testName, setTestName] = useState<string>(name);
    const [testDescription, setTestDescription] = useState<string>(description);
    const [testQuestions, setTestQuestions] = useState<{[key: string]: any}>({});

    const [questionsElements, setQuestionsElements] = useState<ReactJSXElement[]>([]);
    const navigate = useNavigate();

    const addNewTest = async () => {
        if (!testName || !testDescription) return;

        const questions : Question[] = Object.values(testQuestions);

        const newTest : Test = {
            id: id,
            name: testName,
            description: testDescription,
            questions: questions
        };

        const response = await apiService.postTest(newTest);
        if (response instanceof Error) {
            return;
        }

        navigate("/tests")
    };

    const handleQuestionChange = (currentQuestion : Question, index : number) => {
        setTestQuestions(prevTestQuestions => ({
            ...prevTestQuestions,
            [index]: currentQuestion
        }));
    };

    const deleteQuestion = (questionId : number) =>{
        const tempDict = { ...testQuestions };
        delete tempDict[questionId];
        setTestQuestions(tempDict);

        const updatedVariants : ReactJSXElement[] = questionElements.filter((item, itemIndex) => itemIndex != questionId);
        setQuestionsElements(updatedVariants);
    };

    const questionElements = questionsElements.map((element, index) => {
        return <QuestionAdd
            key={index}
            onChangeQuestion={handleQuestionChange}
            onDeleteQuestion={deleteQuestion}
            id={`${index}`}
        />
    });

    const addNewQuestion = () => {
        setQuestionsElements([...questionsElements, questionsElements[0]]);
    };

    return (
        <body className="body">
            <div>
                <div className="test__header">
                    <div className="test__title">
                        <input type="text" className="test__title" placeholder="Название"
                               onChange={e => setTestName(e.target.value)}/>
                    </div>
                    <nav className="question-nav" aria-label="Main">
                        <ul className="question-nav__list">
                            <li className="question-nav__item">
                                <button className="text__type bold">
                                    <img src="img/bold.png"/>
                                </button>
                            </li>
                            <li className="question-nav__item">
                                <button className="text__type italic">
                                    <img src="./img/italic.png"/>
                                </button>
                            </li>
                            <li className="question-nav__item">
                                <button className="text__type underlined">
                                    <img src="./img/underlined.png"/>
                                </button>
                            </li>
                            <li className="question-nav__item">
                                <button className="text__type link">
                                    <img src="./img/link.png"/>
                                </button>
                            </li>
                        </ul>
                    </nav>
                    <div className="test__description">
                        <input type="text" className="test__description" placeholder="Описание"
                               onChange={e => setTestDescription(e.target.value)}/>
                    </div>
                </div>
                {questionElements}
            </div>
            <Button onClick={addNewQuestion}>Добавить вопрос</Button>
            <Button onClick={addNewTest}>Создать тест</Button>
        </body>
    )
}