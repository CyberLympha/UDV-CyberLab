import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import {ChakraProvider} from '@chakra-ui/react'
import './index.scss'
import {BrowserRouter,} from "react-router-dom";


ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <ChakraProvider>
      <BrowserRouter>
        <App/>
      </BrowserRouter>
    </ChakraProvider>
  </React.StrictMode>,
)
