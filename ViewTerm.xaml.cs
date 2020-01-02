using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TermManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewTerm : ContentPage
    {
        Term term;
        public ViewTerm(Term term)
        {
            InitializeComponent();
            this.term = term;
            UpdateFields();

            saveButton.Clicked += (sender, arguments) => SaveTerm();
            deleteButton.Clicked += (sender, arguments) => DeleteTerm();
            cancelButton.Clicked += (sender, arguments) => GoBack();
            viewCoursesButton.Clicked += (sender, arguments) => ViewCourses();
        }

        public void UpdateFields() {
            termName.Text = term.Name;
            startDatePicker.Date = term.StartDate;
            endDatePicker.Date = term.EndDate;
            int numCourses = 0;
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
            {
                connection.CreateTable<Course>();
                List<Course> courses = connection.Table<Course>().ToList();
                for (var i = 0; i < courses.Count(); i++) {
                    if (courses[i].TermId == term.Id) {
                        numCourses++;
                    }
                }
            }
            if (numCourses <= 0) {
                viewCoursesButton.Text = "ADD COURSES";
                courseNarrative.Text = "Press the ADD COURSES button to add, delete, edit, or view course information.";
            }
        }

        public void SaveTerm() {
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
                        var successfulUpdate = connection.Update(term);
                        if (successfulUpdate > 0)
                        {
                            DisplayAlert("SUCCESS", "The Term has been updated successfully.", "OK");
                        }
                    }
                }
                catch (Exception e)
                {
                    DisplayAlert("ERROR", "The Term was not saved to the database. " + e.Message, "OK");
                }
                Navigation.PopAsync();
            }
            else {
                DisplayAlert("WARNING", "The Term's end date MUST be later than the start date.", "OK");
            }

        }

        public async void DeleteTerm() {
            var yes = await DisplayAlert("DELETE", "Are you sure you want to delete this term and all of its courses?", "Yes", "No");
            if (yes)
            {
                using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
                {
                    connection.CreateTable<Term>();
                    connection.Delete(term);
                }
                //delete each course associated with that Term.
                using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
                {
                    List<Course> courses = new List<Course>();
                    connection.CreateTable<Course>();
                    courses = connection.Table<Course>().ToList();
                    for (var i = 0; i < courses.Count; i++) {
                        if (courses[i].TermId == term.Id) {
                            connection.Delete(courses[i]);
                        }
                    }
                }
                await Navigation.PopAsync();
            }
        }

        public void GoBack() {
            Navigation.PopAsync();
        }

        public async void ViewCourses() {
            ViewCourses viewCourses = new ViewCourses(term);
            await Navigation.PushAsync(viewCourses);
        }
    }
}