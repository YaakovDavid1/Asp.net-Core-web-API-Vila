using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vila.Data.Entities
{
    [Table("CheckListVila")]
    public class CheckListVilaEO
    {
        [Key]
        public int VilaId { get; set; }
        public bool HairDryer { get; set; }
        public bool CleaningProducts { get; set; }
        public bool Shampoo { get; set; }
        public bool Conditioner { get; set; }
        public bool BodySoap { get; set; }
        public bool ShowerGel { get; set; }

        public bool WashingMachine { get; set; }
        public bool ClothesDdryer { get; set; }
        public bool Towels { get; set; }
        public bool BedLinen { get; set; }
        public bool Soap { get; set; }
        public bool ToiletPaper { get; set; }
        public bool Hangers { get; set; }
        public bool Iron { get; set; }
        public bool ClothingStorage { get; set; }

        public bool Crib { get; set; }
        public bool ChildrensBooks { get; set; }
        public bool Wifi { get; set; }
        public bool Terrace { get; set; }
        public bool Pool { get; set; }
        public bool HeatedPool { get; set; }
        public bool PoolTable { get; set; }
        public bool Barbecue { get; set; }

        public bool Refrigerator { get; set; }
        public bool Microwave { get; set; }
        public bool CookingBasics { get; set; }
        public bool Freezer { get; set; }
        public bool WineGlasses { get; set; }
        public bool CoffeeMachine { get; set; }
        public virtual VillaDetailsEO VillaDetails { get; set; }

    }
}
