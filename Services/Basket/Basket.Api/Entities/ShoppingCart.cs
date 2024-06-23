namespace Basket.Api.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {

        }

        public ShoppingCart(string username)
        {
            UserName = username;
        }

        public string UserName { get; set; }

        public List<ShoppingCartItem> Items { get; set; }

        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;

                if (Items != null && Items.Any())
                {
                    foreach (var item in Items)
                    {
                        total += item.Price * item.Quantity;
                    }
                }

                return total;
            }
        }
    }
}
