import React from "react";
import {Button as UiButton, ButtonProps as UiProps} from '@chakra-ui/react'

interface ButtonProps {
    onClick: () => void;
    isLoading?: boolean;
    children: React.ReactNode
}

export function Button({children, onClick, isLoading, ...props}: ButtonProps & UiProps) {
    return <UiButton onClick={onClick} isLoading={isLoading} {...props}>
        {children}
    </UiButton>
}
