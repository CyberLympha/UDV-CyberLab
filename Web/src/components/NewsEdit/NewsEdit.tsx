import React from "react";
import {FormControl, FormLabel, Input, Textarea} from "@chakra-ui/react";
import {useNavigate, useParams} from "react-router-dom";

import {Button} from "../Button/Button";
import {apiService} from "../../services";

import style from "./../NewsAdd/NewsAdd.module.scss"
import {News} from "../../../api";


export function NewsEdit() {
    const {id} = useParams<{ id: string }>()
    const [newItem, setNewItem] = React.useState<News>()
    const navigate = useNavigate();

    React.useEffect(() => {
        const getNewItem = async () => {
            if (!id) return;

            const response = await apiService.getNewItem({id: id})
            if (response instanceof Error) {
                return;
            }
            setNewItem(response)
        }
        void getNewItem()
    }, [id])

    const editNewItem = async () => {
        if (!newItem?.title || !newItem.text || !newItem.createdAt || !newItem.id) return;
        const response = await apiService.editNewItem({
            title: newItem?.title,
            text: newItem.title,
            createdAt: newItem.createdAt,
            id: newItem.id
        })
        if (response instanceof Error) {
            return;
        }

        navigate("/news")
    }

    if (!newItem) return <></>

    return (
        <div className={style.container}>
            <div className={style.form}>
                <FormControl>
                    <FormLabel>Заголовок новости</FormLabel>
                    <Input width={"350px"} type='text' value={newItem?.title}
                           onChange={e => setNewItem({...newItem, title: e.target.value})}/>
                    <FormLabel>Текст новости</FormLabel>
                    <Textarea width={"750px"} height={"500px"} size={"lg"} value={newItem?.text}
                              onChange={e => setNewItem({...newItem, text: e.target.value})}/>
                </FormControl>
            </div>
            <Button onClick={editNewItem}>Изменить новость</Button>
        </div>
    )
}