namespace Quick.Order.AppCore.Models
{
    public class Currency
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }




        public override string ToString()
        {
            return $"{Name} {Symbol}";
        }


    }


}
