using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Model
{
    public class ValidationProblemDetails
    {
        public string? Type { get; set; }

        public string? Title { get; set; }

        public int? Status { get; set; }

        public string? Detail { get; set; }

        // URL эндпоинта, на котором произошла ошибка 
        public string? Instance { get; set; }

        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
