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
    public partial class ViewCourses : ContentPage
    {
        Term term;
        Course course = new Course();
        int coursesForThisTerm = 0;
        public ViewCourses(Term term)
        {
            InitializeComponent();
            this.term = term;
            PopulateCourses();
            courseHeader.Text = "COURSES FOR " + term.Name;
            cancelButton.Clicked += (sender, arguments) => GoBack();
            addCourseButton.Clicked += (sender, arguments) => AddCourse();
        }

        public void GoBack()
        {
            //ensure that six courses are added.
            if (coursesForThisTerm != 6) {
                DisplayAlert("WARNING", "You MUST have six courses for a term. Please add " + (6 - coursesForThisTerm) + " more courses before exiting.", "OK");
            }
            else
            {
                Navigation.PopAsync();
            }
        }

        public async void AddCourse() {
            //Ensure that there are not already 6 courses for the term.
            if (coursesForThisTerm < 6)
            {
                AddCourse addCourse = new AddCourse(term);
                addCourse.Disappearing += (sender, arguments) =>
                {
                    PopulateCourses();
                };
                await Navigation.PushAsync(addCourse);
            }
            else {
                await DisplayAlert("WARNING", "You already have six courses. Please remove a course before adding another one.", "OK");
            }
        }

        public async void ViewCourse() {
            //Open new ViewCourse.cs
            ViewCourse viewCourse = new ViewCourse(course);
            viewCourse.Disappearing += (sender, arguments) =>
            {
                PopulateCourses();
            };
            await Navigation.PushAsync(viewCourse);
        }

        public void PopulateCourses()
        {
            coursesForThisTerm = 0;
            listOfCourses.Children.Clear();
            List<Course> courses = new List<Course>();
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
            {
                connection.CreateTable<Course>();
                courses = connection.Table<Course>().ToList();
            }
            for (var i = 0; i < courses.Count; i++)
            {
                if (term.Id == courses[i].TermId) {
                    coursesForThisTerm++;
                    //Create the label
                    Label CourseText = new Label
                    {
                        Text = courses[i].Name + Environment.NewLine + courses[i].StartDate.ToString("MMMM dd, yyyy") + " to " + courses[i].EndDate.ToString("MMMM dd, yyyy"),
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
                        course = courses[int.Parse(CourseText.StyleId)];
                        CourseText.BackgroundColor = Color.LightSkyBlue;
                        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                        {
                            CourseText.BackgroundColor = Color.White;
                            return false; // return true to repeat counting, false to stop timer
                        });
                        ViewCourse();
                    };

                    CourseText.GestureRecognizers.Add(TermText_Touch);

                    //Add to the list of terms
                    listOfCourses.Children.Add(new BoxView { Color = Color.Black, HeightRequest = 1, WidthRequest = 100 });
                    listOfCourses.Children.Add(CourseText);
                }
            }
        }

    }
}