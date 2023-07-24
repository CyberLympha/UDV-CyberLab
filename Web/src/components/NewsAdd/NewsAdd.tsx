import {FormControl, FormLabel, Input, Textarea} from "@chakra-ui/react";
import React from "react";

import style from "./NewsAdd.module.scss"
import newStyle from "../New/New.module.scss"
import {Button} from "../Button/Button";
import {apiService} from "../../services";
import {useNavigate} from "react-router-dom";


interface NewsAddProps {
    title?: string,
    text?: string,
}

export function NewsAdd({title, text}: NewsAddProps) {
    const [localTitle, setLocalTitle] = React.useState(title)
    const [content, setContent] = React.useState(text)
    const navigate = useNavigate();
    const addNew = async () => {
        if (!localTitle || !content) return;

        const response = await apiService.createNewItem({
            title: localTitle,
            text: content,
            createdAt: new Date().toISOString()
        })
        if (response instanceof Error) {
            return;
        }
        navigate("/news")
    }
    return (
        <div className={style.container}>
            <div className={style.form}>
                <Button isDisabled={!localTitle || !content} onClick={addNew}>Создать новость</Button>
                <FormControl>
                    <FormLabel>Заголовок новости</FormLabel>
                    <Input width={"350px"} type='text' autoComplete={"off"}
                           onChange={e => setLocalTitle(e.target.value)}/>
                    <FormLabel>Текст новости</FormLabel>
                    <Textarea width={"750px"} height={"500px"} size={"lg"} onChange={e => setContent(e.target.value)}/>
                </FormControl>
            </div>
        </div>
    )
}