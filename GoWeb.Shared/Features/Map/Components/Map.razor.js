export async function initMapBookStyle(container, latCity, lonCity)
{
    console.log("JS: 1. Функция вызвана, получаем координаты: ", latCity, lonCity);

    if (typeof window.ymaps3 === 'undefined') {
        console.log("JS: 2. Начинаем загрузку скрипта Яндекса");
        try {
            await new Promise((resolve, reject) => {
                const script = document.createElement('script');
                // Твой рабочий ключ API
                script.src = "https://api-maps.yandex.ru/v3/?apikey=7dab67a6-408d-45e0-b597-70c61e0f4951&lang=ru_RU";
                script.async = true;
                script.onload = () => resolve();
                script.onerror = () => reject(new Error("Не удалось загрузить скрипт"));
                document.head.appendChild(script);
            });
        } catch (e) {
            console.error("Ошибка загрузки API Яндекса:", e);
            return null;
        }
    }

    console.log("JS: 3. Ждем готовности ymaps3.ready...");
    await window.ymaps3.ready;

    const { YMap, YMapDefaultSchemeLayer } = window.ymaps3;

    const map = new YMap(container, {
        location: {
            center: [lonCity, latCity],
            zoom: 10
        }
    });

    map.addChild(new YMapDefaultSchemeLayer());

    console.log("JS: 4. Карта успешно отрисована!");
    return map;
}