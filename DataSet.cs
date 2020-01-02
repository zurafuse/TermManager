using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;
using Plugin.LocalNotifications;

namespace TermManager
{
    public static class DataSet
    {
        public static void CreateSampleData()
        {
            int termId = -999;
            string termName = "Term 1 (for evaluation purposes)";
            bool termIsFound = false;
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
            {
                connection.CreateTable<Term>();
                List<Term> Terms = connection.Table<Term>().ToList();
                for (var i = 0; i < Terms.Count; i++)
                {
                    if (Terms[i].Name == termName)
                    {
                        termIsFound = true;
                    }
                }
                //Only create the sample data if the term does not already exist.
                if (termIsFound != true)
                {
                    Term term = new Term
                    {
                        Name = termName,
                        StartDate = new DateTime(2019, 11, 1),
                        EndDate = new DateTime(2020, 4, 30)
                    };
                    int numberInserted = connection.Insert(term);
                    //If the Term insertion was successful, assign the Term Id.
                    if (numberInserted > 0)
                    {
                        termId = connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
                    }
                }
            }
            //If the Term was inserted, create a course for the Term.
            if (termId >= 0)
            {
                Course course = new Course {
                    TermId = termId,
                    Name = "Mobile Application Development Using C# – C971",
                    StartDate = new DateTime(2019, 11, 1),
                    EndDate = new DateTime(2019, 12, 15),
                    Status = "Plan to take",
                    InstructorName = "Timothy Horton",
                    InstructorPhone = "334-300-4786",
                    InstructorEmail = "thort15@wgu.edu",
                    ObjectiveName = "Xamarin Principles Exam",
                    ObjectiveStart = new DateTime(2019, 12, 1),
                    ObjectiveEnd = new DateTime(2019, 12, 3),
                    PerformanceName = "Sample Mobile Appliation",
                    PerformanceStart = new DateTime(2019, 12, 4),
                    PerformanceEnd = new DateTime(2019, 12, 15),
                    startNotify = true,
                    endNotify = true,
                    startObjNotify = true,
                    endObjNotify = true,
                    startPerfNotify = true,
                    endPerfNotify = true
    };
                using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
                {
                    connection.CreateTable<Course>();
                    connection.Insert(course);
                }
            }
        }

        public static void ShowNotifications() {
            List<Course> courses = new List<Course>();
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
            {
                connection.CreateTable<Course>();
                courses = connection.Table<Course>().ToList();
            }
            //Loops through each course and determine if a notification needs to be sent.
            for (var i = 0; i < courses.Count; i++) {
                //Notification for COURSE START DATE
                if (courses[i].startNotify == true) {
                    if (System.DateTime.Now >= courses[i].StartDate.AddDays(-1)) {
                        courses[i].startNotify = false;
                        CrossLocalNotifications.Current.Show("COURSE NOTIFICATION", "Start date is approaching for " + courses[i].Name, 101, DateTime.Now.AddSeconds(3));
                    }               
                }
                //Notification for COURSE END DATE
                if (courses[i].endNotify == true)
                {
                    if (System.DateTime.Now >= courses[i].EndDate.AddDays(-1))
                    {
                        courses[i].endNotify = false;
                        CrossLocalNotifications.Current.Show("COURSE NOTIFICATION", "End date is approaching for " + courses[i].Name, 101, DateTime.Now.AddSeconds(3));
                    }
                }
                //Notification for OBJECTIVE ASSESSMENT START DATE
                if (courses[i].startObjNotify == true)
                {
                    if (System.DateTime.Now >= courses[i].ObjectiveStart.AddDays(-1))
                    {
                        courses[i].startObjNotify = false;
                        CrossLocalNotifications.Current.Show("ASSESSMENT NOTIFICATION", "Start date is approaching for the Objective Assessment for " + courses[i].Name, 101, DateTime.Now.AddSeconds(3));
                    }
                }
                //Notification for OBJECTIVE ASSESSMENT END DATE
                if (courses[i].endObjNotify == true)
                {
                    if (System.DateTime.Now >= courses[i].ObjectiveEnd.AddDays(-1))
                    {
                        courses[i].endObjNotify = false;
                        CrossLocalNotifications.Current.Show("ASSESSMENT NOTIFICATION", "End date is approaching for the Objective Assessment for " + courses[i].Name, 101, DateTime.Now.AddSeconds(3));
                    }
                }
                //Notification for PERFORMANCE ASSESSMENT START DATE
                if (courses[i].startPerfNotify == true)
                {
                    if (System.DateTime.Now >= courses[i].PerformanceStart.AddDays(-1))
                    {
                        courses[i].startPerfNotify = false;
                        CrossLocalNotifications.Current.Show("ASSESSMENT NOTIFICATION", "Start date is approaching for the Performance Assessment for " + courses[i].Name, 101, DateTime.Now.AddSeconds(3));
                    }
                }
                //Notification for PERFORMANCE ASSESSMENT END DATE
                if (courses[i].endPerfNotify == true)
                {
                    if (System.DateTime.Now >= courses[i].PerformanceEnd.AddDays(-1))
                    {
                        courses[i].endPerfNotify = false;
                        CrossLocalNotifications.Current.Show("ASSESSMENT NOTIFICATION", "End date is approaching for the Performance Assessment for " + courses[i].Name, 101, DateTime.Now.AddSeconds(3));
                    }
                }
                //Update the course so notifications will not be sent repeatedly.
                using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DBPath))
                {
                    connection.Update(courses[i]);
                }
            }         
        }
    }
}
