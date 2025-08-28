// Храним ссылки на обработчики для возможности их удаления
const focusHandlers = new Map();

window.setupInputScroll = (selector) => {
    // Находим все поля ввода в форме
    const inputs = document.querySelectorAll(selector);

    inputs.forEach(input => {
        // Создаём обработчик фокуса
        const focusHandler = () => {
            setTimeout(() => {
                // Прокручиваем поле в центр видимой области
                input.scrollIntoView({ behavior: 'smooth', block: 'center' });

                // Дополнительный отступ, чтобы поле было выше клавиатуры
                const offset = 100; // Отступ в пикселях
                window.scrollBy({ top: -offset, behavior: 'smooth' });
            }, 300); // Задержка для открытия клавиатуры
        };

        // Сохраняем обработчик
        focusHandlers.set(input, focusHandler);

        // Добавляем обработчик события focus
        input.addEventListener('focus', focusHandler);
    });
};

window.cleanupInputScroll = (selector) => {
    // Находим все поля ввода
    const inputs = document.querySelectorAll(selector);

    inputs.forEach(input => {
        // Удаляем обработчик, если он существует
        const focusHandler = focusHandlers.get(input);
        if (focusHandler) {
            input.removeEventListener('focus', focusHandler);
            focusHandlers.delete(input);
        }
    });
};