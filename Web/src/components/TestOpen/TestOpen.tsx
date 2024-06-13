import style from "./TestOpen.module.scss"
import { Test } from "../../../api"
import { Link } from 'react-router-dom'


export function TestOpen({id, name, description}: Test) {
    return (
        <div className={style.container}>
            <Link to={`/tests/${id}/questions`}>
                <div className={style.title}>{name}</div>
            </Link>
            <div className={style.text}>{description}</div>
        </div>
    );
}