﻿using Wine_cellar.Entities;

namespace Wine_cellar.ViewModel
{
    public class CreateWineViewModel
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public int DrawerId{ get; set; }
        public IFormFile? Picture { get; set; }
        public int AppelationId { get; set; }
        public WineColor Color { get; set; } 
    }
}
