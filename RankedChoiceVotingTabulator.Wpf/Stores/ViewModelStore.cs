namespace RankedChoiceVotingTabulator.Wpf.Stores
{
    public class ViewModelStore
    {
        public ViewModelStore()
        {
            HomeViewModel = null;
        }

        public ViewModelStore(HomeViewModel homeViewModel)
        {
            HomeViewModel = homeViewModel;
        }

        public HomeViewModel? HomeViewModel { get; set; }
    }
}
