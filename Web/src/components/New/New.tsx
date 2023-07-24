import {useNavigate} from "react-router-dom";

import {Button} from "../Button/Button";

export interface NewItemProps {
    id: string;
    title: string;
    text: string;
    createdAt: string;
}

import style from "./New.module.scss"
import {userStore} from "../../stores";
import {UserRole} from "../../../api";

export function New({id, title, text, createdAt}: NewItemProps) {
    const navigate = useNavigate();

    return (
        <div className={style.container}>
            {userStore.user?.role === UserRole.Admin &&
                <Button className={style.editButton} onClick={() => navigate(`/news/${id}/edit`)}>Изменить
                    новость</Button>}
            <div className={style.title}>{title}</div>
            <div className={style.text}>{text}</div>
            <time className={style.time}>{new Date(createdAt).toLocaleString([], {
                dateStyle: 'short',
                timeStyle: "short"
            })}</time>
        </div>
    );
}