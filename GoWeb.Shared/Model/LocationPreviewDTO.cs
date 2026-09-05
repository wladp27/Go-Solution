using GoWeb.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoWeb.Shared.Model
{
    public class LocationPreviewDTO
    {
        public int Id { get; set; }

        [Display(Name = "Адресс события")]
        public string Address { get; set; }


        [Display(Name = "Город события")]
        public int? CityId { get; set; }

        [Display(Name = "Широта")]
        public double LocationLatitude { get; set; }


        [Display(Name = "Долгота")]
        public double LocationLongitude { get; set; }



    }



}

