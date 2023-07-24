import {UserRole} from "../../api";

export function getReadableVmStatus(status?: string | null) {
    if (status === "stopped") return "Остановлена";
    if (status === "running") return "Запущена";
    return "Неизвестно"
}

export function getReadableUserRole(role?: UserRole) {
    switch (role) {
        case UserRole.Admin:
            return "Преподаватаель"
        case UserRole.User:
            return "Студент"
        default:
            return "Студент"
    }
}