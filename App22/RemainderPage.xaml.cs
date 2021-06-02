using App22.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App22
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RemainderPage : Page,INotifyPropertyChanged
    {
        private ObservableCollection<User> Users;
        private ObservableCollection<Remainder> Remainders;
        private ObservableCollection<Remainder> DubRemainders;
        private ObservableCollection<User> Userfiltered;
        

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropetyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        public RemainderPage()
        {
   
            Remainders = new ObservableCollection<Remainder>();
            DubRemainders = new ObservableCollection<Remainder>(Remainders);
            this.InitializeComponent();
            Users = new ObservableCollection<User>();
            Userfiltered = new ObservableCollection<User>(Users);
            
            
        }

        private async void SetButton_Click(object sender, RoutedEventArgs e)
        {
            if ( string.IsNullOrWhiteSpace(MytextBox.Text))
            {
                MessageDialog dialog = new MessageDialog("We Need Something to Remaind you about");
                dialog.Commands.Add(new UICommand("ok") {Id=0 });

                dialog.DefaultCommandIndex = 0;
                
                var result = await dialog.ShowAsync();
                if((int)result.Id == 0)
                {
                    return;
                }
                
            }

            var selecteddates = Mycalenderview.SelectedDates;
            var selecteditem = Mylistview.SelectedItems.Cast<User>();
            var time = MyTimepicker.SelectedTime;
            Remainders.Add(new Remainder { RemainderDate = selecteddates.FirstOrDefault()+ time.GetValueOrDefault() - selecteddates.FirstOrDefault().TimeOfDay, RemainderName = MytextBox.Text, ReminaderUser = selecteditem.FirstOrDefault() });
            Mylistview.SelectedValue = false;
            Mycalenderview.SelectedDates.Clear();
            MyTimepicker.SelectedTime = null;
            ClockButton.Background = new SolidColorBrush(Colors.White);
            AtButton.Background = new SolidColorBrush(Colors.White);
            MytextBox.Text ="";
            Mylistview.ItemsSource = Users;
           



        }

        private void ClockSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ClockFlyout.Hide();
            ClockButton.Background = new SolidColorBrush(Colors.White);
        }

        private void ClockCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClockFlyout.Hide();
            ClockButton.Background = new SolidColorBrush(Colors.White);
            Mycalenderview.SelectedDates.Clear();
            MyTimepicker.SelectedTime = null;

        }

        private void AtSaveButton_Click(object sender, RoutedEventArgs e)
        {
            AtFlyout.Hide();
            AtButton.Background = new SolidColorBrush(Colors.White);

        }

        private void AtCancelButton_Click(object sender, RoutedEventArgs e)
        {
            AtFlyout.Hide();
            AtButton.Background = new SolidColorBrush(Colors.White);
            Mylistview.SelectedValue = false;
        }

        

        private void MyCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void FormeButton_Click(object sender, RoutedEventArgs e)
        {
            FormeButton.FontSize = 20;
            AssignButton.FontSize = 18;
           
            FormeButton.BorderThickness = new Thickness(0, 0, 0, 2);
            AssignButton.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void AssignButton_Click(object sender, RoutedEventArgs e)
        {
            AssignButton.FontSize = 20;
            FormeButton.FontSize = 18;
            AssignButton.BorderThickness = new Thickness(0, 0, 0, 2);
            FormeButton.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void MyUserTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {

            Mylistview.ItemsSource = Userfiltered;
            List <User> Tempfiltered = Users.Where(p => p.Name.Contains(MyUserTextbox.Text, StringComparison.InvariantCultureIgnoreCase)).ToList();
            foreach (var ite in Users)
            {
                if (Tempfiltered.Contains(ite) && !Userfiltered.Contains(ite))
                {
                    Userfiltered.Add(ite);
                }
                else if (!Tempfiltered.Contains(ite))
                {
                    Userfiltered.Remove(ite);
                }
            }
        }

       

        private void AtButton_Click(object sender, RoutedEventArgs e)
        {
            MyUserTextbox.Text = "";
        }

        private async  void Myuserbutton_Click(object sender, RoutedEventArgs e)
        {
            await System.Threading.Tasks.Task.Run(() => ComputeNextMove().ConfigureAwait(false));
            
        }
        private async System.Threading.Tasks.Task ComputeNextMove()
        { 
            //Get the folder
            var _folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            _folder = await _folder.GetFolderAsync("Model");
            //Get the file
            var _file = await _folder.GetFileAsync("test.txt");

            // read content
            IList<string> _ReadLines = await Windows.Storage.FileIO.ReadLinesAsync(_file);
            foreach (var line in _ReadLines)
            {
                Users.Add(new User { Name = line.Split(" ")[0], Position = line.Split(" ")[1] });
            }
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Mylistview.ItemsSource = Users;
                Myuserbutton.Visibility = Visibility.Collapsed;
                MessageDialog dialog = new MessageDialog("Load the users from the file");
                dialog.Commands.Add(new UICommand("ok") { Id = 0 });
                var result = dialog.ShowAsync();
            });
        }

       
        private void ChangeTextbox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBox textBox = sender as TextBox;

                BindingExpression be = textBox.GetBindingExpression(TextBox.TextProperty);
                if (be != null)
                    be.UpdateSource();
            }
        }
     
      

        private void MyChangeButton_Click(object sender, RoutedEventArgs e)
        {
          /*  for (int i = 0; i < RemaindListview.Items.Count; i++)
            {
                Remainder re = (Remainder)RemaindListview.Items.ElementAt(i);
                re.RemainderName = "abc";
            }
          */
          for (int i = 0; i < Remainders.Count(); i++)
            {
                Remainders[i].RemainderName = "abc";
            }
           
        }

   /*     private void Button_Click(object sender, RoutedEventArgs e)
        {
            RemaindSplitview.IsPaneOpen = !RemaindSplitview.IsPaneOpen;
            Button button = (Button)sender;
            var datasource = (Remainder)button.DataContext;
            
           
        }
   */

      /*  private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBox textBox = sender as TextBox;

                BindingExpression be = textBox.GetBindingExpression(TextBox.TextProperty);
                if (be != null)
                    be.UpdateSource();
            }
        }
      */

       

         private void SplitButton_Click(object sender, RoutedEventArgs e)
         {
            var item=(Remainder)RemaindListview.SelectedItem;
            String Name = item.RemainderName;
            if (string.IsNullOrWhiteSpace(SplitviewTextbox.Text))
            {
                item.RemainderName = Name;
            }
            else
            {
                item.RemainderName = SplitviewTextbox.Text;
            }

            
            var selecteddate = SplitCalendarview.SelectedDates;

            
            if(SplitTimePicker.SelectedTime==null  && SplitCalendarview.SelectedDates.Count ==0 )
            {
                item.RemainderDate = item.RemainderDate;
            }
            else
            {
                item.RemainderDate = selecteddate.FirstOrDefault() + SplitTimePicker.SelectedTime.GetValueOrDefault() - selecteddate.FirstOrDefault().TimeOfDay;
            }

           
            RemaindListview.SelectedItem = null;
            SplitCalendarview.SelectedDates.Clear();
            SplitTimePicker.SelectedTime = null;
            RemaindSplitview.IsPaneOpen = false;
            SplitviewTextbox.Text = "";
            
        }

         private void SplitcancelButton_Click(object sender, RoutedEventArgs e)
         {
             RemaindSplitview.IsPaneOpen = false;
            RemaindListview.SelectedItem = null;
            SplitviewTextbox.Text = "";
            RemaindSplitview.IsPaneOpen = false;

         }

        private void RemaindListview_ItemClick(object sender, ItemClickEventArgs e)
        {
            RemaindSplitview.IsPaneOpen = !RemaindSplitview.IsPaneOpen;
            ListView list = (ListView)sender;
            var data = (Remainder)e.ClickedItem;
            SplitviewTextbox.Text = data.RemainderName;
            if(data.RemainderDate == DateTimeOffset.MinValue)
            {
                SplitviewTextBlock.Text = string.Empty.ToString();
            }
            else
            {
                SplitviewTextBlock.Text = data.RemainderDate.ToString();
            }
            
        }
    }
}
