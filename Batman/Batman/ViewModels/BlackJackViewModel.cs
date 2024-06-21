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

        public IRelayCommand HitCommand { get; private set; }
        public IRelayCommand StandCommand { get; private set; }

        public IRelayCommand FoldCommand { get; private set; }

        public IRelayCommand Start_the_game { get; private set; }   

        private readonly IDeck deck_;
        private readonly IPlayer player_;
        private readonly IDealer dealer_;
        private readonly Casino casino_;


        public ObservableCollection<BitmapImage> PlayerCardImages { get; set; }
        public ObservableCollection<BitmapImage> DealerCardImages { get; set; }

        //     public ObservableCollection<ImageSource> CardImages2 { get; set; }

        public ImageSource TestImage { get; set; }
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        public BlackJackViewModel()
        {
            deck_ = new Deck();
            player_ = new Player();
            dealer_ = new Dealer();
            casino_ = new Casino(deck_, player_, dealer_);
            PlayerCardImages = new ObservableCollection<BitmapImage>();
            DealerCardImages = new ObservableCollection<BitmapImage>();
            //   CardImages2 = new ObservableCollection<ImageSource>();
            StartRound();
            InitializeCommands();
            InitializeSkins();

            //      string imagePath = System.IO.Path.Combine(baseDir, "Pictures", $"{Suit}");
            //    TestImage = new BitmapImage(new Uri(imagePath,UriKind.Absolute));
        }

        private void StartRound()
        {

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

            //if (!File.Exists(packUri))
            //{
            //    Debug.WriteLine("$File not found:{imagePath}");
            //    return null;
            //}
            Debug.WriteLine($"Loading image from: {packUri}");
            return new BitmapImage(new Uri(packUri)); // UriKind.Relative show its only relative to the applicatins current directory not like as universtal path
        }
        private void InitializeSkins()
        {
            //   var image = new BitmapImage(new Uri("Pictures/cardskin.png", UriKind.Relative));
            //    PlayerCardImages.Add(image);
            //  CardImages2.Add(image);

            //  PlayerCardImages.Add(new BitmapImage(new Uri("pack://application:,,,/Pictures/cardskin.png", UriKind.Absolute)));
            DealerCardImages.Add(new BitmapImage(new Uri("pack://application:,,,/Pictures/cardskin.png", UriKind.Absolute)));
        }
        private void UpdateCardDisplay()
        {
            UpdateHandDisplay(player_.Hand_, PlayerCardImages);
            UpdateHandDisplay(dealer_.HiddenCards, DealerCardImages);
        }

        private void UpdateHandDisplay(List<Card> hand, ObservableCollection<BitmapImage> playerCardImages)
        {
            playerCardImages.Clear();
            foreach (var card in hand)
            {
                playerCardImages.Add(GetCardImage(card));
            }
        }
        private void CanClick()

        => casino_.Is_Hand_for_Splitting(player_.Hand_);


        private void Click_On_Hit()
        {
            casino_.TakeActions("HIT");
            UpdateCardDisplay();
        }

        private void Click_On_Stand()
        {
            casino_.TakeActions("STAND");
            UpdateCardDisplay();
        }

        private void Click_On_Fold()
        {
            casino_.TakeActions("FOLD");
            UpdateCardDisplay();
        }
        private void InitializeCommands()
        {
            HitCommand = new RelayCommand(Click_On_Hit);
            StandCommand = new RelayCommand(Click_On_Stand);
            FoldCommand = new RelayCommand(Click_On_Fold);
            Start_the_game = new RelayCommand(Start_the_Round_After_betting);
        }

        private void Bet_10()
        {
            casino_.TakeBet("10");
        }

        private void Bet_20()
        {
            casino_.TakeBet("20");
        }

        private void Bet_50()
        {
            casino_.TakeBet("50");
        }

        private void Bet_100()
        {
            casino_.TakeBet("100");
        }
        private void Start_the_Round_After_betting()
        {
            casino_.InitializeHand("bet");
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