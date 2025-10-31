using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using ToDoList.Model;
using System.Linq;

namespace ToDoList
{
    public partial class MainWindow : Window
    {

        // функция изменения такая,потому что я устал
        public MainWindow()
        {
            InitializeComponent();
            LoadTasks();
            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddTask();
            }
        }
        private void AddTask()
        {
            if (task.Text == string.Empty)
            {
                MessageBox.Show("Какая задача?");
                return;
            }
            if (task.Text.Length >= 10)
            {
                MessageBox.Show("Много букв");
                return;
            }

            using (var context = new DbContact())
            {
                context.DoLists.Add(new DoList { Name = task.Text, Status = 1 });
                context.SaveChanges();
            }

            AddTaskToListBox(task.Text);

            task.Text = string.Empty;
        }

        private void LoadTasks()
        {
            using (var context = new DbContact())
            {
                listBox.Items.Clear();
                listBox2.Items.Clear();
                var tasks = context.DoLists.ToList();
                foreach (var task in tasks)
                {
                    if (task.Status == 1)
                    {
                        AddTaskToListBox(task.Name);
                    }

                    if (task.Status == 2)
                    {
                        AddTaskToListBox2(task.Name);
                    }
                }
            }
        }

        private void AddTaskToListBox(string taskName)
        {
            ListBoxItem newItem = new ListBoxItem();
            newItem.Background = Brushes.White;
            newItem.Height = 31;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            CheckBox checkBox = new CheckBox();
            checkBox.Background = Brushes.Black;
            checkBox.Checked += CheckBox_Checked; 
            checkBox.Unchecked += CheckBox_Unchecked; 
            stackPanel.Children.Add(checkBox);

            TextBlock textBlock = new TextBlock();
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Margin = new Thickness(50, 0, 0, 0);
            textBlock.Text = taskName; 
            stackPanel.Children.Add(textBlock);

            Button closeButton = new Button();
            closeButton.Click += CloseButton_Click;
            closeButton.HorizontalAlignment = HorizontalAlignment.Center;
            closeButton.VerticalAlignment = VerticalAlignment.Center;
            closeButton.Margin = new Thickness(100, 0, 0, 0);
            closeButton.Foreground = Brushes.Red;
            closeButton.Background = Brushes.Transparent;
            closeButton.BorderBrush = Brushes.Transparent;
            PackIcon closeIcon = new PackIcon();
            closeIcon.Kind = PackIconKind.Close;
            closeButton.Background = Brushes.Transparent;
            closeIcon.Width = 20;
            closeIcon.Height = 20;
            closeButton.Content = closeIcon;

            stackPanel.Children.Add(closeButton);

            newItem.Content = stackPanel;
            

            listBox.Items.Add(newItem);
            task.Text = string.Empty;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTaskStatus(sender, 2);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateTaskStatus(sender, 1);
        }

        private void UpdateTaskStatus(object sender, int status)
        {
            CheckBox checkBox = (CheckBox)sender;

            StackPanel stackPanel = (StackPanel)checkBox.Parent;

            TextBlock textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
            if (textBlock != null)
            {
                string taskName = textBlock.Text;

                using (var context = new DbContact())
                {
                    var taskToUpdate = context.DoLists.FirstOrDefault(task => task.Name == taskName);
                    if (taskToUpdate != null)
                    {
                        taskToUpdate.Status = status;
                        context.SaveChanges();
                    }
                }
            }

            LoadTasks();
        }
        private void AddTaskToListBox2(string taskName)
        {
            ListBoxItem newItem = new ListBoxItem();
            newItem.Background = Brushes.White;
            newItem.Height = 31;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            CheckBox checkBox = new CheckBox();
            checkBox.Background = Brushes.Black;
            checkBox.Checked += CheckBox_Checked2;
            stackPanel.Children.Add(checkBox);

            TextBlock textBlock = new TextBlock();
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Margin = new Thickness(50, 0, 0, 0);
            textBlock.Text = taskName;
            stackPanel.Children.Add(textBlock);

            Button closeButton = new Button();
            closeButton.Click += CloseButton_Click;
            closeButton.HorizontalAlignment = HorizontalAlignment.Center;
            closeButton.VerticalAlignment = VerticalAlignment.Center;
            closeButton.Margin = new Thickness(100, 0, 0, 0);
            closeButton.Foreground = Brushes.Red;
            closeButton.Background = Brushes.Transparent;
            closeButton.BorderBrush = Brushes.Transparent;
            PackIcon closeIcon = new PackIcon();
            closeIcon.Kind = PackIconKind.Close;
            closeButton.Background = Brushes.Transparent;
            closeIcon.Width = 20;
            closeIcon.Height = 20;
            closeButton.Content = closeIcon;

            stackPanel.Children.Add(closeButton);

            newItem.Content = stackPanel;

            listBox2.Items.Add(newItem);
        }

        private void CheckBox_Checked2(object sender, RoutedEventArgs e)
        {
            UpdateTaskStatus2(sender, 1);
        }



        private void UpdateTaskStatus2(object sender, int status)
        {
            CheckBox checkBox = (CheckBox)sender;
            StackPanel stackPanel = (StackPanel)checkBox.Parent;
            TextBlock textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
            if (textBlock != null)
            {
                string taskName = textBlock.Text;

                using (var context = new DbContact())
                {
                    var taskToUpdate = context.DoLists.FirstOrDefault(task => task.Name == taskName);
                    if (taskToUpdate != null)
                    {
                        taskToUpdate.Status = status;
                        context.SaveChanges();
                    }
                }
            }
            LoadTasks();
        }


        private void AddClick(object sender, RoutedEventArgs e)
        {
            if (task.Text == string.Empty)
            {
                MessageBox.Show("Какая задача?");
                return;
            }
            if (task.Text.Length >= 10)
            {
                MessageBox.Show("Много букв");
                return;
            }

            using (var context = new DbContact())
            {
                context.DoLists.Add(new DoList { Name = task.Text, Status = 1});
                context.SaveChanges();
            }

            ListBoxItem newItem = new ListBoxItem();
            newItem.Background = Brushes.White;
            newItem.Height = 31;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            CheckBox checkBox = new CheckBox();
            checkBox.Background = Brushes.Black;
            stackPanel.Children.Add(checkBox);

            TextBlock textBlock = new TextBlock();
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Margin = new Thickness(50, 0, 0, 0);
            textBlock.Text = task.Text;
            stackPanel.Children.Add(textBlock);

            Button closeButton = new Button();
            closeButton.Click += CloseButton_Click;
            closeButton.HorizontalAlignment = HorizontalAlignment.Center;
            closeButton.VerticalAlignment = VerticalAlignment.Center;
            closeButton.Margin = new Thickness(100, 0, 0, 0);
            closeButton.Foreground = Brushes.Red;
            closeButton.Background = Brushes.Transparent;
            closeButton.BorderBrush = Brushes.Transparent;
            PackIcon closeIcon = new PackIcon();
            closeIcon.Kind = PackIconKind.Close;
            closeButton.Background = Brushes.Transparent;
            closeIcon.Width = 20;
            closeIcon.Height = 20;
            closeButton.Content = closeIcon;
            stackPanel.Children.Add(closeButton);

            newItem.Content = stackPanel;

            listBox.Items.Add(newItem);

            LoadTasks();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Button closeButton = (Button)sender;

            StackPanel stackPanel = (StackPanel)closeButton.Parent;

            TextBlock textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
            if (textBlock != null)
            {
                string taskName = textBlock.Text;

                using (var context = new DbContact())
                {
                    var taskToRemove = context.DoLists.FirstOrDefault(task => task.Name == taskName);
                    if (taskToRemove != null)
                    {
                        context.DoLists.Remove(taskToRemove);
                        context.SaveChanges();
                    }
                }


                ListBoxItem itemToRemove = (ListBoxItem)stackPanel.Parent;

                listBox.Items.Remove(itemToRemove);
                LoadTasks();
            }
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null && listBox.SelectedItem != null)
            {
                ListBoxItem selectedItem = listBox.SelectedItem as ListBoxItem;
                if (selectedItem != null)
                {
                    StackPanel stackPanel = selectedItem.Content as StackPanel;
                    if (stackPanel != null)
                    {
                        TextBlock textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
                        if (textBlock != null)
                        {
                            task.Text = textBlock.Text;
                        }
                    }
                }
            }
        }

        private void ChangeClick(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = listBox.SelectedItem as ListBoxItem;
            if (selectedItem != null)
            {
                StackPanel stackPanel = selectedItem.Content as StackPanel;
                if (stackPanel != null)
                {
                    TextBlock textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
                    if (textBlock != null)
                    {
                        string oldTaskName = textBlock.Text;
                        string newTaskName = task.Text;

                        using (var context = new DbContact())
                        {
                            var taskToUpdate = context.DoLists.FirstOrDefault(task => task.Name == oldTaskName);
                            if (taskToUpdate != null)
                            {
                                taskToUpdate.Name = newTaskName;
                                context.SaveChanges();
                            }
                        }

                        textBlock.Text = newTaskName;
                    }
                }
            }
        }

        
    }
}
