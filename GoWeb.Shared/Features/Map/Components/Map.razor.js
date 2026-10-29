export async function initMapBookStyle(container, latCity, lonCity)
{
    console.log("JS: 1. Функция вызвана, получаем координаты: ", latCity, lonCity);

    if (typeof window.ymaps3 === 'undefined') {
        console.log("JS: 2. Начинаем загрузку скрипта Яндекса");
        try {
            await new Promise((resolve, reject) => {
                const script = document.createElement('script');
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

    const { YMap, YMapDefaultSchemeLayer,YMapDefaultFeaturesLayer } = window.ymaps3;

    const map = new YMap(container, {
        location: {
            center: [lonCity, latCity],
            zoom: 10
        }
    });

    map.addChild(new YMapDefaultSchemeLayer());
    map.addChild(new YMapDefaultFeaturesLayer());

    console.log("JS: 4. Карта успешно отрисована!");
    return map;
}


export async function addMarkers(map, points, mapComponent)
{
    if (!map)
    {
        console.warn("Карта еще не готова. Ждем инициализации...");
        return;
    }
    await window.ymaps3.ready;
    const { YMap, YMapDefaultSchemeLayer, YMapMarker, YMapLayer } = window.ymaps3;
    const { YMapClusterer, clusterByGrid } = await window.ymaps3.import('@yandex/ymaps3-clusterer@0.0.1');

    const content = document.createElement('div');
    content.style.width = '14px';
    content.style.height = '14px';
    content.style.backgroundColor = '#007bff'; 
    content.style.borderRadius = '50%'; 
    content.style.border = '2px solid white';
    content.style.boxShadow = '0 1px 3px rgba(0,0,0,0.3)';

    const dotElement = document.createElement('div');
    dotElement.style.width = '20px';
    dotElement.style.height = '20px';
    dotElement.style.backgroundColor = '#ff0000';
    dotElement.style.borderRadius = '50%';
    dotElement.style.border = '2px solid white';
    dotElement.style.boxShadow = '0 2px 4px rgba(0,0,0,0.3)'; 

    const marker = (feature) => {
        return new window.ymaps3.YMapMarker(
                                            {
                                                coordinates: feature.geometry.coordinates,
                                                onClick() { mapComponent.invokeMethodAsync("OpenEventInMap", feature.id); }
                                            },
                                            content.cloneNode(true)
                                           );
    }
    const cluster = (coordinates, features) => {
        
        return new window.ymaps3.YMapMarker({coordinates},  dotElement );
    }

    const clusterer = new YMapClusterer({
        method: clusterByGrid({ gridSize: 64 }),
        features: points,
        marker,
        cluster
    });
    map.addChild(clusterer);
        
}