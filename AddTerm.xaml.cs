using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TermManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTerm : ContentPage
    {
        public AddTerm()
        {
            InitializeComponent();

            cancelButton.Clicked += (sender, arguments) => GoBack();
            saveButton.Clicked += (sender, arguments) => SaveTerm();
        }

        public async void AddCourses(Term term) {
            //Add ViewCourses page, passing the new term as the parameter.
            ViewCourses viewCourses = new ViewCourses(term);
            await Navigation.PushAsync(viewCourses);
        }


        public void SaveTerm()
        {
            Term term = new Term();
            term.Name = termName.Text;
            term.StartDate = startDatePicker.Date;
            term.EndDate = endDatePicker.Date;
            if (term.StartDate < term.EndDate)
            {
                try
                {
                    using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
                    {
                        connection.CreateTable<Term>();
                        var successfulUpdate = connection.Insert(term);
                        if (successfulUpdate > 0)
                        {
                            DisplayAlert("SUCCESS", "The Term has been added successfully. You will now need to add six courses to the term.", "OK");
                            Navigation.PopAsync();
                            AddCourses(term);
                        }
                        else {
                            DisplayAlert("WARNING", "The Term was not added to the database.", "OK");
                        }
                    }
                }
                catch (Exception e)
                {
                    DisplayAlert("ERROR", "The Term was not added to the database. " + e.Message, "OK");
                    Navigation.PopAsync();
                }
                //pop async
            }
            else
            {
                DisplayAlert("WARNING", "The Term's end date MUST be later than the start date.", "OK");
            }

        }

        public void GoBack()
        {
            Navigation.PopAsync();
        }
    }
}