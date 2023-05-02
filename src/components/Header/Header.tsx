import {Button} from "../Button/Button";
import {userStore} from "../../stores";
import {observer} from "mobx-react-lite";
import {LoginForm} from "../AuthForm/LoginForm";
import React from "react";

export const Header = observer(() => {


  const user = userStore.user?.role;

  return (<>
    {user}
    {
    }

  </>);
})
