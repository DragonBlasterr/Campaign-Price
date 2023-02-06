using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace campaign_price
{
    class Product
    {
       public int Id { get; set; }
       public string Name { get; set; }
       public int Price { get; set; }

       //the three propoties should be filled when the product object
       //is created because not every product is on campaign 
       //public List<Tuple<int,int>> ComboProducts = new List<Tuple<int, int>>(); //[(1,10%),(2,20%)] the second value is how much procent discount do the costumer get if he combine with this product
       public int VolumePrice { get; set; }
       public int VolumeAmount { get; set; }
       public bool IsOnVolumeSale { get; set; }

       public Product(int id, string name, int price) 
       {
            Id = id;
            Name = name;
            Price = price;
       }
    }
}
