﻿namespace Wine_celar.Entities
{
    public class Appelation
    {
        public int AppelationId { get; set; }
        public string AppelationName { get; set; }
        public int KeepMin { get; set; }
        public int KeepMax { get; set; }
        public List<Color> Color { get; set; }

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
