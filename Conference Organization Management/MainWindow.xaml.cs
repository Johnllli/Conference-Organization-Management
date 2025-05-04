using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using COM;

namespace Conference_Organization_Management
{
    public partial class MainWindow : Window
    {
        cnCOM cn;

        public MainWindow()
        {
            InitializeComponent();
            cn = new cnCOM();
            InitializeDB();
            LoadConferenceComboBox();
        }

        private void InitializeDB()
        {
            cn.Database.EnsureCreated();
            if (cn.Conferences != null && !cn.Conferences.Any() && !cn.Presentations.Any())
            {
                SeeDB();
            }
        }

        private void SeeDB()
        {
            try
            {
                var c1 = new Conferences { ID = 1, Title = "First show", StartDate = new(2024, 05, 01), EndDate = new(2024, 05, 01), Location = "Building A" };
                var c2 = new Conferences { ID = 2, Title = "Second show", StartDate = new(2024, 05, 02), EndDate = new(2024, 05, 02), Location = "Building B" };
                var c3 = new Conferences { ID = 3, Title = "Third show", StartDate = new(2024, 05, 03), EndDate = new(2024, 05, 03), Location = "Building C" };

                var pt0 = new Presenter { ID = 1, Name = "Big Human", FieldOfExpertise = "Big human knowledge" };
                var pt1 = new Presenter { ID = 2, Name = "Medium Human", FieldOfExpertise = "Medium human knowledge" };
                var pt2 = new Presenter { ID = 3, Name = "Small Human", FieldOfExpertise = "Small human knowledge" };

                cn.Conferences.AddRange(c1, c2, c3);
                cn.Presenters.AddRange(pt0, pt1, pt2);

                cn.Presentations.AddRange(
                    new Presentation { ID = 1, PresentationTitle = "How do big human live", DateAndTime = new(2024, 05, 01, 9, 0, 0), Conferences = c1, Presenter = pt0 },
                    new Presentation { ID = 2, PresentationTitle = "How do Medium human live", DateAndTime = new(2024, 05, 01, 12, 0, 0), Conferences = c1, Presenter = pt1 },
                    new Presentation { ID = 3, PresentationTitle = "How do Small human live", DateAndTime = new(2024, 05, 01, 15, 0, 0), Conferences = c1, Presenter = pt2 },
                    new Presentation { ID = 4, PresentationTitle = "How do big human eat", DateAndTime = new(2024, 05, 02, 9, 0, 0), Conferences = c2, Presenter = pt0 },
                    new Presentation { ID = 5, PresentationTitle = "How do Medium human eat", DateAndTime = new(2024, 05, 02, 12, 0, 0), Conferences = c2, Presenter = pt1 },
                    new Presentation { ID = 6, PresentationTitle = "How do Small human eat", DateAndTime = new(2024, 05, 02, 15, 0, 0), Conferences = c2, Presenter = pt2 },
                    new Presentation { ID = 7, PresentationTitle = "How do big human sleep", DateAndTime = new(2024, 05, 03, 9, 0, 0), Conferences = c3, Presenter = pt0 },
                    new Presentation { ID = 8, PresentationTitle = "How do Medium human sleep", DateAndTime = new(2024, 05, 03, 12, 0, 0), Conferences = c3, Presenter = pt1 },
                    new Presentation { ID = 9, PresentationTitle = "How do Small human sleep", DateAndTime = new(2024, 05, 03, 15, 0, 0), Conferences = c3, Presenter = pt2 }
                );

                cn.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while initializing database: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void LoadConferenceComboBox()
        {
            cbConferenceTitle.ItemsSource = cn.Conferences.ToList();
            cbConferenceTitle.DisplayMemberPath = "Title";
        }

        private void cbConference_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbConferenceTitle.SelectedItem is Conferences selectedConf)
            {
                tbTitle.Text = selectedConf.Title;
                tbLocation.Text = selectedConf.Location;
                tbDateTime.Text = selectedConf.StartDate.ToString("yyyy-MM-dd HH:mm");
            }
        }

        private void btSaveConference_Click(object sender, RoutedEventArgs e)
        {
            if (cbConferenceTitle.SelectedItem is not Conferences selectedConf)
            {
                MessageBox.Show("Please select a conference to update.");
                return;
            }

            if (!ValidateConferenceForm()) return;

            selectedConf.Title = tbTitle.Text;
            selectedConf.Location = tbLocation.Text;
            selectedConf.StartDate = DateTime.Parse(tbDateTime.Text);
            selectedConf.EndDate = selectedConf.StartDate;

            try
            {
                cn.SaveChanges();
                MessageBox.Show("Conference updated.");
                grConferenceChangeNew.Visibility = Visibility.Collapsed;
                ResetConferenceForm();
                LoadConferenceComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating conference:\n{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void btSaveNewConference_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateConferenceForm()) return;

            var newConf = new Conferences
            {
                Title = tbTitle.Text,
                Location = tbLocation.Text,
                StartDate = DateTime.Parse(tbDateTime.Text),
                EndDate = DateTime.Parse(tbDateTime.Text)
            };

            cn.Conferences.Add(newConf);

            try
            {
                cn.SaveChanges();
                MessageBox.Show("New conference added.");
                grConferenceChangeNew.Visibility = Visibility.Collapsed;
                ResetConferenceForm();
                LoadConferenceComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding new conference:\n{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void ResetConferenceForm()
        {
            tbTitle.Text = "";
            tbLocation.Text = "";
            tbDateTime.Text = "";
            cbConferenceTitle.SelectedItem = null;
        }

        private bool ValidateConferenceForm()
        {
            if (string.IsNullOrWhiteSpace(tbTitle.Text) ||
                string.IsNullOrWhiteSpace(tbLocation.Text) ||
                string.IsNullOrWhiteSpace(tbDateTime.Text))
            {
                MessageBox.Show("All fields must be filled.");
                return false;
            }

            if (!DateTime.TryParse(tbDateTime.Text, out _))
            {
                MessageBox.Show("Invalid date format.");
                return false;
            }

            return true;
        }

        private void mi_ConferenceChangeNew_Click(object sender, RoutedEventArgs e)
        {
            HideAllViews();
            grConferenceChangeNew.Visibility = Visibility.Visible;
        }

        private void btBackConference_Click(object sender, RoutedEventArgs e)
        {
            grConferenceChangeNew.Visibility = Visibility.Collapsed;
        }

        private void mi_PresentationChangeNew_Click(object sender, RoutedEventArgs e)
        {
            HideAllViews();
            grPresentationChangeNew.Visibility = Visibility.Visible;

            cbPresentation.ItemsSource = cn.Presentations.Include(p => p.Conferences).Include(p => p.Presenter).ToList();
            cbPresConference.ItemsSource = cn.Conferences.ToList();
            cbPresPresenter.ItemsSource = cn.Presenters.ToList();
        }

        private void cbPresentation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPresentation.SelectedItem is Presentation selectedPres)
            {
                tbPresTitle.Text = selectedPres.PresentationTitle;
                tbPresDateTime.Text = selectedPres.DateAndTime.ToString("yyyy-MM-dd HH:mm");
                cbPresConference.SelectedItem = selectedPres.Conferences;
                cbPresPresenter.SelectedItem = selectedPres.Presenter;
            }
        }

        private void btSavePresentation_Click(object sender, RoutedEventArgs e)
        {
            if (cbPresentation.SelectedItem is not Presentation selectedPres || !ValidatePresentationForm()) return;

            selectedPres.PresentationTitle = tbPresTitle.Text;
            selectedPres.DateAndTime = DateTime.Parse(tbPresDateTime.Text);
            selectedPres.Conferences = cbPresConference.SelectedItem as Conferences;
            selectedPres.Presenter = cbPresPresenter.SelectedItem as Presenter;

            try
            {
                cn.SaveChanges();
                MessageBox.Show("Presentation updated.");
                grPresentationChangeNew.Visibility = Visibility.Collapsed;
                ResetPresentationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating presentation:\n{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void btSaveNewPresentation_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidatePresentationForm()) return;

            var newPres = new Presentation
            {
                PresentationTitle = tbPresTitle.Text,
                DateAndTime = DateTime.Parse(tbPresDateTime.Text),
                Conferences = cbPresConference.SelectedItem as Conferences,
                Presenter = cbPresPresenter.SelectedItem as Presenter
            };

            cn.Presentations.Add(newPres);

            try
            {
                cn.SaveChanges();
                MessageBox.Show("New presentation added.");
                grPresentationChangeNew.Visibility = Visibility.Collapsed;
                ResetPresentationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving presentation:\n{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void ResetPresentationForm()
        {
            tbPresTitle.Text = "";
            tbPresDateTime.Text = "";
            cbPresentation.SelectedItem = null;
            cbPresConference.SelectedItem = null;
            cbPresPresenter.SelectedItem = null;
        }

        private bool ValidatePresentationForm()
        {
            if (string.IsNullOrWhiteSpace(tbPresTitle.Text) ||
                string.IsNullOrWhiteSpace(tbPresDateTime.Text) ||
                cbPresConference.SelectedItem == null ||
                cbPresPresenter.SelectedItem == null)
            {
                MessageBox.Show("All fields must be filled.");
                return false;
            }

            if (!DateTime.TryParse(tbPresDateTime.Text, out _))
            {
                MessageBox.Show("Invalid date/time format.");
                return false;
            }

            return true;
        }

        private void btBackPresentation_Click(object sender, RoutedEventArgs e)
        {
            grPresentationChangeNew.Visibility = Visibility.Collapsed;
        }

        private void mi_FsClick(object sender, RoutedEventArgs e) => ShowData("First show");
        private void mi_SsClick(object sender, RoutedEventArgs e) => ShowData("Second show");
        private void mi_TsClick(object sender, RoutedEventArgs e) => ShowData("Third show");
        private void mi_AsClick(object sender, RoutedEventArgs e) => ShowData();

        private void ShowData(string title = null)
        {
            HideAllViews();
            var query = cn.Presentations.Include(p => p.Presenter).Include(p => p.Conferences);
            dgFs.ItemsSource = string.IsNullOrEmpty(title) ? query.ToList() : query.Where(p => p.Conferences.Title == title).ToList();
            dgFs.Visibility = Visibility.Visible;
        }

        private void mi_ExitClick(object sender, RoutedEventArgs e)
        {
            DetachDB(cn);
            Application.Current.Shutdown();
        }

        private void HideAllViews()
        {
            dgFs.Visibility = Visibility.Collapsed;
            grConferenceChangeNew.Visibility = Visibility.Collapsed;
            grPresentationChangeNew.Visibility = Visibility.Collapsed;
        }

        private void DetachDB(cnCOM cn)
        {
            cn.Database.OpenConnection();
            var connection = cn.Database.GetDbConnection();
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();

            string[] commands = {
                "USE master",
                $"ALTER DATABASE [{connection.Database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE",
                $"ALTER DATABASE [{connection.Database}] SET OFFLINE WITH ROLLBACK IMMEDIATE",
                $"EXEC sp_detach_db '{connection.Database}'"
            };

            using var sqlCommand = new SqlCommand { Connection = connection as SqlConnection };
            foreach (var cmd in commands)
            {
                sqlCommand.CommandText = cmd;
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}