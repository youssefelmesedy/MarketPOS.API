export function randomString(length) {
    const chars = 'Youssef_Mostafa_Elmesedy_Software_Developer';
    return Array.from({ length }, () => chars[Math.floor(Math.random() * chars.length)]).join('');
}

export function randomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}