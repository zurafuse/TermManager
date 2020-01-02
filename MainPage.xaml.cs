using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows;
using SQLite;
using Plugin.LocalNotifications;

namespace TermManager
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        Term term = new Term();
        List<Term> TermList = new List<Term>();
        public MainPage()
        {
            InitializeComponent();
            DataSet.CreateSampleData();
            PopulateTerms();
            DataSet.ShowNotifications();
            addTermButton.Clicked += (sender, arguments) => AddTerm();
        }

        //Show list of terms
        public void PopulateTerms() {           
            listOfTerms.Children.Clear();
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
            {
                connection.CreateTable<Term>();
                TermList = connection.Table<Term>().ToList();
            }
            for (var i = 0; i < TermList.Count; i++)
            {
                //Create the label
                Label TermText = new Label
                {
                    Text = TermList[i].Name + Environment.NewLine + TermList[i].StartDate.ToString("MMMM dd, yyyy") + " to " + TermList[i].EndDate.ToString("MMMM dd, yyyy"),
                    FontSize = 20,
                    TextColor = Color.Black,
                    HeightRequest = 80,
                    VerticalTextAlignment = TextAlignment.Center,
                    StyleId = i.ToString()
                };
                //Click event
                TapGestureRecognizer TermText_Touch = new TapGestureRecognizer();
                TermText_Touch.Tapped += (s, e) =>
                {
                    term = TermList[int.Parse(TermText.StyleId)];
                    TermText.BackgroundColor = Color.LightSkyBlue;
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        TermText.BackgroundColor = Color.White;
                        return false; // return true to repeat counting, false to stop timer
                    });
                    ViewTerm();
                };
                
                TermText.GestureRecognizers.Add(TermText_Touch);

                //Add to the list of terms
                listOfTerms.Children.Add(new BoxView { Color = Color.Black, HeightRequest = 1, WidthRequest = 100});
                listOfTerms.Children.Add(TermText);
            }
        }

        public async void AddTerm() {
            //Create Navigation to the Add Term page.
            AddTerm addTerm = new AddTerm();
            addTerm.Disappearing += (sender, arguments) =>
            {
                PopulateTerms();
            };
            await Navigation.PushAsync(addTerm);
        }

        public async void ViewTerm() {
            //Action/Method goes here.
            //Create Navigation to the View Term page.
            ViewTerm viewTerm = new ViewTerm(term);
            viewTerm.Disappearing += (sender, arguments) =>
            {
                PopulateTerms();
            };
            await Navigation.PushAsync(viewTerm);           
        }

    }
}
