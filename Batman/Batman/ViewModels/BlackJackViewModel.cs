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
using System.Net.Sockets;
using System.Text;
namespace Batman
{
    public partial class BlackJackViewModel : ObservableObject
    {

        private bool betplaced_ = false;
        private bool check_if_hit_was_pressed = false;
        private bool check_if_double_was_pressed = false;
        private bool Bet_pressed = false;
        private bool fold_pressed = false;
        private int dealers_hand_value;

        private int player_hand_value;

        private int player_add_bet;

        private string _statusMessage;
        private int player_balance;

        public IRelayCommand? DoubleCommand { get; private set; } // IRelayCommand and RelayCommand is used to check if we can execute command like buttons and others in MVVM
        public IRelayCommand? HitCommand { get; private set; }
        public IRelayCommand? StandCommand { get; private set; }

        public IRelayCommand? FoldCommand { get; private set; }

        public IRelayCommand? Start_the_game { get; private set; }

        public IRelayCommand? Bet_10_Euro { get; private set; }
        public IRelayCommand? Bet_20_Euro { get; private set; }

        public IRelayCommand? Bet_50_Euro { get; private set; }

        public IRelayCommand? Bet_100_Euro { get; private set; }

        public IRelayCommand? BetCommand { get; private set; }

        private  IDeck deck_ { get; set; }
        private IPlayer player_ { get; set; }
        private  IDealer dealer_ { get; set; }
        private Casino casino_ { get; set; }


        public ObservableCollection<BitmapImage> PlayerCardImages { get; set; } // this provides some kind of notification when item is added removed
        public ObservableCollection<BitmapImage> DealerCardImages { get; set; }

        public int Player_Balance
        {
            get => player_balance;
            set
            {
                SetProperty(ref player_balance, value); // ref is use to pass the parameter by reference it means that changes are made to the parameteer will effect the original variable
            }                                      
        }
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

        public int PlayerHandValue
        {
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
         //   player_.ChipsChanged += OnChipsChanged;
            Player_Balance = player_.Chips_;
            PlayerCardImages = new ObservableCollection<BitmapImage>(); 
            DealerCardImages = new ObservableCollection<BitmapImage>();
            _statusMessage = "";
            InitializeCommands();
            InitializeSkins();
        }


        public void ResetGame()
        {
            deck_ = new Deck();
            player_ = new Player();
            dealer_ = new Dealer();
            casino_ = new Casino(deck_, player_, dealer_);
            casino_.RoundEnded += Casino_RoundEnded;
            player_.ChipsChanged += OnChipsChanged;
            Player_Balance = player_.Chips_;
            PlayerCardImages.Clear();
            DealerCardImages.Clear();
            _statusMessage = "";
            Player_Add_Bet = 0;
            PlayerHandValue = 0;
            DealersHandValue = 0;
            player_.Wins_ = 0;
            ResetFlags();
        }

       private void OnChipsChanged(object? sender, int newChipsCount)
        {
            Player_Balance = newChipsCount;
        }

        private void Casino_RoundEnded(object? sender, string e)
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
                if (dealer_.RevealedCards.Count() == Constants.VALUE_OF_1)
                {
                    DealerCardImages.Add(GetCardImage(card));

                    InitializeSkins();
                }
                else
                {
                    DealerCardImages.Add(GetCardImage(card));

                }
            }
            DealersHandValue = dealer_.GetHandValue();
        }



        private void On_Stand()
        {
            dealer_.RevealCard(dealer_.RevealedCards);
        }


        private void ResetFlags()
        {
            check_if_hit_was_pressed = false;
            check_if_double_was_pressed = false;
            betplaced_ = false;
            Bet_pressed = false;
            Player_Add_Bet = 0;
            player_.Wins_ = 0;
        }


        void Check_If_Player_Bust_When_He_Gets_Card()
        {
            if (player_.GetHandValue() > Constants.VALUE_OF_21)
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
                if (!Bet_pressed)
                {
                    throw new InvalidOperationException("Place a bet first!");
                }
                if (check_if_hit_was_pressed)
                {
                    throw new InvalidOperationException("You can't Double when you Hitted!");
                }
                if (player_.GetHandValue() >= Constants.VALUE_OF_21)
                {
                    throw new InvalidOperationException("You are exceeding Limits");
                }
                casino_.TakeActions("DOUBLE");
                Check_If_Player_Bust_When_He_Gets_Card();
                UpdateCardDisplayForPlayer();
                check_if_double_was_pressed = true;

            }
            catch (Exception ex)
            {
                check_if_double_was_pressed = false;
                StatusMessage = $"{ex.Message}";
            }
        }


        private void Click_On_Hit()
        {
            try
            {
                if (!Bet_pressed)
                {
                    throw new InvalidOperationException("You have to Press BET");
                }
                if (check_if_double_was_pressed)
                {
                    throw new InvalidOperationException("You can't Hit after Double!");
                }
                if (player_.GetHandValue() < Constants.VALUE_OF_21)
                {
                    casino_.TakeActions("HIT");
                    Check_If_Player_Bust_When_He_Gets_Card();
                    UpdateCardDisplayForPlayer();
                    check_if_hit_was_pressed = true;
                }
                else
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

                Player_Add_Bet = Constants.VALUE_OF_O;

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
            if (Player_Balance == 0 && player_.Wins_ == 0)
            {
                fold_pressed = true;
               ResetGame();
            } else if (!fold_pressed){
                casino_.TakeActions("FOLD");

                UpdateCardDisplayForPlayer();
                UpdateCardDisplayForDealer();
                dealer_.RevealedCards.Clear();
                ResetFlags();
            }
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
                    betplaced_ = false;
                    Bet_pressed = true;
                    fold_pressed = false;
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
            PlaceBet(Constants.VALUE_OF_10);
        }

        private void Bet_20()
        {
            PlaceBet(Constants.VALUE_OF_20);
        }

        private void Bet_50()
        {
            PlaceBet(Constants.VALUE_OF_50);
        }

        private void Bet_100()
        {
            PlaceBet(Constants.VALUE_OF_100);
        }

        private void PlaceBet(int amount)
        {
            try
            {
                if (Bet_pressed)
                {
                    throw new InvalidOperationException("You are already playing.");
                }

                if (player_.Chips_ >= amount)
                {
                    casino_.TakeBet(amount.ToString());
                    Player_Add_Bet += amount;
                    Player_Balance = player_.Chips_;
                    betplaced_ = true;
                }
                else
                {
                    throw new InvalidOperationException("You don't have enough chips!");
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"{ex.Message}";
            }
        }

        private void InitializeSkins()
        {
            DealerCardImages.Add(new BitmapImage(new Uri("pack://application:,,,/Pictures/cardskin.png")));

        }
    }
}