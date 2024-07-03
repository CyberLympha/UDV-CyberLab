import  "../TestsAdd/TestsAdd.css";


export function VariantAdd({ onChangeVariant, onChangeAnswer, onDeleteVariant,
                            variantId, questionId, variantsType } : any) {

    const changeVariant = (event : any) => {
        onChangeVariant(event.target.value, variantId);
    };

    const changeAnswer = () => {
        onChangeAnswer(variantId);
    };

    const deleteVariant = () => {
        onDeleteVariant(variantId);
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
                <button onClick={deleteVariant}>
                    <img src="./img/delete.png"/>
                </button>
            </span>
        </li>
    )
}