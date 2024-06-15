import { useState } from "react";

import {useNavigate} from "react-router-dom";

import {Question, Test} from "../../../api";

import {Button} from "../Button/Button";
import {QuestionAdd} from "../QuestionAdd/QuestionAdd";
import {apiService} from "../../services";

import  "./TestsAdd.css";


export function TestsAdd({id, name, description} : Test) {
    const [testName, setTestName] = useState<string>(name);
    const [testDescription, setTestDescription] = useState<string>(description);
    const [testQuestions, setTestQuestions] = useState<{[key: string]: any}>({});
    const [keys, setKeys] = useState<number[]>([]);
    const [questionsElements, setQuestionsElements] = useState<React.ReactNode[]>([]);
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

        setQuestionsElements(prevState => prevState.filter((item, itemIndex) => itemIndex != questionId));
        setKeys(keys.filter((item) => item != questionId));
    };

    const questionElements = questionsElements.map((element, index) => {
        if (keys[index] === undefined) {
            return;
        }

        return <QuestionAdd
            key={`${keys[index]}`}
            onChangeQuestion={handleQuestionChange}
            onDeleteQuestion={deleteQuestion}
            id={`${keys[index]}`}
        />;
    });

    const addNewQuestion = () => {
        if (keys[0] === undefined) {
            setKeys([0]);
        } else {
            setKeys([...keys, keys[keys.length - 1] + 1]);
        }  

        setQuestionsElements([...questionsElements, 
        <QuestionAdd
            key={`${keys}`}
            onChangeQuestion={handleQuestionChange}
            onDeleteQuestion={deleteQuestion}
            id={`${keys}`}
        />]);
    };

    return (
        <body className="body">
            <div>
                <div className="test__header">
                    <div className="test__title">
                        <input type="text" className="test__title" placeholder="Название"
                               onChange={e => setTestName(e.target.value)}/>
                    </div>
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