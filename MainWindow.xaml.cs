using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MathQuizApp
{
    public partial class MainWindow : Window
    {
        private Random _random;
        private int _score;
        private int _timerSeconds;
        private int _correctAnswer;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _random = new Random();
            _score = 0;
            _timerSeconds = 30; // 30 seconds per question
            InitializeTimer();
            LoadNewQuestion();
        }

        // Initialize Timer
        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        // Timer Tick Event: Update the timer countdown every second
        private void Timer_Tick(object sender, EventArgs e)
        {
            _timerSeconds--;
            TimerTextBlock.Text = $"Time Left: {_timerSeconds}s";

            if (_timerSeconds == 0)
            {
                SubmitAnswer(); // Automatically submit answer when time is up
            }
        }

        // Start a new quiz question
        private void LoadNewQuestion()
        {
            _timerSeconds = 30;
            _timer.Start(); // Start the timer when a new question is loaded

            // Randomly generate a math question based on the difficulty level
            string difficulty = (DifficultyComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            int number1, number2;

            switch (difficulty)
            {
                case "Easy":
                    number1 = _random.Next(1, 10);
                    number2 = _random.Next(1, 10);
                    break;
                case "Medium":
                    number1 = _random.Next(10, 50);
                    number2 = _random.Next(10, 50);
                    break;
                case "Hard":
                    number1 = _random.Next(50, 100);
                    number2 = _random.Next(50, 100);
                    break;
                default:
                    number1 = _random.Next(1, 10);
                    number2 = _random.Next(1, 10);
                    break;
            }

            int operation = _random.Next(1, 5); // 1=Add, 2=Subtract, 3=Multiply, 4=Divide

            switch (operation)
            {
                case 1: // Addition
                    QuestionTextBlock.Text = $"{number1} + {number2} = ?";
                    _correctAnswer = number1 + number2;
                    break;
                case 2: // Subtraction
                    QuestionTextBlock.Text = $"{number1} - {number2} = ?";
                    _correctAnswer = number1 - number2;
                    break;
                case 3: // Multiplication
                    QuestionTextBlock.Text = $"{number1} * {number2} = ?";
                    _correctAnswer = number1 * number2;
                    break;
                case 4: // Division
                    QuestionTextBlock.Text = $"{number1} ÷ {number2} = ?";
                    _correctAnswer = number1 / number2;
                    break;
            }

            // Reset feedback and answer box
            FeedbackTextBlock.Text = "";
            AnswerTextBox.Text = "";
        }

        // Submit Answer Button Clicked
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            SubmitAnswer();
        }

        // Check and submit the answer
        private void SubmitAnswer()
        {
            _timer.Stop();

            if (int.TryParse(AnswerTextBox.Text, out int userAnswer))
            {
                if (userAnswer == _correctAnswer)
                {
                    _score++;
                    FeedbackTextBlock.Text = "Correct!";
                    FeedbackTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                }
                else
                {
                    FeedbackTextBlock.Text = "Incorrect!";
                    FeedbackTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                FeedbackTextBlock.Text = "Please enter a valid number.";
                FeedbackTextBlock.Foreground = System.Windows.Media.Brushes.Red;
            }

            // Update Score
            ScoreTextBlock.Text = $"Score: {_score}";

            // Load the next question after a short delay
            Dispatcher.InvokeAsync(() =>
            {
                System.Threading.Thread.Sleep(1000);
                LoadNewQuestion();
            });
        }
    }
}
