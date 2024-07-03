

export function Variants ( { variantsType, variant, questionId, variantId, onChangeAnswer } : any ) {

    const changeAnswer = () => {
        onChangeAnswer(variant, questionId, variantsType);
    };

    return (
        <li className="answer">
            <span className="answer__prepend radio">
                <input type={variantsType} name={`${questionId}`} onClick={changeAnswer} />
            </span>
            <div className="answer__title">
                <text type="text" className="answer__title">
                    {variant}
                </text>
            </div>
        </li>
    )
}