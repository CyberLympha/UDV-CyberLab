import  "../TestsAdd/TestsAdd.css";
import React, {useState} from "react";


export function VariantAdd({ onChangeVariant, onChangeAnswer, variantId, questionId, variantsType } : any) {

    const changeVariant = (event : any) => {
        onChangeVariant(event.target.value, variantId);
    };

    const changeAnswer = () => {
        onChangeAnswer(variantId);
    };

    return (
        <li className="answer">
            <span className="answer__prepend radio">
                <input type={variantsType} name={`${questionId}`} onClick={changeAnswer}/>
            </span>
            <div className="answer__title">
                <input onChange={changeVariant}
                       type="text" className="answer__title" placeholder="Новый заголовок"/>
            </div>
            <span className="answer__append">
                <img src="./img/image.png"/>
                <img src="./img/delete.png"/>
            </span>
        </li>
    )
}
