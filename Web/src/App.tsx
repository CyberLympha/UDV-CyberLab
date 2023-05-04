import React from 'react'
import './App.css'
import {Route, Routes} from "react-router-dom";
import {SideMenu} from "./components/SideMenu/SideMenu";
import {News} from "./components/News/News";
import {apiService} from "./services";
import {userStore} from "./stores";
import {Vms} from "./components/Vms/Vms";
import {NewVm} from "./components/NewVm/NewVm";
import {Vm} from "./components/Vm";

function App() {
    const [loading, setLoading] = React.useState(false)

    React.useEffect(() => {
        setLoading(true);

        const fetchUser = async () => {
            const response = await apiService.getCurrentUser();

            if (response instanceof Error) {
                setLoading(false)
                return;
            }
            userStore.setUser(response)
            setLoading(false)
        }

        void fetchUser()
    }, [])

    if (loading) return <div>Загрузка</div>

    return (
        <Routes>
            <Route path="/" element={<SideMenu/>}>
                <Route path={"/news"} element={<News/>}/>
                <Route path={"/vms"} element={<Vms/>}/>
                <Route path={"/vms/new"} element={<NewVm/>}/>
                <Route path={"/vms/:vmid"} element={<Vm/>}/>
            </Route>

        </Routes>
    )
}

export default App
