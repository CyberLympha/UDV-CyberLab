import React from "react";
import style from "../NewTest/NewTest.module.scss";

export interface NewItemProps {
    id?: string;
    text?: string;
    description?: string;
    questionType?: string;
    correctAnswer?: string;
    questionData?: string[];
}

export function NewQuestion({id, text, description, questionType, correctAnswer, questionData}: NewItemProps) {
    return (
        <div className={style.container}>
            <div className={style.title}>{text}</div>
            <div className={style.text}>{description}</div>
        </div>
    );
}