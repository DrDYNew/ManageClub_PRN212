using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
using ManageClub_PRN212.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ManageClub_PRN212.WPF.Admin
{
    public partial class ReportWPF : Window
    {
        private readonly ReportDAO _rDAO;
        private Button _selectedButton;
        private List<ReportResult> _clubReports;
        private int _selectedMonth;
        private User _currentUser;

        public ReportWPF()
        {
            InitializeComponent();
            _rDAO = new ReportDAO();
            _clubReports = new List<ReportResult>();
            LoadMonths();
            cbReportType.SelectedIndex = 0;
            _selectedButton = BtnReport;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
        }

        public ReportWPF(User user) : this()
        {
            _currentUser = user;
        }

        private void LoadMonths()
        {
            cbMonth.ItemsSource = Enumerable.Range(1, 12).Select(m => new { Month = m, Name = new DateTime(DateTime.Now.Year, m, 1).ToString("MMMM") });
            cbMonth.DisplayMemberPath = "Name";
            cbMonth.SelectedValuePath = "Month";
            cbMonth.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void LoadReportData(int month)
        {
            _clubReports.Clear();
            var clubs = _rDAO.GetAllClubs();
            foreach (var club in clubs)
            {
                var report = new ReportResult
                {
                    ClubName = club.ClubName,
                    NewMembers = _rDAO.GetNewMembersByMonth(club.ClubId, month, DateTime.Now.Year),
                    EventCount = _rDAO.GetEventCountByMonth(club.ClubId, month, DateTime.Now.Year),
                    TotalParticipants = _rDAO.GetTotalParticipantsByMonth(club.ClubId, month, DateTime.Now.Year),
                    TotalCost = _rDAO.GetTotalCostByMonth(club.ClubId, month, DateTime.Now.Year),
                    PositiveFeedbackCount = _rDAO.GetPositiveFeedbackCountByMonth(club.ClubId, month, DateTime.Now.Year),
                    NegativeFeedbackCount = _rDAO.GetNegativeFeedbackCountByMonth(club.ClubId, month, DateTime.Now.Year)
                };
                _clubReports.Add(report);
            }
            UpdateChart();
        }

        private void UpdateChart()
        {
            pieChart.Visibility = Visibility.Collapsed;
            columnChart.Visibility = Visibility.Collapsed;

            switch (cbReportType.SelectedIndex)
            {
                case 0: 
                    pieChart.Visibility = Visibility.Visible;
                    var newMembersSeries = new SeriesCollection();
                    foreach (var report in _clubReports)
                    {
                        newMembersSeries.Add(new PieSeries
                        {
                            Title = report.ClubName,
                            Values = new ChartValues<int> { report.NewMembers },
                            DataLabels = true
                        });
                    }
                    pieChart.Series = newMembersSeries;
                    break;

                case 1:
                    columnChart.Visibility = Visibility.Visible;
                    var eventSeries = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Title = "Events",
                            Values = new ChartValues<int>(_clubReports.Select(r => r.EventCount)),
                            DataLabels = true
                        }
                    };
                    columnChart.Series = eventSeries;
                    columnChart.AxisX[0].Labels = _clubReports.Select(r => r.ClubName).ToArray();
                    break;

                case 2: // Participants (Pie)
                    pieChart.Visibility = Visibility.Visible;
                    var participantsSeries = new SeriesCollection();
                    foreach (var report in _clubReports)
                    {
                        participantsSeries.Add(new PieSeries
                        {
                            Title = report.ClubName,
                            Values = new ChartValues<int> { report.TotalParticipants },
                            DataLabels = true
                        });
                    }
                    pieChart.Series = participantsSeries;
                    break;

                case 3: 
                    columnChart.Visibility = Visibility.Visible;
                    var costSeries = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Title = "Total Cost",
                            Values = new ChartValues<decimal>(_clubReports.Select(r => r.TotalCost)),
                            DataLabels = true
                        }
                    };
                    columnChart.Series = costSeries;
                    columnChart.AxisX[0].Labels = _clubReports.Select(r => r.ClubName).ToArray();
                    break;

                case 4: 
                    columnChart.Visibility = Visibility.Visible;
                    var feedbackSeries = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Title = "Positive Feedback (Rating >= 4)",
                            Values = new ChartValues<int>(_clubReports.Select(r => r.PositiveFeedbackCount)),
                            DataLabels = true,
                            Fill = System.Windows.Media.Brushes.Green 
                        },
                        new ColumnSeries
                        {
                            Title = "Negative Feedback (Rating <= 3)",
                            Values = new ChartValues<int>(_clubReports.Select(r => r.NegativeFeedbackCount)),
                            DataLabels = true,
                            Fill = System.Windows.Media.Brushes.Red 
                        }
                    };
                    columnChart.Series = feedbackSeries;
                    columnChart.AxisX[0].Labels = _clubReports.Select(r => r.ClubName).ToArray();
                    columnChart.AxisY[0].Title = "Feedback Count";
                    break;
            }
        }

        private void CbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMonth.SelectedValue != null)
            {
                _selectedMonth = (int)cbMonth.SelectedValue;
                LoadReportData(_selectedMonth);
            }
        }

        private void CbReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateChart();
        }

        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"ClubReport_{cbMonth.Text}_{DateTime.Now.Year}.pdf"
                };
                if (dialog.ShowDialog() == true)
                {
                    ExportToPdf(dialog.FileName);
                    MessageBox.Show("Report exported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToPdf(string filePath)
        {
            using (var document = new Document())
            {
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();
                document.Add(new Paragraph($"Club Report for {cbMonth.Text} {DateTime.Now.Year}"));
                document.Add(new Paragraph(" "));

                var table = new PdfPTable(7); // Thêm 2 cột cho Positive và Negative Feedback
                table.WidthPercentage = 100;
                table.AddCell("Club Name");
                table.AddCell("New Members");
                table.AddCell("Events");
                table.AddCell("Participants");
                table.AddCell("Total Cost");
                table.AddCell("Positive Feedback (Rating >= 4)");
                table.AddCell("Negative Feedback (Rating <= 3)");

                foreach (var report in _clubReports)
                {
                    table.AddCell(report.ClubName);
                    table.AddCell(report.NewMembers.ToString());
                    table.AddCell(report.EventCount.ToString());
                    table.AddCell(report.TotalParticipants.ToString());
                    table.AddCell(report.TotalCost.ToString("C"));
                    table.AddCell(report.PositiveFeedbackCount.ToString());
                    table.AddCell(report.NegativeFeedbackCount.ToString());
                }

                document.Add(table);
                document.Close();
            }
        }

        private void BtnUserManagement_Click(object sender, RoutedEventArgs e)
        {
            UpdateSidebarButtonStyle(sender);
            new AccountWindow().Show();
            this.Close();
        }

        private void BtnClubManagement_Click(object sender, RoutedEventArgs e)
        {
            UpdateSidebarButtonStyle(sender);
            new ClubManagement(_currentUser).Show();
            this.Close();
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            UpdateSidebarButtonStyle(sender);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                new Login().Show();
                this.Close();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateSidebarButtonStyle(object sender)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
        }
    }
}