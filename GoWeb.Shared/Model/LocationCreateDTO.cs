using GoWeb.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoWeb.Shared.Model
{
    public class LocationCreateDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите адресс события")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Адресс должен быть от 2 до 500 символов.")]
        [Display(Name = "Адресс события")]
        public string Address { get; set; } = default!;

        [Required(ErrorMessage = "Пожалуйста,выберите город события")]

        [Display(Name = "Город события")]
        public int CityId { get; set; }


        [Required(ErrorMessage = "Пожалуйста, введите широту")]
        [Display(Name = "Широта")]
        public double LocationLatitude { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите долготу")]
        [Display(Name = "Долгота")]
        public double LocationLongitude { get; set; }

        [Display(Name = "Описание локации")]
        public string? LocationDescription { get; set; }

        public List<string>? imagesPaths { get; set; }

    }
}
