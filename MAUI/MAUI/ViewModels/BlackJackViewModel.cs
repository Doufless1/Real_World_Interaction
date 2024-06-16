using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using UIKit;
namespace MAUI
{
    public class BlackJackViewModel : BindableObject
    {
        private readonly IDeck deck_; //new Deck();
        private readonly IPlayer player_;// = new Player();
        private readonly IDealer dealer_;
        public string? GameStatus { get; set; }
        public ICommand HitCommand { get; private set; }
        public ICommand StandCommand { get; private set; }
        public ICommand FoldCommand { get; private set; }
        public ICommand DoubleCommand { get; private set; }
        public ICommand SplitCommand { get; private set; }

        /* public BlackJackViewModel(IDeck deck, IPlayer player, IDealer dealer)
         {
             deck_ = deck ?? throw new ArgumentNullException(nameof(deck));
             player_ = player ?? throw new ArgumentNullException(nameof(player));
             dealer_ = dealer ?? throw new ArgumentNullException(nameof(dealer));

             InitializeCommands();
         }*/

        public BlackJackViewModel()
        {
            deck_ = new Deck();
            player_ = new Player();
            dealer_ = new Dealer();
            HitCommand = new Command(ExecuteHit);
            HitCommand = new Command(ExecuteStand);
            HitCommand = new Command(ExecuteFold);
            HitCommand = new Command(ExecuteDouble);
            HitCommand = new Command(ExecuteSplit);
            InitializeCommands();
        }

        
        //public ICommand HitCommand { get; }
     
        private void ExecuteHit()
        {
            player_.Hand_.Add(deck_.DrawCard());
            OnPropertyChanged(nameof(player_.Hand_)); // just shwos that the value has been changed
        }
        private void ExecuteStand()
        {
            player_.Hand_.Add(deck_.DrawCard());
            OnPropertyChanged(nameof(player_.Hand_)); // just shwos that the value has been changed
        }
        private void ExecuteFold()
        {
            player_.Hand_.Clear();
            OnPropertyChanged(nameof(player_.Hand_)); // just shwos that the value has been changed
        }
        private void ExecuteDouble()
        {
            player_.Hand_.Add(deck_.DrawCard());
            OnPropertyChanged(nameof(player_.Hand_)); // just shwos that the value has been changed
        }
        private void ExecuteSplit()
        {
            player_.Hand_.Add(deck_.DrawCard());
            OnPropertyChanged(nameof(player_.Hand_)); // just shwos that the value has been changed
        }
    }
}