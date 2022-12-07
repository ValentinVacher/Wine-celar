﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wine_celar.Repositories;
using Wine_cellar.Entities;
using Wine_cellar.Repositories;

namespace Wine_cellar.Entities
{
    public class Appelation
    {
        [Key]
        public int AppelationId { get; set; }
        public string AppelationName { get; set; }
        public int KeepMin { get; set; }
        public int KeepMax { get; set; }
        public WineColor Color { get; set; }
        public List<Wine> Wines { get; set; }

        public bool IsApogee()
        {
            
            var ToDay = DateTime.Now;
            var result = ToDay.Year;
            if (result >= KeepMin && result <= KeepMax)
            {
                return true;
            }
            else
                return false;
        }
       
    }
}
