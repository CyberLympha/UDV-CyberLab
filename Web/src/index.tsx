import React from 'react'
import {ChakraProvider} from '@chakra-ui/react'
import ReactDOM from 'react-dom/client'
import {BrowserRouter} from "react-router-dom";

import {App} from './App'

import './index.scss'


ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
    <React.StrictMode>
        <ChakraProvider>
            <BrowserRouter>
                <App/>
            </BrowserRouter>
        </ChakraProvider>
    </React.StrictMode>,
)
