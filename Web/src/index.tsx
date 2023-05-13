import React from 'react'
import {configure, spy} from 'mobx'

import {ChakraProvider} from '@chakra-ui/react'
import ReactDOM from 'react-dom/client'
import {BrowserRouter} from "react-router-dom";

import {App} from './App'

import './index.scss'

spy((ev) => {
    if (ev.type.includes('action')) {
        console.log(ev)
    }
})

configure({
    enforceActions: 'never',
    reactionScheduler: (f) => setTimeout(f),
})

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
    <React.StrictMode>
        <ChakraProvider>
            <BrowserRouter>
                <App/>
            </BrowserRouter>
        </ChakraProvider>
    </React.StrictMode>,
)
