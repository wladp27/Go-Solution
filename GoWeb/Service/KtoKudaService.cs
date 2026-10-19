using GoWeb.Shared.Model;
using System.Text.RegularExpressions;
using AngleSharp;
using GoWeb.Shared.Сonstants;
using GoWeb.Shared.Models;
namespace GoWeb.Service
{
    public class KtoKudaService
    {



        public async Task<List<EventSummaryDTO>> GetEvent()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string categoryUrl = "https://smr.ktokyda.ru/our-events/voleybol";
            string myCookies = "_ym_uid=1784983399424533810; _ym_d=1790424490; tmr_lvid=fd7cf9ffb6c4663013d556c02ae3b9f9; tmr_lvidTS=1762679731105; _ym_isad=1; myCity=show; PHPSESSID=91471e4f9e9c8baa36c428b23afc3f46; OCSESSID=ac6ea777788ed7a1e09ff1c07a; language=ru-ru; currency=USD; domain_sid=Ouf1GyDs7X-wbGeGYeGvM%3A1791823444604; tmr_detect=0%7C1791824031092";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Cookie", myCookies);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

            string htmlContent;
            try
            {
                htmlContent = await client.GetStringAsync(categoryUrl);
            }
            catch (Exception ex)
            {
                return new();
            }


            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(htmlContent));

            var eventCards = document.QuerySelectorAll(".ds-category-products .product-layout");

            if (eventCards.Length == 0)
            {
                return new();
            }

            List<EventSummaryDTO> eventsList = new List<EventSummaryDTO>();

            foreach (var card in eventCards)
            {
                string dataPid = card.GetAttribute("data-pid");
                string eventDateStr = card.GetAttribute("data-date");
                string rawPrice = card.GetAttribute("data-price");

                string title = card.QuerySelector(".ds-module-title")?.TextContent.Trim();
                string address = card.QuerySelector(".ds-module-code")?.TextContent.Trim();
                string timeText = card.QuerySelector(".date")?.TextContent.Trim();
                string status = card.QuerySelector(".ds-module-stock")?.TextContent.Trim();
                string imagePath = card.QuerySelector(".ds-module-img-box img")?.GetAttribute("src");
                string href = card.QuerySelector("a.ds-module-title")?.GetAttribute("href")
                           ?? card.QuerySelector("a")?.GetAttribute("href");
                int? realId = null;
                if (!string.IsNullOrEmpty(href))
                {
                    var match = Regex.Match(href, @"product_id_int=(\d+)");
                    if (match.Success)
                    {
                        realId = int.Parse(match.Groups[1].Value);
                    }
                    else
                    {
                        var fallbackMatch = Regex.Match(href, @"/(\d+)(?:\?|$)");
                        if (fallbackMatch.Success)
                        {
                            realId = int.Parse(fallbackMatch.Groups[1].Value);
                        }
                    }
                }

                decimal.TryParse(rawPrice, out decimal price);
                DateTimeOffset startTime = ParseDateTime(eventDateStr, timeText);

                var dto = new EventSummaryDTO
                {
                    Id = 0, // Настоящий цифровой ID для записи
                    EventName = title ?? "Без названия",
                    EventDescription = $"Оригинальный GUID: {dataPid}. Адрес: {address}. Статус: {status}",
                    RequiredRating = 0,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(2), // Стандартно +2 часа на игру
                    MinParticipants = 1,
                    MaxParticipants = 14,
                    Price = price,
                    EventTypeId = 1,
                    StatusEventId = 1, // Например, 1 = Published
                    ImagePath = imagePath
                };

                eventsList.Add(dto);
            }
            return eventsList;
            //// Выводим результат
            //Console.WriteLine($"Успешно спарсили {eventsList.Count} событий (за 1 быстрый запрос)!\n");
            //foreach (var ev in eventsList)
            //{
            //    string idDisplay = ev.Id.HasValue ? ev.Id.ToString() : "ID НЕ НАЙДЕН (обнови куки!)";
            //    Console.WriteLine($"[ID: {idDisplay}] {ev.EventName}");
            //    Console.WriteLine($"Дата: {ev.StartTime:dd.MM.yyyy HH:mm} | Цена: {ev.Price} руб.");
            //    Console.WriteLine($"Описание: {ev.EventDescription}");
            //    Console.WriteLine(new string('-', 50));
            //}
        }

        // Вспомогательный метод для склеивания даты из атрибута и времени из текста
        static DateTimeOffset ParseDateTime(string dateStr, string timeText)
        {
            try
            {
                DateTime baseDate = string.IsNullOrEmpty(dateStr) ? DateTime.Today : DateTime.Parse(dateStr);
                var match = Regex.Match(timeText ?? "", @"\d{2}:\d{2}");

                if (match.Success)
                {
                    TimeSpan time = TimeSpan.Parse(match.Value);
                    // UTC+4 для Самары. Замени на свой часовой пояс при необходимости.
                    return new DateTimeOffset(baseDate.Add(time), TimeSpan.FromHours(4));
                }
                return new DateTimeOffset(baseDate, TimeSpan.Zero);
            }
            catch
            {
                return DateTimeOffset.Now.AddDays(1);
            }
        }
    }
}
