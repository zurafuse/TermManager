using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TermManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewNote : ContentPage
    {
        Note note;
        Course course;
        public ViewNote(Note note, Course course)
        {
            InitializeComponent();
            this.note = note;
            this.course = course;
            noteEntry.Text = note.Content;
            saveButton.Clicked += (sender, arguments) => SaveNote();
            cancelButton.Clicked += (sender, arguments) => GoBack();
            deleteButton.Clicked += (sender, arguments) => DeleteNote();
            emailButton.Clicked += (sender, arguments) => EmailNote();
        }

        public void GoBack()
        {
            Navigation.PopAsync();
        }

        public async void EmailNote() {
            try
            {
                List<string> recipients = new List<string>();
                recipients.Add(emailEntry.Text);
                var email = new EmailMessage
                {
                    Subject = "NOTE FOR " + course.Name,
                    Body = noteEntry.Text,
                    To = recipients,
                };
                await Email.ComposeAsync(email);
            }
            catch (Exception error)
            {
                await DisplayAlert("ERROR", error.Message, "OK");
            }
        }

        public async void DeleteNote()
        {
            var yes = await DisplayAlert("DELETE", "Are you sure you want to delete this note?", "Yes", "No");
            if (yes)
            {
                using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
                {
                    connection.CreateTable<Note>();
                    connection.Delete(note);
                }
                await Navigation.PopAsync();
            }
        }

        public void SaveNote()
        {
            note.Content = noteEntry.Text;
            try
            {
                using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
                {
                    connection.CreateTable<Note>();
                    var successfulUpdate = connection.Update(note);
                    if (successfulUpdate > 0)
                    {
                        DisplayAlert("SUCCESS", "The Note has been updated successfully.", "OK");
                    }
                }
            }
            catch (Exception e)
            {
                DisplayAlert("ERROR", "The changes were not added to the database. " + e.Message, "OK");
            }
            Navigation.PopAsync();
        }
    }
}