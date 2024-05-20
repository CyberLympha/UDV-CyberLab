import React from "react";
import  "../TestsAdd/TestsAdd.css";
import {Question} from "../../../api";
import {apiService} from "../../services";
import {Button} from "../Button/Button";


export function QuestionAdd({id, text, description, questionType, questionData, correctAnswer} : Question) {
    const [localName, setLocalName] = React.useState(text);
    const [localDescription, setLocalDescription] = React.useState(description);

    // const qwe = async () => {
    //
    //     const response = await apiService.postQuestion({
    //         text: "localName",
    //         description: "localDescription",
    //         questionType: "questionType",
    //         questionData: [],
    //         correctAnswer: "correctAnswer"
    //     });
    //
    //     if (response instanceof Error) {
    //         return;
    //     }
    // };

    return (
        <div className="test__body">
            {/*<Button onClick={qwe}>Отправить вопрос</Button>*/}
            <ul className="list__questions">
                <li className="question">
                    <div className="ellipsis">
                        <svg viewBox="0 0 14 9" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path
                                d="M1.744 0.325114C1.97867 0.325114 2.2 0.367136 2.408 0.451326C2.616 0.535589 2.8 0.647018 2.96 0.785636C3.12 0.924303 3.24533 1.09021 3.336 1.28335C3.42667 1.47649 3.472 1.68202 3.472 1.89987C3.472 2.11785 3.42667 2.3233 3.336 2.51645C3.24533 2.70959 3.12 2.87797 2.96 3.02159C2.8 3.16521 2.616 3.27911 2.408 3.3633C2.2 3.44749 1.97867 3.48959 1.744 3.48959C1.50933 3.48959 1.28533 3.44749 1.072 3.3633C0.858667 3.27911 0.672 3.16521 0.512 3.02159C0.352 2.87797 0.226667 2.70959 0.136 2.51645C0.0453333 2.3233 0 2.11778 0 1.89987C0 1.68197 0.0453333 1.47652 0.136 1.28335C0.226667 1.09021 0.352 0.924303 0.512 0.785636C0.672 0.64697 0.858667 0.535542 1.072 0.451326C1.28533 0.367185 1.50933 0.325114 1.744 0.325114ZM7.00803 0.325114C7.25333 0.325114 7.48 0.367136 7.68803 0.451326C7.896 0.535589 8.07733 0.647018 8.232 0.785636C8.38667 0.924254 8.50936 1.09021 8.6 1.28335C8.69069 1.47649 8.73603 1.68202 8.73603 1.89987C8.73603 2.11785 8.69069 2.3233 8.6 2.51645C8.50936 2.70959 8.38667 2.87797 8.232 3.02159C8.07733 3.16521 7.896 3.27911 7.68803 3.3633C7.48 3.44749 7.25333 3.48959 7.00803 3.48959C6.77333 3.48959 6.54933 3.44749 6.336 3.3633C6.12267 3.27911 5.93867 3.16521 5.784 3.02159C5.62933 2.87797 5.50667 2.70959 5.416 2.51645C5.32533 2.3233 5.28 2.11778 5.28 1.89987C5.28 1.68197 5.32533 1.47652 5.416 1.28335C5.50667 1.09021 5.62933 0.924303 5.784 0.785636C5.93867 0.64697 6.12267 0.535542 6.336 0.451326C6.54933 0.367185 6.77333 0.325114 7.00803 0.325114ZM12.272 0.369685C12.5067 0.369685 12.7253 0.409305 12.928 0.488543C13.1307 0.56778 13.3067 0.676733 13.456 0.8154C13.6053 0.954066 13.7227 1.11749 13.808 1.30568C13.8933 1.49388 13.936 1.69197 13.936 1.89995C13.936 2.10795 13.8933 2.30607 13.808 2.49423C13.7227 2.68242 13.6053 2.84585 13.456 2.98452C13.3067 3.12318 13.1307 3.23209 12.928 3.31138C12.7253 3.39061 12.5067 3.43023 12.272 3.43023C12.048 3.43023 11.8346 3.39061 11.632 3.31138C11.4294 3.23209 11.2507 3.12318 11.096 2.98452C10.9414 2.84585 10.8213 2.68242 10.736 2.49423C10.6507 2.30604 10.608 2.10795 10.608 1.89995C10.608 1.69202 10.6507 1.49385 10.736 1.30568C10.8213 1.11752 10.9413 0.954066 11.096 0.8154C11.2507 0.676733 11.4294 0.56778 11.632 0.488543C11.8347 0.409305 12.048 0.369685 12.272 0.369685ZM1.744 5.51023C1.97867 5.51023 2.2 5.55233 2.408 5.63652C2.616 5.72071 2.8 5.83214 2.96 5.9708C3.12 6.10947 3.24533 6.27538 3.336 6.46852C3.42667 6.66166 3.472 6.86719 3.472 7.08509C3.472 7.303 3.42667 7.50852 3.336 7.70166C3.24533 7.8948 3.12 8.06319 2.96 8.2068C2.8 8.35042 2.616 8.46433 2.408 8.54852C2.2 8.63271 1.97867 8.6748 1.744 8.6748C1.50933 8.6748 1.28533 8.63271 1.072 8.54852C0.858667 8.46433 0.672 8.35042 0.512 8.2068C0.352 8.06319 0.226667 7.8948 0.136 7.70166C0.0453333 7.50852 0 7.303 0 7.08509C0 6.86719 0.0453333 6.66166 0.136 6.46852C0.226667 6.27538 0.352 6.10947 0.512 5.9708C0.672 5.83214 0.858667 5.72071 1.072 5.63652C1.28533 5.55233 1.50933 5.51023 1.744 5.51023ZM7.00803 5.51023C7.25333 5.51023 7.48 5.55233 7.68803 5.63652C7.896 5.72071 8.07733 5.83214 8.232 5.9708C8.38667 6.10947 8.50936 6.27538 8.6 6.46852C8.69069 6.66166 8.73603 6.86719 8.73603 7.08509C8.73603 7.303 8.69069 7.50852 8.6 7.70166C8.50936 7.8948 8.38667 8.06319 8.232 8.2068C8.07733 8.35042 7.896 8.46433 7.68803 8.54852C7.48 8.63271 7.25333 8.6748 7.00803 8.6748C6.77333 8.6748 6.54933 8.63271 6.336 8.54852C6.12267 8.46433 5.93867 8.35042 5.784 8.2068C5.62933 8.06319 5.50667 7.8948 5.416 7.70166C5.32533 7.50852 5.28 7.303 5.28 7.08509C5.28 6.86719 5.32533 6.66166 5.416 6.46852C5.50667 6.27538 5.62933 6.10947 5.784 5.9708C5.93867 5.83214 6.12267 5.72071 6.336 5.63652C6.54933 5.55233 6.77333 5.51023 7.00803 5.51023ZM12.272 5.51023C12.5173 5.51023 12.744 5.55233 12.952 5.63652C13.16 5.72071 13.3414 5.83214 13.4961 5.9708C13.6507 6.10947 13.7733 6.27538 13.8641 6.46852C13.9547 6.66166 14 6.86719 14 7.08509C14 7.303 13.9547 7.50852 13.8641 7.70166C13.7733 7.8948 13.6507 8.06319 13.4961 8.2068C13.3414 8.35042 13.16 8.46433 12.952 8.54852C12.744 8.63271 12.5173 8.6748 12.272 8.6748C12.0373 8.6748 11.8133 8.63271 11.6 8.54852C11.3866 8.46433 11.2027 8.35042 11.048 8.2068C10.8933 8.06319 10.7707 7.8948 10.6801 7.70166C10.5893 7.50852 10.5441 7.303 10.5441 7.08509C10.5441 6.86719 10.5893 6.66166 10.6801 6.46852C10.7707 6.27538 10.8933 6.10947 11.048 5.9708C11.2027 5.83214 11.3866 5.72071 11.6 5.63652C11.8133 5.55233 12.0373 5.51023 12.272 5.51023Z"
                                fill="#010002"/>
                        </svg>
                    </div>
                    <nav className="question-type-nav">
                        <div className="question__title">
                            <input type="text" className="question__title" placeholder="Новый заголовок"/>
                        </div>
                        <img src="./img/image.png"/>

                        <select className="question__type">
                            <option selected>
                                Один из списка
                            </option>
                            <option>
                                Несколько из списка
                            </option>
                        </select>
                    </nav>
                    <nav className="question-nav">
                        <ul className="question-nav__list">
                            <li className="question-nav__item">
                                <button className="text__type bold">
                                    <img src="./img/bold.png"/>
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
                    <ul className="list__answers">
                        <li className="answer">
                                <span className="answer__prepend radio">
                                    <input type="radio" name="radio"/>
                                </span>
                            <div className="answer__title">
                                <input type="text" className="answer__title" placeholder="Новый заголовок"/>
                            </div>
                            <span className="answer__append">
                                    <img src="./img/image.png"/>
                                    <img src="./img/delete.png"/>
                                </span>
                        </li>
                        <li className="answer">
                                <span className="answer__prepend radio">
                                    <input type="radio" name="radio"/>
                                </span>
                            <div className="answer__title">
                                <input type="text" className="answer__title" placeholder="Добавить вариант"/>
                            </div>
                            <span className="answer__append">
                                    <img src="./img/image.png"/>
                                    <img src="./img/delete.png"/>
                                </span>
                        </li>
                    </ul>
                    <footer className="test__footer">
                        <div className="test__actions">
                            <div className="action copy">
                                <img src="./img/paper.png"/>
                            </div>
                            <div className="action delete">
                                <img src="./img/trash.png"/>
                            </div>
                            <div className="vertical"></div>
                            <div className="required">
                                Обязательный вопрос
                            </div>

                            <div className="action required">
                                <label className="switch">
                                    <input type="checkbox"/>
                                    <span className="slider round"></span>
                                </label>
                            </div>
                            <div className="other">
                                <img src="./img/other.png"/>
                            </div>
                        </div>
                    </footer>
                </li>
            </ul>
        </div>
    )
}
