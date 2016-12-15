using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        //below we state that navigation property City,
        //the foreign key on point of interest is name CityId
        [ForeignKey("CityId")]
        //Below is a navigation property, and a relationship
        //will be created between the two
        public City City { get; set; }
        public int CityId { get; set; }
    }
}
