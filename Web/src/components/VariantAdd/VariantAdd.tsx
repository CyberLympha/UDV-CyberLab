import  "../TestsAdd/TestsAdd.css";


export function VariantAdd({ onChangeVariant, id } : any) {

    const changeVariant = (event : any) => {
        onChangeVariant(event.target.value, id);
    };

    return (
        <li className="answer">
            <span className="answer__prepend radio">
                <input type="radio" name="radio"/>
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
