import { useEffect, useState } from "react";
import {Question} from "../../../api";
import {VariantAdd} from "../VariantAdd/VariantAdd";
import  "../TestsAdd/TestsAdd.css";


export function QuestionAdd({ onChangeQuestion, onDeleteQuestion, id } : any, { text } : Question) {    
    const [nameQuestion, setNameQuestion] = useState(text);
    const [questionType, setQuestionType] = useState<string>("Radio");
    const [indexesAnswers, setIndexesAnswers] = useState<Set<number>>(new Set<number>());
    const [dictVariants, setDictVariants] = useState<{[key: string]: any}>({});
    const [variantsElements, setVariantsElements] = useState<React.ReactNode[]>([]);
    const [question, setQuestion] = useState<Question>({
        id: "",
        text: "",
        description: "delete",
        questionType: "Radio",
        correctAnswer: "",
        questionData: {Variants : ""}
    });
    const [keys, setKeys] = useState<number[]>([]);
    
    const questionTypes = [
        {label: "Один из списка", value: "Radio"},
        {label: "Несколько из списка", value: "CheckBox"}
    ];

    useEffect(() => {
        const tempVariants = {"Variants" : ""};
        tempVariants.Variants = `[${Object.values(dictVariants)}]`;

        const answers : string[] = [];
        indexesAnswers.forEach((index) => {
            if ((dictVariants as never)[index] !== undefined) {
                answers.push((dictVariants as never)[index]);
            }
        });
        
        setQuestion({
            ...question,
            text: nameQuestion,
            questionType: questionType,
            questionData: tempVariants,
            correctAnswer: `[${answers}]`
        });

    }, [nameQuestion, questionType, dictVariants, indexesAnswers]);

    useEffect(() => {
        sendQuestion();

    }, [question]);

    const deleteQuestion = () => {
        onDeleteQuestion(id);
    };

    const sendQuestion = () => {
        onChangeQuestion(question, id);
    };

    const handleTextChanged = (event : any) => {
        setNameQuestion(event.target.value);
    };

    const handleTypeChanged = (event : any) => {
        setQuestionType(event.target.value);
        setIndexesAnswers(new Set<number>());
    };

    const addNewVariant = () => {
        if (keys[0] === undefined) {
            setKeys([0]);
        } else {
            setKeys([...keys, keys[keys.length - 1] + 1]);
        }        

        setVariantsElements([...variantsElements, 
        <VariantAdd
            key={`${keys}`}
            onChangeVariant={handleVariantChange}
            onChangeAnswer={handleAnswerChange}
            onDeleteVariant={deleteVariant}
            variantId={`${keys}`}
            questionId={`${id}`}
            variantsType={questionType}
        />]);
    };

    const handleVariantChange = (currentVariant : string, index : number) => {
        setDictVariants(prevDictionary => ({
            ...prevDictionary,
            [index]: `"${currentVariant}"`
        }));
    };

    const handleAnswerChange = (index : number) => {
        if (questionType == "Radio") {
            setIndexesAnswers(new Set<number>([index]));
        } else {
            if (!indexesAnswers.has(index)) {
                setIndexesAnswers(prevSet => new Set<number>([...prevSet, index]));
            } else {
                indexesAnswers.delete(index);
                setIndexesAnswers(indexesAnswers);
            }
        }
    };

    const deleteVariant = (variantId : number) => {
        const tempVariants = { ...dictVariants };
        delete tempVariants[variantId];
        setDictVariants(tempVariants);

        setVariantsElements(prevState => prevState.filter((item, itemIndex) => itemIndex != variantId));
        setKeys(keys.filter((item) => item != variantId));
    };

    return (
        <div className="test__body">
            <ul className="list__questions">
                <li className="question">
                    <div className="ellipsis">
                    </div>
                    <nav className="question-type-nav">
                        <div className="question__title">
                            <input type="text" className="question__title" placeholder="Новый заголовок"
                                   onChange={handleTextChanged}/>
                        </div>
                        <select onChange={handleTypeChanged} className="question__type">
                            {questionTypes.map((type, index) => (
                                <option key={index} value={type.value}>
                                    {type.label}
                                </option>
                            ))}
                        </select>
                    </nav>
                    <ul className="list__answers">
                        {variantsElements.map((element, index) => {
                            if (keys[index] === undefined) {
                                return;
                            }
                            return <VariantAdd
                                key={`${keys[index]}`}
                                onChangeVariant={handleVariantChange}
                                onChangeAnswer={handleAnswerChange}
                                onDeleteVariant={deleteVariant}
                                variantId={`${keys[index]}`}
                                questionId={`${id}`}
                                variantsType={questionType}
                            />;
                        })}
                        <li className="answer">
                            <div className="answer__title">
                                <button onClick={addNewVariant} className="answer__append">
                                    Добавить вариант
                                </button>
                            </div>
                        </li>
                    </ul>
                    <footer className="test__footer">
                        <div className="test__actions">
                            <div className="action delete">
                                <button  onClick={deleteQuestion}>
                                    <img src="./img/trash.png"/>
                                </button>                               
                            </div>
                        </div>
                    </footer>
                </li>
            </ul>
        </div>
    )
}
