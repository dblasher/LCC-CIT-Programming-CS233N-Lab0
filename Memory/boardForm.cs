using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public partial class boardForm : Form
    {
        public boardForm()
        {
            InitializeComponent();
        }

        #region Instance Variables
        const int NOT_PICKED_YET = -1;

        int firstCardNumber = NOT_PICKED_YET;
        int secondCardNumber = NOT_PICKED_YET;
        int matches = 0;
        #endregion

        #region Methods

        // This method finds a picture box on the board based on it's number (1 - 20)
        // It takes an integer as it's parameter and returns the picture box controls
        // that's name contains that number
        private PictureBox GetCard(int i)
        {
            PictureBox card = (PictureBox)this.Controls["card" + i];
            return card;
        }

        // This method gets the filename for the image displayed in a picture box given it's number
        // It takes an integer as it's parameter and returns a string containing the 
        // filename for the image in the corresponding picture box
        private string GetCardFilename(int i)
        {
            return GetCard(i).Tag.ToString();
        }

        // This method changes the filename for a given picture box
        // It takes an integer and a string that represents a filename as it's parameters
        // It doesn't return a value but stores the filename for the image to be displayed
        // in the picture box.  It doesn't actually display the new image
        private void SetCardFilename(int i, string filename)
        {
            GetCard(i).Tag = filename;
        }

        // These 2 methods get the value (and suit) of the card in a given picturebox
        // Both methods take an integer as the parameter and return a string
        private string GetCardValue(int index)
        {
            return GetCardFilename(index).Substring(4, 1);
        }

        private string GetCardSuit(int index)
        {
            return GetCardFilename(index).Substring(5, 1);
        }

        // TODO:  students should write this one
        //this needs to check that the values of index1 and index2 are the same. "a" == "a" means they're both aces.
        private bool IsMatch(int index1, int index2)
        {
            if (GetCardValue(index1)==GetCardValue(index2)) {
                //why double equal and not triple? I forget the difference
                return true;
            }
            else { return false; }
        }

        // This method fills each picture box with a filename
        //this should also have a ceiling for i, otherwise we could end up trying to run SetCardFilename()
        //on a picture box that doesn't exist. Or use try{} catch{}
        private void FillCardFilenames()
        {
            string[] values = { "a", "2", "j", "q", "k" };
            string[] suits = { "c", "d", "h", "s" };
            int i = 1;

            for (int suit = 0; suit <= 3; suit++)
            {
                for (int value = 0; value <= 4; value++)
                {
                    SetCardFilename(i, "card" + values[value] + suits[suit] + ".jpg");
                    i++;
                }
            }
        }

        // TODO:  students should write this one
        private void ShuffleCards()
        {
            Random rnd = new Random();
          
            for (int cardNum=1;cardNum<=20; cardNum++)
            {
                int randomNum = rnd.Next(1, 21);
                string temp = GetCardFilename(cardNum);
                string tempRandom = GetCardFilename(randomNum);
                SetCardFilename(cardNum, tempRandom);
                SetCardFilename(randomNum, temp); 
            }

            //we'll write this together in class 1/16
            /*my thinking:
             * create array, deck, holding all possible card file names, should have 20 entries like ["cardac.jpg","cardak.jpg",...]
             * create random number object
             * create new queue, either local or global, called shuffledDeck
             * pop an entry from the array at random.Next(0,deck.count()) and add it to shuffleDeck
             * loop through this process until deck.count()==0
             * shuffleDeck should have a queue of 20 strings in random order
             * set each card.Tag=an item in the queue
             * 
             * OR, 
             */
        }

        // This method loads (shows) an image in a picture box.  Assumes that filenames
        // have been filled in an earlier call to FillCardFilenames
        private void LoadCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\Cards\\" + GetCardFilename(i));
        }

        // This method loads the image for the back of a card in a picture box
        private void LoadCardBack(int i)
        {
            PictureBox card = GetCard(i);
            card.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\Cards\\black_back.jpg");
        }

        // TODO:  students should write all of these
        // shows (loads) the backs of all of the cards
        private void LoadAllCardBacks()
        {
            for (int i =1; i <= 20; i++)
            {          
                //LoadCard(i);
                //disable comment below when we know ShuffleCards() and LoadAllCardbacks() are working correctly
                LoadCardBack(i);
            }
            //loop calling LoadCardBack
        }

        // Hides a picture box
        private void HideCard(int i)
        {
            LoadCardBack(i);
        }

        private void HideAllCards()
        {
            for (int card = 1; card <= 20; card++)
            {
                LoadCardBack(card);
            }
        }

        // shows a picture box
        private void ShowCard(int i)
        {
            LoadCard(i);
        }

        private void ShowAllCards()
        {
            for (int card = 1; card <= 20; card++)
            {
                LoadCard(card);
            }
        }

        // disables a picture box
        private void DisableCard(int i)
        {
            GetCard(i).Click-= new System.EventHandler(this.card_Click);
            /*
             * private void DisableSquare(Label square)       
            *square.Click -= new System.EventHandler(this.label_Click);     
             */
        }

        private void DisableAllCards()
        {
            for (int card = 1; card <= 20; card++)
            {
                DisableCard(card);
            }
        }

        private void EnableCard(int i)
        {
            GetCard(i).Click += new System.EventHandler(this.card_Click);
        }

        private void EnableAllCards()
        {
            for (int card = 1; card <= 20; card++)
            {
                EnableCard(card);
            }
        }
        
        private void EnableAllVisibleCards()
        {
            //if the card's Image property isn't equal to its Tag then enable it. The Tag holds the front of the card's filename
            for(int card = 1; card <= 20; card++)
            {
                if (GetCard(card).Image != GetCard(card).Tag)
                {
                    EnableCard(card);
                }              
            }
        }

        private void NewGame()
        {
            ShuffleCards();
            LoadAllCardBacks();
            EnableAllCards();
            matches = 0;
            resultLabel.Hide();
            resultLabel.Text = "Match";
        }
        #endregion

        #region EventHandlers
        private void boardForm_Load(object sender, EventArgs e)
        {
            resultLabel.Hide();
            // assign each card a file name, card1.Tag="cardac.jpg" for example
            FillCardFilenames();

            //randomly swaps all card tags, card1.Tag=card17.Tag, card2.Tag=card12.Tag, etc.
            ShuffleCards();
            
            //sets all card image properties to black_back.jpg, we can still get the front of the card from the Tag property
            LoadAllCardBacks();
            
          
        }

        private void card_Click(object sender, EventArgs e)
        {
            PictureBox card = (PictureBox)sender;
            int cardNumber = int.Parse(card.Name.Substring(4));
            resultLabel.Hide();
            if (firstCardNumber == NOT_PICKED_YET)
            {
                firstCardNumber = cardNumber;
                LoadCard(firstCardNumber);
                DisableCard(firstCardNumber);
            }
            else
            {
                secondCardNumber = cardNumber;
                LoadCard(secondCardNumber);
                DisableAllCards();
                flipTimer.Start();
            }

            /* 
             * if the first card isn't picked yet
             *      save the first card index
             *      load the card
             *      disable the card
             *  else (the user just picked the second card)
             *      save the second card index
             *      load the card
             *      disable all of the cards
             *      start the flip timer
             *  end if
            */
        }

        private void flipTimer_Tick(object sender, EventArgs e)
        {
            flipTimer.Stop();

            //if we have a match
            if (IsMatch(firstCardNumber, secondCardNumber))
            {
                resultLabel.Show();
                matches += 1;
                firstCardNumber = NOT_PICKED_YET;
                secondCardNumber = NOT_PICKED_YET;
                if (matches == 10)
                {
                    resultLabel.Text = "You Win!";
                }
                else
                {
                    EnableAllVisibleCards();
                }
            }

            //if they dont match
            else
            {
                HideCard(firstCardNumber);
                HideCard(secondCardNumber);
                firstCardNumber = NOT_PICKED_YET;
                secondCardNumber = NOT_PICKED_YET;
                EnableAllVisibleCards();
            }
            /*
             * stop the flip timer
             * -if the first card and second card are a match
             *      -increment the number of matches
             *      ? hide the first card
             *      ? hide the second card
             *      -reset the first card number
             *      -reset the second card number
             *      if the number of matches is 10
             *          show a message box
             *      else
             *          enable all of the cards left on the board
             *      end if
             * else
             *      flip the first card back over
             *      flip the second card back over
             *      reset the first card number
             *      reset the second card number
             *      enable all of the cards left on the board
             * end if
             */
        }

        #endregion

        private void newGameButton_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
/*
 * NewGame(){
  1 shuffle deck
 assign pictureBox0 a random card back (do face first to check if working), repeat for all picture boxes
 
    after writing form load test show all cards

    then switch the shuffle deck to show card back on form load
   2  work on making the card clickable, causing show card to run and storing that card number to first_card_number instance variable
    disable the first card
   3 start the Flip timer event handler

    next card clicked showed be saved to second_card_number

     //string[] deck = {"cardac.jpg","cardad.jpg", "cardah.jpg", "cardas.jpg", "card2c.jpg", "card2d.jpg", "card2h.jpg", "card2s.jpg", "cardjc.jpg", "cardjd.jpg",
            // "cardjh.jpg","cardjs.jpg","cardqc.jpg","cardqd.jpg","cardqh.jpg","cardqs.jpg","cardkc.jpg","cardkd.jpg","cardkh.jpg","cardks.jpg"};
 */
