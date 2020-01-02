using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace TermManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCourse : ContentPage
    {
        Term term;
        Picker picker = new Picker();
        Picker startNotify = new Picker();
        Picker endNotify = new Picker();
        Picker objStartNotify = new Picker();
        Picker objEndNotify = new Picker();
        Picker perfStartNotify = new Picker();
        Picker perfEndNotify = new Picker();
        public AddCourse(Term term)
        {
            InitializeComponent();
            this.term = term;
            AddStatusPicker();
            AddNotificationSetting();
            saveButton.Clicked += (sender, arguments) => SaveCourse();
            cancelButton.Clicked += (sender, arguments) => GoBack();
        }

        public void AddStatusPicker()
        {
            picker.Title = "CHOOSE FROM LIST";
            picker.VerticalOptions = LayoutOptions.CenterAndExpand;
            picker.HorizontalOptions = LayoutOptions.Fill;
            picker.Items.Add("Plan to take");
            picker.Items.Add("In progress");
            picker.Items.Add("Completed");
            picker.Items.Add("Dropped");
            picker.Items.Add("Pending");
            picker.SelectedItem = "CHOOSE FROM LIST";
            courseStatus.Children.Add(picker);
        }

        public void AddNotificationSetting()
        {
            //Notification setting for Course Start Date.
            startNotify.Title = "Would you like to receive a notification as this date approaches?";
            startNotify.VerticalOptions = LayoutOptions.CenterAndExpand;
            startNotify.HorizontalOptions = LayoutOptions.Fill;
            startNotify.WidthRequest = 40;
            startNotify.Items.Add("YES");
            startNotify.Items.Add("NO");
            startNotify.SelectedItem = "YES";
            startDateStack.Children.Add(startNotify);

            //Notification setting for Course End Date.
            endNotify.Title = "Would you like to receive a notification as this date approaches?";
            endNotify.VerticalOptions = LayoutOptions.CenterAndExpand;
            endNotify.HorizontalOptions = LayoutOptions.Fill;
            endNotify.WidthRequest = 40;
            endNotify.Items.Add("YES");
            endNotify.Items.Add("NO");
            endNotify.SelectedItem = "YES";
            endDateStack.Children.Add(endNotify);

            //Notification setting for Objective Assessment Start Date.
            objStartNotify.Title = "Would you like to receive a notification as this date approaches?";
            objStartNotify.VerticalOptions = LayoutOptions.CenterAndExpand;
            objStartNotify.HorizontalOptions = LayoutOptions.Fill;
            objStartNotify.WidthRequest = 40;
            objStartNotify.Items.Add("YES");
            objStartNotify.Items.Add("NO");
            objStartNotify.SelectedItem = "YES";
            objStartStack.Children.Add(objStartNotify);

            //Notification setting for Objective Assessment End Date.
            objEndNotify.Title = "Would you like to receive a notification as this date approaches?";
            objEndNotify.VerticalOptions = LayoutOptions.CenterAndExpand;
            objEndNotify.HorizontalOptions = LayoutOptions.Fill;
            objEndNotify.WidthRequest = 40;
            objEndNotify.Items.Add("YES");
            objEndNotify.Items.Add("NO");
            objEndNotify.SelectedItem = "YES";
            objEndStack.Children.Add(objEndNotify);

            //Notification setting for Performance Assessment Start Date.
            perfStartNotify.Title = "Would you like to receive a notification as this date approaches?";
            perfStartNotify.VerticalOptions = LayoutOptions.CenterAndExpand;
            perfStartNotify.HorizontalOptions = LayoutOptions.Fill;
            perfStartNotify.WidthRequest = 40;
            perfStartNotify.Items.Add("YES");
            perfStartNotify.Items.Add("NO");
            perfStartNotify.SelectedItem = "YES";
            perfStartStack.Children.Add(perfStartNotify);

            //Notification setting for Performance Assessment End Date.
            perfEndNotify.Title = "Would you like to receive a notification as this date approaches?";
            perfEndNotify.VerticalOptions = LayoutOptions.CenterAndExpand;
            perfEndNotify.HorizontalOptions = LayoutOptions.Fill;
            perfEndNotify.WidthRequest = 40;
            perfEndNotify.Items.Add("YES");
            perfEndNotify.Items.Add("NO");
            perfEndNotify.SelectedItem = "YES";
            perfEndStack.Children.Add(perfEndNotify);
        }

        public bool IsGoodEmail(string email)
        {
            try
            {
                MailAddress thisEmailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool IsGoodPhoneNumber(string phone) {
            bool isGood = true;
            for (var i = 0; i < instructorPhone.Text.Count(); i++) {
                if (instructorPhone.Text[i] != '(' && instructorPhone.Text[i] != ')' && instructorPhone.Text[i] != '-'
                    && instructorPhone.Text[i] != '0' && instructorPhone.Text[i] != '1' && instructorPhone.Text[i] != '2'
                    && instructorPhone.Text[i] != '3' && instructorPhone.Text[i] != '4' && instructorPhone.Text[i] != '5'
                    && instructorPhone.Text[i] != '6' && instructorPhone.Text[i] != '7' && instructorPhone.Text[i] != '8'
                    && instructorPhone.Text[i] != '9' && instructorPhone.Text[i] != '#')
                {
                    isGood = false;
                }
            }
            return isGood;
        }

        public void GoBack()
        {
            Navigation.PopAsync();
        }

        public void SaveCourse()
        {
            Course course = new Course();
            course.Name = courseName.Text;
            course.TermId = term.Id;
            course.StartDate = startDatePicker.Date;
            course.EndDate = endDatePicker.Date;
            course.InstructorName = instructorName.Text;
            course.InstructorEmail = instructorEmail.Text;
            course.InstructorPhone = instructorPhone.Text;
            course.ObjectiveName = objName.Text;
            course.ObjectiveStart = objStart.Date;
            course.ObjectiveEnd = objEnd.Date;
            course.PerformanceName = perfName.Text;
            course.PerformanceStart = perfStart.Date;
            course.PerformanceEnd = perfEnd.Date;
            //Get Notification Settings
            course.startNotify = startNotify.SelectedItem.ToString() == "YES" ? true : false;
            course.endNotify = endNotify.SelectedItem.ToString() == "YES" ? true : false;
            course.startObjNotify = objStartNotify.SelectedItem.ToString() == "YES" ? true : false;
            course.endObjNotify = objEndNotify.SelectedItem.ToString() == "YES" ? true : false;
            course.startPerfNotify = perfStartNotify.SelectedItem.ToString() == "YES" ? true : false;
            course.endPerfNotify = perfEndNotify.SelectedItem.ToString() == "YES" ? true : false;

            if (picker.SelectedItem.ToString() == "CHOOSE FROM LIST")
            {
                course.Status = "Plan to take";
            }
            else {
                course.Status = picker.SelectedItem.ToString();
            }
            //Validate that the instructor information is not empty.
            if (IsGoodEmail(instructorEmail.Text) && !string.IsNullOrEmpty(instructorName.Text) &&
                !string.IsNullOrEmpty(instructorPhone.Text) && IsGoodPhoneNumber(instructorPhone.Text))
            {
                if (course.StartDate < course.EndDate && course.ObjectiveStart < course.ObjectiveEnd &&
                    course.PerformanceStart < course.PerformanceEnd)
                {
                    try
                    {
                        using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
                        {
                            connection.CreateTable<Course>();
                            var successfulUpdate = connection.Insert(course);
                            if (successfulUpdate > 0)
                            {
                                DisplayAlert("SUCCESS", "The Course has been updated successfully.", "OK");
                                DataSet.ShowNotifications();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        DisplayAlert("ERROR", "The Course was not saved to the database. " + e.Message, "OK");
                    }
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("WARNING", "The Course's end date and the objectives' end dates MUST be later than their start dates.", "OK");
                }
            }
            else {
                DisplayAlert("WARNING", "The Instructor information fields are mandatory and cannot be blank. Be sure that a valid email address and a valid phone number was entered.", "OK");
            }
        }
    }
}