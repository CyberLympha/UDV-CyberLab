

export function Variants ( { variantsType } : any ) {

    return (
        <li className="answer">
            <span className="answer__prepend radio">
                {/* <input type={variantsType} name={`${questionId}`} onClick={changeAnswer}/> */}
                <input type={variantsType} />
            </span>
            <div className="answer__title">
                <text type="text" className="answer__title"/>
            </div>
        </li>
    )
}