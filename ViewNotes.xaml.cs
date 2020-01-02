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
    public partial class ViewNotes : ContentPage
    {
        Course course;
        Note note = new Note();
        List<Note> notes = new List<Note>();
        public ViewNotes(Course course)
        {
            InitializeComponent();
            this.course = course;
            PopulateNotes();
            addNoteButton.Clicked += (sender, arguments) => AddNote();
            goBackButton.Clicked += (sender, arguments) => GoBack();
        }

        public void GoBack()
        {
            Navigation.PopAsync();
        }

        public async void AddNote() {
            AddNote addNote = new AddNote(course);
            addNote.Disappearing += (sender, arguments) =>
            {
                PopulateNotes();
            };
            await Navigation.PushAsync(addNote);
        }

        public async void ViewNote()
        {
            ViewNote viewNote = new ViewNote(note, course);
            viewNote.Disappearing += (sender, arguments) =>
            {
                PopulateNotes();
            };
            await Navigation.PushAsync(viewNote);
        }

        public void PopulateNotes() {
            courseName.Text = "NOTES FOR COURSE " + course.Name;
            listOfNotes.Children.Clear();
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
            {
                connection.CreateTable<Note>();
                notes = connection.Table<Note>().ToList();
            }
            for (var i = 0; i < notes.Count; i++) {
                if (notes[i].CourseId == course.Id) {
                    Label NoteText = new Label { 
                        Text = notes[i].Content,
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
                        note = notes[int.Parse(NoteText.StyleId)];
                        NoteText.BackgroundColor = Color.LightSkyBlue;
                        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                        {
                            NoteText.BackgroundColor = Color.White;
                            return false; // return true to repeat counting, false to stop timer
                        });
                        ViewNote();
                    };

                    NoteText.GestureRecognizers.Add(TermText_Touch);

                    //Add to the list of terms
                    listOfNotes.Children.Add(new BoxView { Color = Color.Black, HeightRequest = 1, WidthRequest = 100 });
                    listOfNotes.Children.Add(NoteText);
                }
            }
        }

    }
}