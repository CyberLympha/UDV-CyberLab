import React, {useState} from "react";
import  "./TestsAdd.css";
import {Button} from "../Button/Button";
import {QuestionAdd} from "../QuestionAdd/QuestionAdd";
import {apiService} from "../../services";
import {useNavigate} from "react-router-dom";
import {Question, Test} from "../../../api";


export function TestsAdd({id, name, description, questions} : Test) {
    const [localName, setLocalName] = React.useState(name);
    const [localDescription, setLocalDescription] = React.useState(description);
    const [localQuestion, setLocalQuestion] = React.useState<string[]>([]);
    const [dictQuestions, setDictQuestions] = useState({});
    const [value] = useState<string>('');
    const navigate = useNavigate();

    const addNewTest = async () => {
        if (!localName || !localDescription) return;

        const questions : Question[] = Object.values(dictQuestions);

        const newTest : Test = {
            id: id,
            name: localName,
            description: localDescription,
            questions: questions
        };

        console.log(newTest);

        const response = await apiService.postTest(newTest);
        if (response instanceof Error) {
            return;
        }

        navigate("/tests")
    };

    const handleQuestionChange = (currentQuestion : Question, index : number) => {
        setDictQuestions(prevDictionary => ({
            ...prevDictionary,
            [index]: currentQuestion
        }));
    };

    const questionElements = localQuestion.map((element, index) => {
        return <QuestionAdd
            key={index}
            onChangeQuestion={handleQuestionChange}
            id={`${index}`}
        />
    });

    const addNewQuestion = () => {
        setLocalQuestion([...localQuestion, value]);
    };

    return (
        <body className="body">
            <div>
                <div className="test__header">
                    <div className="test__title">
                        <input type="text" className="test__title" placeholder="Название"
                               onChange={e => setLocalName(e.target.value)}/>
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
                               onChange={e => setLocalDescription(e.target.value)}/>
                    </div>
                </div>
                {questionElements}
            </div>
            <Button onClick={addNewQuestion}>Добавить вопрос</Button>
            <Button onClick={addNewTest}>Создать тест</Button>
        </body>
    )
}