using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
using ManageClub_PRN212.ViewModels;
using ManageClub_PRN212.WPF.Member;
using ManageClub_PRN212.WPF.President;

namespace ManageClub_PRN212.WPF
{
    /// <summary>
    /// Interaction logic for EventFeedbackWPF.xaml
    /// </summary>
    public partial class EventFeedbackWPF : Window
    {
        private readonly EventDAO _eventDAO;
        private readonly EventFeedbackDAO _feedbackDAO;
        private readonly User _currentUser;
        private readonly Event _currentEvent;
        private List<FeedbackViewModel> _feedbackViewModels;
        private int _selectedRating = 0;
        private List<Button> _starButtons;

        public EventFeedbackWPF(User currentUser, Event selectedEvent)
        {
            InitializeComponent();
            _eventDAO = new EventDAO();
            _feedbackDAO = new EventFeedbackDAO();
            _currentUser = currentUser;
            _currentEvent = selectedEvent;
            _starButtons = new List<Button>();

            // Set user information in footer
            CurrentUserText.Text = _currentUser.FullName;
            CurrentRoleText.Text = _currentUser.Role.RoleName;

            // Store star buttons for easy access
            _starButtons.Add(Star1);
            _starButtons.Add(Star2);
            _starButtons.Add(Star3);
            _starButtons.Add(Star4);
            _starButtons.Add(Star5);

            // Load event details and feedback
            LoadEventDetails();
            LoadFeedback();
            SetupUIByRole();
        }

        private void LoadEventDetails()
        {
            if (_currentEvent == null)
            {
                MessageBox.Show("Event information is missing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            EventNameText.Text = _currentEvent.EventName;
            EventDateText.Text = _currentEvent.EventDate.ToString("dd/MM/yyyy");
            EventLocationText.Text = _currentEvent.Location;
            
            // Update average rating display
            UpdateAverageRatingDisplay();
        }

        private void UpdateAverageRatingDisplay()
        {
            double averageRating = _feedbackDAO.GetAverageRating(_currentEvent.EventId);
            AverageRatingText.Text = averageRating.ToString("0.0");
            
            // Update star display
            int fullStars = (int)Math.Floor(averageRating);
            bool halfStar = (averageRating - fullStars) >= 0.5;
            
            string stars = new string('★', fullStars);
            if (halfStar) stars += "½";
            stars += new string('☆', 5 - fullStars - (halfStar ? 1 : 0));
            
            AverageRatingStars.Text = stars;
        }

        private void LoadFeedback()
        {
            try
            {
                // Get all feedback for this event
                var feedbacks = _feedbackDAO.GetFeedbackByEvent(_currentEvent.EventId);
                
                _feedbackViewModels = new List<FeedbackViewModel>();
                
                foreach (var feedback in feedbacks)
                {
                    var viewModel = new FeedbackViewModel
                    {
                        FeedbackId = feedback.FeedbackId,
                        EventId = feedback.EventId,
                        UserId = feedback.UserId,
                        Rating = feedback.Rating ?? 0,
                        Comments = feedback.Comments,
                        User = feedback.User,
                        Event = feedback.Event
                    };
                    
                    // Set delete button visibility based on user role
                    bool canDelete = _currentUser.RoleId == 1 || // Admin
                                     _currentUser.RoleId == 2 || // Club President
                                     feedback.UserId == _currentUser.UserId; // Own feedback
                    
                    viewModel.DeleteButtonVisibility = canDelete ? Visibility.Visible : Visibility.Collapsed;
                    
                    _feedbackViewModels.Add(viewModel);
                }
                
                FeedbackListBox.ItemsSource = _feedbackViewModels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading feedback: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetupUIByRole()
        {
            // Check if user is a member of the club and has participated in the event
            bool canSubmitFeedback = _currentUser.RoleId == 3 && // Member
                                     _feedbackDAO.HasUserParticipatedInEvent(_currentEvent.EventId, _currentUser.UserId);

            // Also check if user has already submitted feedback
            if (_feedbackDAO.HasUserSubmittedFeedback(_currentEvent.EventId, _currentUser.UserId))
            {
                canSubmitFeedback = false;
            }
            
            SubmitFeedbackButton.Visibility = canSubmitFeedback ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SubmitFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            // Show the feedback submission panel
            SubmitFeedbackPanel.Visibility = Visibility.Visible;
            
            // Reset the form
            _selectedRating = 0;
            CommentsTextBox.Text = string.Empty;
            UpdateStarDisplay();
        }

        private void Star_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button starButton && int.TryParse(starButton.Tag.ToString(), out int rating))
            {
                _selectedRating = rating;
                UpdateStarDisplay();
            }
        }

        private void UpdateStarDisplay()
        {
            for (int i = 0; i < _starButtons.Count; i++)
            {
                _starButtons[i].Content = i < _selectedRating ? "★" : "☆";
                _starButtons[i].Foreground = i < _selectedRating 
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC107")) 
                    : new SolidColorBrush(Colors.Black);
            }
        }

        private void SubmitFeedback_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRating == 0)
            {
                MessageBox.Show("Please select a rating.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CommentsTextBox.Text))
            {
                MessageBox.Show("Please add some comments about your experience.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var feedback = new EventFeedback
                {
                    EventId = _currentEvent.EventId,
                    UserId = _currentUser.UserId,
                    Rating = _selectedRating,
                    Comments = CommentsTextBox.Text.Trim()
                };

                bool result = _feedbackDAO.AddFeedback(feedback);
                
                if (result)
                {
                    MessageBox.Show("Thank you for your feedback!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    SubmitFeedbackPanel.Visibility = Visibility.Collapsed;
                    
                    // Reload feedback list and update UI
                    LoadFeedback();
                    UpdateAverageRatingDisplay();
                    SetupUIByRole(); // Update submit button visibility
                }
                else
                {
                    MessageBox.Show("You have already submitted feedback for this event.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    SubmitFeedbackPanel.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting feedback: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelFeedback_Click(object sender, RoutedEventArgs e)
        {
            SubmitFeedbackPanel.Visibility = Visibility.Collapsed;
        }

        private void DeleteFeedback_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag != null)
            {
                int feedbackId = Convert.ToInt32(deleteButton.Tag);
                
                var result = MessageBox.Show("Are you sure you want to delete this feedback?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        bool deleted = _feedbackDAO.DeleteFeedback(feedbackId);
                        
                        if (deleted)
                        {
                            MessageBox.Show("Feedback deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                            // Reload feedback list and update UI
                            LoadFeedback();
                            UpdateAverageRatingDisplay();
                            SetupUIByRole(); // Update submit button visibility if it was the user's own feedback
                        }
                        else
                        {
                            MessageBox.Show("Could not delete feedback.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting feedback: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 