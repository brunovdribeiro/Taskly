import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import {Provider} from "react-redux";
import {apiStore} from "./stores/apiStore.ts";

createRoot(document.getElementById('root')!).render(
  <StrictMode>
      <Provider store={apiStore}>
            <App />
      </Provider>
  </StrictMode>,
)
