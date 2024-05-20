import React, {useState} from "react";
import  "./TestsAdd.css";
import {Button} from "../Button/Button";
import {QuestionAdd} from "../QuestionAdd/QuestionAdd";
import {apiService} from "../../services";
import {useNavigate} from "react-router-dom";
import {Question} from "../../../api";

interface TestsAddProps {
    id?: string;
    name?: string;
    description?: string;
    questions?: Question;
}

export function TestsAdd({id, name, description, questions} : TestsAddProps) {
    const [localName, setLocalName] = React.useState(name);
    const [localDescription, setLocalDescription] = React.useState(description);
    const [localQuestion, setLocalQuestion] = useState<string[]>([]);
    const [value] = useState<string>('');
    const navigate = useNavigate();

    const addNewTest = async () => {
        if (!localName || !localDescription) return;

        const response = await apiService.postTest({
            name: localName,
            description: localDescription,
            questions: []
        });
        if (response instanceof Error) {
            return;
        }

        navigate("/tests")
    };

    // const qwe = async () => {
    //
    //     const response = await apiService.postQuestion({
    //         text: "",
    //         description: "",
    //         questionType: "",
    //         questionData: [],
    //         correctAnswer: ""
    //     });
    //
    //     if (response instanceof Error) {
    //         return;
    //     }
    //
    //     navigate("/tests")
    // };

    const question = localQuestion.map((element, index) => {
        return <QuestionAdd key={`${index}`} />;
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
                {question}
            </div>
            <Button onClick={addNewQuestion}>Добавить вопрос</Button>
            <Button onClick={addNewTest}>Создать тест</Button>
            {/*<Button onClick={qwe}>Отправить вопрос</Button>*/}
        </body>
    )
}