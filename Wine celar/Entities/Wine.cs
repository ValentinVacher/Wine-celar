﻿namespace Wine_celar.Entities
{
    public class Wine
    {
        public int WineId { get; set; }
        public string Color { get; set; }
        public string Appelation { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int KeepMin { get; set; }
        public int KeepMax { get; set;}
        public Drawer Drawer { get; set; }
        public int DrawerId { get; set; }
    }
}
