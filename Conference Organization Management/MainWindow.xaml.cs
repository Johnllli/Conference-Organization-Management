using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using COM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;


namespace Conference_Organization_Management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        cnCOM cn;
        public MainWindow()
        {
            InitializeComponent();
            cn = new cnCOM();
            InitializeDB();

        }

        private void InitializeDB()
        {
            cn.Database.EnsureCreated();
            if (cn.Conferences != null)
            {
                if (!cn.Conferences.Any())
                {
                    if (!cn.Presentations.Any())
                    {
                        SeeDB();
                    }
                }
                /*else
                    showdata();*/
               
            }
        }
        private void showdata()
        {
            var s = "";
            foreach (var p in cn.Conferences)
            {
                s += p.ID + ", " + p.Title;

            }
            Content = s;
        }

       private void SeeDB()
        {
            var c1 = new Conferences 
            { 
                ID = 1, Title = "First show", 
                StartDate = new DateTime(2024, 05, 01), EndDate = new DateTime(2024, 05, 01), 
                Location = "Building A" 
            };
            var c2 = new Conferences
            {
                ID = 2, Title = "Second show",
                StartDate = new DateTime(2024, 05, 02), EndDate = new DateTime(2024, 05, 02),
                Location = "Building B"
            };
            var c3 = new Conferences
            {
                ID = 3, Title = "Third show",
                StartDate = new DateTime(2024, 05, 03), EndDate = new DateTime(2024, 05, 03),
                Location = "Building C"
            };
            var pt0 = new Presenter
            {
                ID = 1,
                Name = "Big Human",
                FieldOfExpertise = "Big human knowledge"
            };
            var pt1 = new Presenter
            {
                ID = 2,
                Name = "Medium Human",
                FieldOfExpertise = "Medium human knowledge"
            };
            var pt2 = new Presenter
            {
                ID = 3,
                Name = "Small Human",
                FieldOfExpertise = "Small human knowledge"
            };
            var p00 = new Presentation
            {
                ID = 1,
                PresentationTitle = "How do big human live",
                DateAndTime = new DateTime(2024, 05, 01, 9, 0, 0),
                Conferences = c1,
                Presenter = pt0
                
            };
            var p01 = new Presentation
            {
                ID = 2,
                PresentationTitle = "How do Medium human live",
                DateAndTime = new DateTime(2024, 05, 01, 12, 0, 0),
                Conferences = c1,
                Presenter = pt1

            };
            var p02 = new Presentation
            {
                ID = 3,
                PresentationTitle = "How do Small human live",
                DateAndTime = new DateTime(2024, 05, 01, 15, 0, 0),
                Conferences = c1,
                Presenter = pt2

            };
            var p10 = new Presentation
            {
                ID = 4,
                PresentationTitle = "How do big human eat",
                DateAndTime = new DateTime(2024, 05, 02, 9, 0, 0),
                Conferences = c2,
                Presenter = pt0

            };
            var p11 = new Presentation
            {
                ID = 5,
                PresentationTitle = "How do Medium human eat",
                DateAndTime = new DateTime(2024, 05, 02, 12, 0, 0),
                Conferences = c2,
                Presenter = pt1

            };
            var p12 = new Presentation
            {
                ID = 6,
                PresentationTitle = "How do Small human eat",
                DateAndTime = new DateTime(2024, 05, 02, 15, 0, 0),
                Conferences = c2,
                Presenter = pt2

            };
            var p20 = new Presentation
            {
                ID = 7,
                PresentationTitle = "How do big human sleep",
                DateAndTime = new DateTime(2024, 05, 03, 9, 0, 0),
                Conferences = c3,
                Presenter = pt0

            };
            var p21 = new Presentation
            {
                ID = 8,
                PresentationTitle = "How do Medium human sleep",
                DateAndTime = new DateTime(2024, 05, 03, 12, 0, 0),
                Conferences = c3,
                Presenter = pt1

            };
            var p22 = new Presentation
            {
                ID = 9,
                PresentationTitle = "How do Small human sleep",
                DateAndTime = new DateTime(2024, 05, 03, 15, 0, 0),
                Conferences = c3,
                Presenter = pt2

            };

            c1.Presentations.Add(p00);
            c1.Presentations.Add(p01);
            c1.Presentations.Add(p02);
            c2.Presentations.Add(p10);
            c2.Presentations.Add(p11);
            c2.Presentations.Add(p12);
            c3.Presentations.Add(p20);
            c3.Presentations.Add(p21);
            c3.Presentations.Add(p22);
            cn.Conferences.AddRange([c1, c2, c3]);
            cn.Presentations.AddRange([p00, p01, p02, p10, p11, p12, p20, p21, p22]);
            cn.Presenters.AddRange([pt0, pt1, pt2]);
            cn.SaveChanges();

        }

        private void mi_FsClick(object sender, RoutedEventArgs e)
        {
            var FirstShowD = cn.Presentations
                .Include(p => p.Presenter)
                .Include(p => p.Conferences)
                .Where(p => p.Conferences.Title == "First show")
                .ToList();
            dgFs.ItemsSource = FirstShowD;
            dgFs.Visibility = Visibility.Visible;
        }

        private void mi_SsClick(object sender, RoutedEventArgs e)
        {
            var SecondShowD = cn.Presentations
                .Include(p => p.Presenter)
                .Include(p => p.Conferences)
                .Where(p => p.Conferences.Title == "Second show")
                .ToList();
            dgFs.ItemsSource = SecondShowD;
            dgFs.Visibility = Visibility.Visible;
        }

        private void mi_TsClick(object sender, RoutedEventArgs e)
        {
            var ThirdShowD = cn.Presentations
                .Include(p => p.Presenter)
                .Include(p => p.Conferences)
                .Where(p => p.Conferences.Title == "Third show")
                .ToList();
            dgFs.ItemsSource = ThirdShowD;
            dgFs.Visibility = Visibility.Visible;
        }
        private void mi_AsClick(object sender, RoutedEventArgs e)
        {
            var AllShowD = cn.Presentations
                .Include(p => p.Presenter)
                .Include(p => p.Conferences)
                .ToList();
            dgFs.ItemsSource = AllShowD;
            dgFs.Visibility = Visibility.Visible;
        }
        private void DetachDB(cnCOM cn)
        {
            cn.Database.OpenConnection();
            var connection = cn.Database.GetDbConnection();
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            string[] commands =
            { "USE master",
                $"ALTER DATABASE [{connection.Database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE",
                $"ALTER DATABASE [{connection.Database}] SET OFFLINE WITH ROLLBACK IMMEDIATE",
                $"EXEC sp_detach_db '{connection.Database}'"
            };
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = connection as SqlConnection;
                foreach (string command in commands)
                {
                    sqlCommand.CommandText = command;
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        private void mi_ExitClick(object sender, RoutedEventArgs e)
        {
            DetachDB(cn);
            Application.Current.Shutdown();

        }

        private void mi_ChangeClick(object sender, RoutedEventArgs e)
        {

        }

        
    }
}