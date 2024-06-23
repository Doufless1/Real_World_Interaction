using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Batman.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using System.Windows.Media;
using Batman.ViewModels;
namespace Batman
{
    public partial class BlackJackViewModel : ObservableObject
    {

        // private readonly CurrencyService currencyService_;

        private bool betplaced_ = false;
        private bool check_if_hit_was_pressed = false;
        private bool check_if_double_was_pressed = false;

        private int dealers_hand_value;

        private int player_hand_value;

        private int player_add_bet;

        private string _statusMessage;

        public IRelayCommand? DoubleCommand { get; private set; }
        public IRelayCommand? HitCommand { get; private set; }
        public IRelayCommand? StandCommand { get; private set; }

        public IRelayCommand? FoldCommand { get; private set; }

        public IRelayCommand? Start_the_game { get; private set; }

        public IRelayCommand? Bet_10_Euro { get; private set; }
        public IRelayCommand? Bet_20_Euro { get; private set; }

        public IRelayCommand? Bet_50_Euro { get; private set; }

        public IRelayCommand? Bet_100_Euro { get; private set; }

        public IRelayCommand? BetCommand { get; private set; }

        private readonly IDeck deck_;
        private readonly IPlayer player_;
        private readonly IDealer dealer_;
        private readonly Casino casino_;


        public ObservableCollection<BitmapImage> PlayerCardImages { get; set; }
        public ObservableCollection<BitmapImage> DealerCardImages { get; set; }

        
        public int Player_Add_Bet
        {
            get => player_add_bet;
            set => SetProperty(ref player_add_bet, value);
        }
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public int DealersHandValue
        {
            get => dealers_hand_value;
            set => SetProperty(ref dealers_hand_value, value);
        }
        //   OnPropertyChanged(nameof(StatusMessage));


        public int PlayerHandValue{
            get => player_hand_value;
            set => SetProperty(ref player_hand_value, value);
            }
        public BlackJackViewModel()
        {
            deck_ = new Deck();
            player_ = new Player();
            dealer_ = new Dealer();
            casino_ = new Casino(deck_, player_, dealer_);
            casino_.RoundEnded += Casino_RoundEnded;
            PlayerCardImages = new ObservableCollection<BitmapImage>();
            DealerCardImages = new ObservableCollection<BitmapImage>();
            //   CardImages2 = new ObservableCollection<ImageSource>();
            InitializeCommands();
            InitializeSkins();

            //      string imagePath = System.IO.Path.Combine(baseDir, "Pictures", $"{Suit}");
            //    TestImage = new BitmapImage(new Uri(imagePath,UriKind.Absolute));
        }
        private void Casino_RoundEnded(object? sender,string e)
        { 
       StatusMessage = e;

        }

        private BitmapImage GetCardImage(Card card)
        {
            string suit = card.Suit_ switch
            {
                Suit.Clubs => "cl",
                Suit.Diamonds => "di",
                Suit.Hearts => "he",
                Suit.Spades => "sp",
                _ => throw new InvalidOperationException("Invalid suit")
            };
            string value = card.Face_ switch
            {
                Face.Ace => "1",
                Face.Two => "2",
                Face.Three => "3",
                Face.Four => "4",
                Face.Five => "5",
                Face.Six => "6",
                Face.Seven => "7",
                Face.Eight => "8",
                Face.Nine => "9",
                Face.Ten => "10",
                Face.Jack => "j",
                Face.Queen => "q",
                Face.King => "k",
                _ => throw new InvalidOperationException("Invalid card value")

            };
            string filename = $"{suit}{value}.gif";
            string packUri = $"pack://application:,,,/Pictures/{filename}";

       
            Debug.WriteLine($"Loading image from: {packUri}");
            return new BitmapImage(new Uri(packUri));
        }
       
        private void UpdateCardDisplayForPlayer() 
        {
            PlayerCardImages.Clear();
            foreach (var card in player_.Hand_)
            {
                PlayerCardImages.Add(GetCardImage(card));
            }
            PlayerHandValue = player_.GetHandValue();
        }
        private void UpdateCardDisplayForDealer()
        {

            DealerCardImages.Clear();
            foreach (var card in dealer_.RevealedCards)
            {
                if (dealer_.RevealedCards.Count() == 1)
                {
                    DealerCardImages.Add(GetCardImage(card));
                   
                    InitializeSkins();
                } else{
                    DealerCardImages.Add(GetCardImage(card));
                   
                }
            }
            DealersHandValue = dealer_.GetHandValue();
        }

  
        private void On_Stand()
        {
            dealer_.RevealCard(dealer_.RevealedCards);
        }
        private void CanClick()

        => casino_.Is_Hand_for_Splitting(player_.Hand_);


        private void ResetFlags()
        {
            check_if_hit_was_pressed = false;
            check_if_double_was_pressed = false;
            betplaced_ = false;
        }

        void Check_If_Player_Bust_When_He_Gets_Card()
        {
            if (player_.GetHandValue() > 21)
            {
                casino_.EndRound(RoundResult.PLAYER_BUST);
            }
        }
        private void Click_On_Double()
        {
            try
            {
                if (check_if_double_was_pressed)
                {
                    throw new InvalidOperationException("You can't use Double Twice!");
                }
                if (!betplaced_)
                {
                    throw new InvalidOperationException("Place a bet first!");
                }
                if (check_if_hit_was_pressed)
                {
                    throw new InvalidOperationException("You can't Double when you Hitted!");
                }
                if (player_.GetHandValue() >= 21)
                {
                    throw new InvalidOperationException("You are exceding Limits");
                }        
                    casino_.TakeActions("DOUBLE");
                    Check_If_Player_Bust_When_He_Gets_Card();
                    UpdateCardDisplayForPlayer();
                    check_if_double_was_pressed = true;
                
            } catch (Exception ex)
            {
                StatusMessage = $"{ex.Message}";
            }
        }

        private void Click_On_Hit()
        {
            try
            {
                if (check_if_double_was_pressed)
                {
                    throw new InvalidOperationException("You can't Hit after Double!");
                }
                if (player_.GetHandValue() < 21 )
                {
                    casino_.TakeActions("HIT");
                    Check_If_Player_Bust_When_He_Gets_Card();
                    UpdateCardDisplayForPlayer();
                    check_if_hit_was_pressed = true;
                } else
                {
                    throw new InvalidOperationException("You already BUSTED!");
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"{ex.Message}";
            }
        }

        private void Click_On_Stand()
        {
            try
            {
                On_Stand();

                UpdateCardDisplayForDealer();
                casino_.TakeActions("STAND");

                casino_.ProcessHand(player_.Hand_);
                UpdateCardDisplayForPlayer();

                UpdateCardDisplayForDealer();

                UpdateCardDisplayForPlayer();
            }
            catch (Exception ex)
            {
                StatusMessage = $"{ex.Message}";
            }
        }

        private void Click_On_Fold()
        { 
            casino_.TakeActions("FOLD");

            UpdateCardDisplayForPlayer();
            UpdateCardDisplayForDealer();
            dealer_.RevealedCards.Clear();
            Player_Add_Bet = 0;
        ResetFlags();
        }

        private void Start_the_Round_After_betting()
        {
            try
            {
                if (betplaced_)
                {
                    casino_.InitializeHand("BET");
                    UpdateCardDisplayForDealer();
                    UpdateCardDisplayForPlayer();
                }
                else
                {
                    throw new InvalidOperationException("Please put a bet First!");
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"{ex.Message}";
            }
        }
        private void InitializeCommands()
        {
            HitCommand = new RelayCommand(Click_On_Hit);
            StandCommand = new RelayCommand(Click_On_Stand);
            FoldCommand = new RelayCommand(Click_On_Fold);
            BetCommand = new RelayCommand(Start_the_Round_After_betting);
            DoubleCommand = new RelayCommand(Click_On_Double);
            Bet_10_Euro = new RelayCommand(Bet_10);
            Bet_50_Euro = new RelayCommand(Bet_50);
            Bet_20_Euro = new RelayCommand(Bet_20);
            Bet_100_Euro = new RelayCommand(Bet_100);
            
        }
       
        private void Bet_10()
        {
            casino_.TakeBet("10");
            Player_Add_Bet += 10;
            betplaced_ = true;
        }

        private void Bet_20()
        {
            casino_.TakeBet("20");
            Player_Add_Bet += 20;
            betplaced_ = true;
        }

        private void Bet_50()
        {
            casino_.TakeBet("50");
            Player_Add_Bet += 50;
            betplaced_ = true;
        }

        private void Bet_100()
        {
            casino_.TakeBet("100");
            Player_Add_Bet += 100;
            betplaced_ = true;
        }
     
        private void InitializeSkins()
        {
      
                DealerCardImages.Add(new BitmapImage(new Uri("pack://application:,,,/Pictures/cardskin.png")));
            
        }

        /*private bool CanClick()
            => FirstName == "Kevin";*/


        /*        public event PropertyChangedEventHandler? PropertyChanged; //Everytime some of the propertie changes it says okey I am gonna update the context

                [RelayCommand]
                private void Click()
                {
                    FirstName = "Robert";
                }*/
    }
}