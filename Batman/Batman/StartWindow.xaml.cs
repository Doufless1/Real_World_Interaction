using System;
using System.Windows;
using System.Windows.Controls;
using Batman.ViewModels;
namespace Batman
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();

        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void GameCommandButton_Click(object sender, RoutedEventArgs e)
        {
            ShowGameCommandWindow("Blackjack, also known as 21, is a popular casino card game where the goal is to have a hand value as close to 21 as possible without exceeding it. The game is usually played with one or more decks of 52 cards. Here’s a detailed guide on how to play Blackjack, including descriptions of various commands and their interactions:" +
                "\r\n\r\nThe primary goal in Blackjack is to beat the dealer's hand without going over 21. Number cards (2-10) are worth their face value, face cards (Jack, Queen, King) are each worth 10 points, and Aces can be worth either 1 or 11 points, depending on which value is more beneficial for the hand. Each player is dealt two cards, and the dealer is also dealt two cards (one face up and one face down)." +
                "\r\n\r\nTo begin, place your wager at the beginning of each round. This is the amount of money you are willing to risk on that particular hand. This action signifies your participation in the round and sets the stage for the ensuing play. No other commands can be executed until a bet is placed." +
                "\r\n\r\nThe first command is \"Hit.\" When you request another card from the dealer to add to your hand, it is done in hopes of increasing your hand value closer to 21. You can keep hitting until you decide to stand or until your hand value exceeds 21 (bust). If you bust, you lose your bet immediately, regardless of the dealer’s hand." +
                "\r\n\r\nNext, there is \"Stand,\" where you keep your current hand and end your turn. When you choose to stand, your hand value is final. The dealer then plays their hand according to the rules (usually hitting until they reach a hand value of at least 17). The game compares your hand against the dealer’s to determine the winner." +
                "\r\n\r\n\"Fold\" is another command, where you forfeit your hand and lose your bet. This command is less common in standard Blackjack rules and is more associated with other card games. In some variations, folding might be allowed to minimize losses in specific scenarios." +
                "\r\n\r\n\"Double Down\" is when you double your initial bet in exchange for committing to stand after receiving one more card. This command is typically used when the player has a strong initial hand (such as a total of 10 or 11). After doubling down, you receive exactly one additional card, and your turn ends. The risk is higher, but so is the potential reward." +
                "\r\n\r\n\"Split\" is when your first two cards are of the same value, and you can split them into two separate hands. Each hand then gets an additional card, and you play each hand independently. Splitting requires you to place an additional bet equal to your original bet. This command increases your chances of winning but also doubles your risk. Each hand can win, lose, or push (tie) independently." +
                "\r\n\r\nThe dealer’s face-down card, known as the “hole card,” is revealed after all players have finished their actions. The dealer must hit until their hand value is at least 17. In some casinos, a dealer must hit on a “soft” 17 (a hand containing an Ace valued as 11). If the dealer busts, all remaining players win. If the dealer does not bust, their hand is compared to the players' hands to determine the outcomes." +
                "\r\n\r\nA “Natural Blackjack” occurs if your initial two cards total 21 (Ace and a 10-value card), and you usually receive a payout of 3:2, provided the dealer doesn’t also have a Blackjack. A “Regular Win” happens if your hand value is closer to 21 than the dealer’s without exceeding 21, and you typically receive a payout of 1:1. A “Push” is when your hand value ties with the dealer’s hand, and your bet is returned. “Insurance” is an option if the dealer’s face-up card is an Ace. You can take “insurance” against the dealer having Blackjack, and this side bet pays 2:1 if the dealer’s hole card is a 10-value card. However, taking insurance is generally considered a bad bet." +
                "\r\n\r\nTo play Blackjack successfully, it’s crucial to learn the basic strategy, which provides guidance on when to hit, stand, double down, or split based on your hand value and the dealer’s face-up card. Some players also employ card counting, a technique to keep track of the cards that have been dealt to adjust betting and playing strategies. While card counting is not illegal, it is frowned upon in casinos." +
                "\r\n\r\nIn summary, Blackjack is a strategic game where understanding the value of your hand, the dealer’s potential hand, and the proper use of commands can significantly influence your success. Each command plays a critical role in managing risk and optimizing your chances of winning against the dealer.");
        }
        private void BetButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowGameCommandWindow("Bet: Place Your Bet. First the number you want is dependable on your. Further after u press the bet you can DOUBLE if you want HIT STAND or FOLD. Before hand you cant do that." +
                "You will encounter a error on what should you do based on this error u will accomplish your action u want. ");
        }

        private void StandButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowGameCommandWindow("Stand: Keep your current hand its gonna compare it to the dealer and you are gonna see if u won or lost.");
        }

        private void FoldButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowGameCommandWindow("Fold: The fold button if u have bad hands at the begging u can forfeit and take half of your money but if you do it after the dealer or u won. Its gonna claer your bet and hand's.");
        }

        private void DoubleButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowGameCommandWindow("Double: Double your bet and receive one more card but you cant hit again u only can stand or fold.");
        }

        private void HitButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowGameCommandWindow("Hit: Take another card to improve your hand.");
        }

        private void ShowGameCommandWindow(string description)
        {
            GameCommand gameCommandWindow = new GameCommand(description);
            gameCommandWindow.Show();
        }
    }
}
