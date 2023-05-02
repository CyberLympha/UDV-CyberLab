export function getReadableVmStatus(status?: string | null) {
    if (status === "stopped") return "Остановлена";
    if (status === "running") return "Запущена";
    return "Неизвестно"
}
