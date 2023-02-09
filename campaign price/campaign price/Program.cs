namespace campaign_price
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> ComboProducts = new List<int>();
            ComboProducts.Add(1);
            ComboProducts.Add(3); 
            ComboProducts.Add(4);
            ComboProducts.Add(5);

            List<Product> products = new List<Product>();
            products.Add(new Product(1, "cola", 200)); //inflation gör så att allting blir dyrt :)
            products.Add(new Product(2, "mat", 150));
            products.Add(new Product(3, "drick", 250));
            products.Add(new Product(4, "bröd", 500));
            products.Add(new Product(5, "vatten", 200));

            //products[0].ComboProducts.Add(Tuple.Create(products[1].Id, 20)); //if somone buy cola with drick the get a 20% discount
            products[4].IsOnVolumeSale = true;
            products[4].VolumeAmount = 2; // if you buy 2 vatten you will get a discount of 100 sek
            products[4].VolumePrice = 300;

            Console.WriteLine("what do you wnat to buy wirte the prodcut id:"); // you can write 1+3 for example then you will buy a cola and a drick or just 1
            var x = Console.ReadLine();
            string[] ProductsToBuy = x.Split("+");

            Dictionary<int,int> ProductsToBuyList = new Dictionary<int,int>();

            foreach (var p in ProductsToBuy)
            {
                int id = int.Parse(p);
                if (ProductsToBuyList.Any(product => product.Key == id)) //is this product id already in the list then increase the amount by one elese add it
                {
                    ProductsToBuyList[id] = ProductsToBuyList[id] + 1;
                }
                else
                {
                    ProductsToBuyList.Add(id, 1);
                }
            }
            
            var to_pay=0;
            var productsToBuyInComboPrice = ProductsToBuyList.Where(x => ComboProducts.Contains(x.Key)).ToList();
            if(productsToBuyInComboPrice.Count % 2 != 0) //if they buy 3 combo prdoucts (odd number) for example then remove the last product from ComboPrice 
            {
                productsToBuyInComboPrice.RemoveAt(productsToBuyInComboPrice.Count-1);
            }

            foreach (var p in ProductsToBuyList) //if you buy a product that are both in VolumePrice and ComboPrice at the same time then count the first one as a comboPrice and the rest as a VolumePrice
            {
                if (productsToBuyInComboPrice.Any(x => x.Key == p.Key)) //this product is a combo product
                {
                    var antal_p = productsToBuyInComboPrice.Find(x => x.Key == p.Key).Value;
                    productsToBuyInComboPrice.Remove(p);
                    var second_key = productsToBuyInComboPrice[0].Key;
                    var antal_second_key = productsToBuyInComboPrice.Find(x => x.Key == second_key).Value;

                    if(antal_p <= antal_second_key) //
                    {
                        if (antal_p % 2 == 0 || antal_p == 1)
                        {
                            ProductsToBuyList[p.Key] = ProductsToBuyList[p.Key] - antal_p;
                            ProductsToBuyList[second_key] = ProductsToBuyList[second_key] - antal_p;
                            to_pay += 30 * antal_p; //we have used two of our items as a combo price which are 30 for each two products
                        }
                        else
                        {
                            ProductsToBuyList[p.Key] = ProductsToBuyList[p.Key] - antal_p - 1;
                            ProductsToBuyList[second_key] = ProductsToBuyList[second_key] - antal_p - 1;
                            to_pay += 30 * (antal_p - 1); //we have used two of our items as a combo price which are 30 for each two products
                        }
                    }
                    else
                    {
                        if (antal_second_key % 2 == 0 || antal_second_key == 1)
                        {
                            ProductsToBuyList[p.Key] = ProductsToBuyList[p.Key] - antal_second_key;
                            ProductsToBuyList[second_key] = ProductsToBuyList[second_key] - antal_second_key;
                            to_pay += 30 * antal_second_key;
                        }
                        else
                        {
                            ProductsToBuyList[p.Key] = ProductsToBuyList[p.Key] - antal_second_key - 1;
                            ProductsToBuyList[second_key] = ProductsToBuyList[second_key] - antal_second_key - 1;
                            to_pay += 30 * (antal_second_key - 1);
                        }
                    }
                    productsToBuyInComboPrice.RemoveAt(0); //remove the first element from the list to expire two products in total
                    
                    if (ProductsToBuyList[p.Key] == 0) //no more items then remove it from the list
                    {
                        ProductsToBuyList.Remove(p.Key);
                    }
                    if (ProductsToBuyList[second_key] == 0) //no more items then remove it from the list
                    {
                        ProductsToBuyList.Remove(second_key);
                    }
                }

                if(products.Any(x => x.Id == p.Key))
                {
                    var product = products.Find(x => x.Id == p.Key);
                    if (product.IsOnVolumeSale)
                    {
                        if (ProductsToBuyList[p.Key] >= product.VolumeAmount)
                        {
                            ProductsToBuyList[p.Key] = ProductsToBuyList[p.Key] - product.VolumeAmount;
                            to_pay += product.VolumePrice;
                        }
                        if (ProductsToBuyList[p.Key] == 0) //no more items then remove it from the list
                        {
                            ProductsToBuyList.Remove(p.Key);
                        }
                    }
                }

                if (ProductsToBuyList.Any(x => x.Key == p.Key)) //there are multiple amount of this item still even after the we expired the comboitem 
                {
                    var product = products.Find(x => x.Id == p.Key);
                    to_pay += (product.Price)*(ProductsToBuyList[p.Key]); //add the price of the product * the amount that is left
                    ProductsToBuyList[p.Key] = ProductsToBuyList[p.Key] - 1;
                    if (ProductsToBuyList[p.Key] == 0) //no more items then remove it from the list
                    {
                        ProductsToBuyList.Remove(p.Key);
                    }
                }
            }
            Console.WriteLine(to_pay);

        }
    }
}
