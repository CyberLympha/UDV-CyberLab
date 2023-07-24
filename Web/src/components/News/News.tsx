import React from "react";
import {useNavigate} from "react-router-dom";

import {News as NewsItem, UserRole} from "../../../api"
import {apiService} from "../../services";
import {New} from "../New";
import {Button} from "../Button/Button";
import {userStore} from "../../stores";

import style from "./News.module.scss"
import {AiOutlinePlus} from "react-icons/all";
import InfiniteScroll from "react-infinite-scroll-component";


export function News() {
    const [news, setNews] = React.useState<NewsItem[]>([]);
    const navigate = useNavigate();
    const count = React.useRef(0);
    const hasMore = React.useRef(true);
    const createNewNewItem = async () => {
        navigate("/news/add")
    }

    const fetchMoreData = async () => {
        count.current += 10;
        const response = await apiService.getNews({offset: count.current});

        if (response instanceof Error) {
            return;
        }

        if (!response.length) {
            hasMore.current = false;
        }

        setNews([...news, ...response])
    };

    React.useEffect(() => {
        const fetch = async () => {
            const response = await apiService.getNews({offset: count.current});

            if (response instanceof Error) {
                return;
            }
            setNews(response)
        }
        void fetch();
    }, [])

    const addNewItemButton = <div className={style.addNewItem}>
        <Button rightIcon={<AiOutlinePlus/>} onClick={createNewNewItem}>Добавить новость</Button>
    </div>

    return (
        <div id={"news"} className={style.container}>
            {userStore.user?.role === UserRole.Admin && addNewItemButton}
            <InfiniteScroll
                dataLength={news.length}
                next={fetchMoreData}
                hasMore={hasMore.current}
                loader={<h4>Загружаем новости...</h4>}
            >
                {news?.map(newItem => <New key={"1"} {...newItem} />)}
            </InfiniteScroll>
        </div>
    )
}
