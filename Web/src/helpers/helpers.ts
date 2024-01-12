import {UserRole} from "../../api";

export function getReadableVmStatus(status?: string | null) {
    if (status === "stopped") return "Остановлена";
    if (status === "running") return "Запущена";
    return "Неизвестно"
}

export function getReadableUserRole(role?: UserRole) {
    switch (role) {
        case UserRole.Admin:
            return "Админ"
        case UserRole.User:
            return "Студент"
        case UserRole.Teacher:
            return "Учитель"
        default:
            return "Студент"
    }
}