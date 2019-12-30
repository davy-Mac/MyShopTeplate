namespace MyShop.Core.ViewModels
{
    public class BasketSummaryViewModel
    {
        public int BasketCount { get; set; }
        public decimal BasketTotal { get; set; }

        public BasketSummaryViewModel() // this is an empty constructor to set default values
        {

        }

        public BasketSummaryViewModel(int basketCount, decimal basketTotal) // constructor of the basket summary or the total in the basket
        {
            this.BasketCount = basketCount;
            this.BasketTotal = basketTotal;
        }
    }
}
