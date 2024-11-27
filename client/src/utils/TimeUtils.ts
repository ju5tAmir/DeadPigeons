export function formatTime(seconds: number): string {
    const hours = Math.floor(seconds / 3600); // 3600 seconds in an hour
    const minutes = Math.floor((seconds % 3600) / 60); // Remaining minutes
    const secs = seconds % 60; // Remaining seconds

    // If there are hours, format as hh:mm:ss
    if (hours > 0) {
        return `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:${String(secs).padStart(2, '0')}`;
    }

    // If there are no hours, format as mm:ss
    return `${String(minutes).padStart(2, '0')}:${String(secs).padStart(2, '0')}`;
}


export function formatDate(isoString: string): string {
    const date = new Date(isoString);

    const formattedDate = date.toLocaleDateString(undefined, {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });

    const formattedTime = date.toLocaleTimeString(undefined, {
        hour: '2-digit',
        minute: '2-digit',
    });

    return `${formattedDate}, ${formattedTime}`;
}