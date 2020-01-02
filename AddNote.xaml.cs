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
    public partial class AddNote : ContentPage
    {
        Course course;
        public AddNote(Course course)
        {
            InitializeComponent();
            this.course = course;
            saveButton.Clicked += (sender, arguments) => SaveNote();
            cancelButton.Clicked += (sender, arguments) => GoBack();
        }

        public void GoBack()
        {
            Navigation.PopAsync();
        }

        public void SaveNote()
        {
            Note note = new Note {
                CourseId = course.Id,
                Content = noteEntry.Text
            };
            try
            {
                using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
                {
                    connection.CreateTable<Note>();
                    var successfulUpdate = connection.Insert(note);
                    if (successfulUpdate > 0)
                    {
                        DisplayAlert("SUCCESS", "The Note has been added successfully.", "OK");
                    }
                }
            }
            catch (Exception e)
            {
                DisplayAlert("ERROR", "The Note was not added to the database. " + e.Message, "OK");
            }
            Navigation.PopAsync();
        }

    }
}