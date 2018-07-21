using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models

{
    //padrao é plural
    [Table("Restaurants")]
    public class Restaurant
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        //relacionamento
        public ICollection<Dish> Dishes { get; set; }

        public Restaurant()
        {
            Dishes = new Collection<Dish>();
        }
    }
}
