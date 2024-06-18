using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Batman.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace Batman
{
    public partial class BlackJackViewModel : ObservableObject
    {
       
        public IRelayCommand HitCommand { get; private set; }
        public IRelayCommand StandCommand { get; private set; }

        public IRelayCommand FoldCommand {  get; private set; }

        private readonly IDeck deck_;
        private readonly IPlayer player_;
        private readonly IDealer dealer_;
        private readonly Casino casion_;



        public BlackJackViewModel()
        {
            deck_ = new Deck();
            player_ = new Player();
            dealer_ = new Dealer();
            casion_ = new Casino(deck_, player_,dealer_);
            InitializeCommands();
        }
        private void CanClick()

        => casion_.Is_Hand_for_Splitting(player_.Hand_);


        private void Click_On_Hit()
        {
            casion_.TakeActions("HIT");
        }
     
        private void Click_On_Stand()
        {
            casion_.TakeActions("STAND");
        }

        private void Click_On_Fold()
        {
            casion_.TakeActions("FOLD");
        }
        private void InitializeCommands()
        {
            HitCommand = new RelayCommand(Click_On_Hit);
            StandCommand = new RelayCommand(Click_On_Stand);
            FoldCommand = new RelayCommand(Click_On_Fold);
        }

           /*private bool CanClick()
               => FirstName == "Kevin";*/


        /*        public event PropertyChangedEventHandler? PropertyChanged; //Everytime some of the propertie changes it says okey I am gonna update the context

                [RelayCommand]
                private void Click()
                {
                    FirstName = "Robert";
                }*/
        /*private readonly IDeck deck_;
        private readonly IPlayer player_;
        private readonly IDealer dealer_;
        private readonly Casino casino_;
        public string? GameStatus { get; set; }
        public ICommand HitCommand { get; private set; }
        public ICommand StandCommand { get; private set; }
        public ICommand FoldCommand { get; private set; }
        public ICommand DoubleCommand { get; private set; }
        public ICommand SplitCommand { get; private set; }

        public BlackJackViewModel()
        {
            deck_ = new Deck();
            player_ = new Player();
            dealer_ = new Dealer();
            casino_ = new Casino(deck_, player_, dealer_);
            InitializeCommands();
          
        }

        private void InitializeCommands()
        {
            HitCommand = new Command(ExecuteHit);
            StandCommand = new Command(ExecuteStand);
            FoldCommand = new Command(ExecuteFold);
            DoubleCommand = new Command(ExecuteDouble);
            SplitCommand = new Command(ExecuteSplit);
        }

        private void ExecuteHit()
        {
            casino_.TakeActions("HIT");
            UpdateHand();
        }

        private void ExecuteStand()
        {
            casino_.TakeActions("STAND");
            UpdateHand();
        }

        private void ExecuteFold()
        {
            casino_.TakeActions("FOLD");
            UpdateHand();
        }

        private void ExecuteDouble()
        {
            casino_.TakeActions("DOUBLE");
            UpdateHand();
        }

        private void ExecuteSplit()
        {
            casino_.TakeActions("SPLIT");
            UpdateHand();
        }

        private void UpdateHand()
        {
            OnPropertyChanged(nameof(player_.Hand_));
            OnPropertyChanged(nameof(dealer_.HiddenCards));
        }

        private void StartNewRound()
        {
            casino_.StartRound();
            UpdateHand();
        }
    }*/
    }
}