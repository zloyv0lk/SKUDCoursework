using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SKUD.DataBase;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace SKUD.Pages
{
    /// <summary>
    /// Логика взаимодействия для SecurityPage.xaml
    /// </summary>
    public partial class SecurityPage : Page
    {
        employeer emp;
        public SecurityPage()
        {
            InitializeComponent();
            clearTB();
            guestLV.ItemsSource = DB.entities.guests.ToList();
            eventLV.ItemsSource = DB.entities.events.ToList();
            empList.IsChecked = true;

            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            timeTB.Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
            if(File.Exists("C:/Users/andrey/Desktop/SKUD/ArduinoServer/bin/Debug/TestFile.txt"))
            {
                try
                {
                    string cardInfo;
                    using (StreamReader reader = new StreamReader("C:/Users/andrey/Desktop/SKUD/ArduinoServer/bin/Debug/TestFile.txt"))
                    {
                        cardInfo = reader.ReadToEnd();

                        emp = DB.entities.employeers.FirstOrDefault(c => cardInfo.Contains(c.pass.pass_code));
                        departmentTB.Text = " " + emp.department.title;
                        nameTB.Text = " " + emp.surname + " " + emp.name + " " + emp.patronomyc;
                        uidTB.Text = " №" + Convert.ToString(emp.employeer_id);

                        using (var stream = new MemoryStream(emp.photo))
                        {
                            empPhoto.Source = BitmapFrame.Create(stream,BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        }

                        emp_eventLV.ItemsSource = DB.entities.events.Where(c => c.date.Year == DateTime.Now.Year && c.date.Month == DateTime.Now.Month && c.date.Day == DateTime.Now.Day && c.employeer.employeer_id == emp.employeer_id).ToList();
                    }
                }
                catch
                {
                    //MessageBox.Show("Приложите карту ещё раз");
                }
                
                    

            }
        }

        private void empLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            guest gst = (guest)guestLV.SelectedItem;
            fioTB.Text = gst.surname + " " + gst.name + " " + gst.patronomyc;
            passportTB.Text = gst.passport;
            dateTB.Text = Convert.ToString(gst.date);
        }

        private void fioSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchGuest();
        }
        void searchGuest()
        {
            guestLV.ItemsSource = DB.entities.guests.Where(c => c.surname.Contains(fioSearch.Text) || c.name.Contains(fioSearch.Text) || c.patronomyc.Contains(fioSearch.Text)).ToList();
        }


        private void guestEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                guest_event gst_event = new guest_event();
                guest gst = (guest)guestLV.SelectedItem;
                if (DB.entities.guest_event.FirstOrDefault(c => c.guest.guest_id == gst.guest_id && c.event_type.event_type_id == 1) == null)
                {
                    gst_event.guest = gst;
                    gst_event.date = DateTime.Now;
                    gst_event.event_type = DB.entities.event_type.FirstOrDefault(c => c.event_type_id == 1);
                    DB.entities.guest_event.AddOrUpdate(gst_event);
                    DB.entities.SaveChanges();
                    MessageBox.Show("Посетитель вошёл");
                }
                else
                    MessageBox.Show("Посетитель уже вошёл");
            }
            catch
            {
                MessageBox.Show("Выберите посетителя");
            }
        }

        private void guestExitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                guest_event gst_event = new guest_event();
                guest gst = (guest)guestLV.SelectedItem;
                if (DB.entities.guest_event.FirstOrDefault(c => c.guest.guest_id == gst.guest_id && c.event_type.event_type_id == 2) == null)
                {
                    gst_event.guest = gst;
                    gst_event.date = DateTime.Now;
                    gst_event.event_type = DB.entities.event_type.FirstOrDefault(c => c.event_type_id == 2);
                    DB.entities.guest_event.AddOrUpdate(gst_event);
                    DB.entities.SaveChanges();
                    MessageBox.Show("Посетитель вышёл");
                }
                else
                    MessageBox.Show("Посетитель уже вышёл");
            }
            catch
            {
                MessageBox.Show("Выберите посетителя");
            }
        }

        private void empList_Checked(object sender, RoutedEventArgs e)
        {
            eventLV.ItemsSource = DB.entities.events.ToList();
            fioSearch2.Text = null;
        }

        private void gstList_Checked(object sender, RoutedEventArgs e)
        {
            eventLV.ItemsSource = DB.entities.guest_event.ToList();
            fioSearch2.Text = null;
        }

        private void fioSearch2_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchGuest2();
        }
        void searchGuest2()
        {
            if(empList.IsChecked == true)
                eventLV.ItemsSource = DB.entities.events.Where(c => c.employeer.name.Contains(fioSearch2.Text) || c.employeer.surname.Contains(fioSearch2.Text) || c.employeer.patronomyc.Contains(fioSearch2.Text)).ToList();
            else
                eventLV.ItemsSource = DB.entities.guest_event.Where(c => c.guest.name.Contains(fioSearch2.Text) || c.guest.surname.Contains(fioSearch2.Text) || c.guest.patronomyc.Contains(fioSearch2.Text)).ToList();
        }

        private void accessAllowedBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DB.entities.events.FirstOrDefault(c => c.event_type.event_type_id == 1 && c.employeer.employeer_id == emp.employeer_id && c.date.Year == DateTime.Now.Year && c.date.Month == DateTime.Now.Month && c.date.Day == DateTime.Now.Day) == null)
            {
                @event evnt = new @event();
                evnt.employeer = emp;
                evnt.date = DateTime.Now;
                evnt.event_type = DB.entities.event_type.FirstOrDefault(c => c.event_type_id == 1);
                DB.entities.events.Add(evnt);
                DB.entities.SaveChanges();
                clearTB();
            }
            else if (DB.entities.events.FirstOrDefault(c => c.event_type.event_type_id == 2 && c.employeer.employeer_id == emp.employeer_id && c.date.Year == DateTime.Now.Year && c.date.Month == DateTime.Now.Month && c.date.Day == DateTime.Now.Day) == null)
            {
                @event evnt = new @event();
                evnt.employeer = emp;
                evnt.date = DateTime.Now;
                evnt.event_type = DB.entities.event_type.FirstOrDefault(c => c.event_type_id == 2);
                DB.entities.events.Add(evnt);
                DB.entities.SaveChanges();
                clearTB();
            }
            else
                MessageBox.Show("Сотрудник уже вышел");
            clearTB();
        }

        private void accessDeniedBtn_Click(object sender, RoutedEventArgs e)
        {
            clearTB();
        }
        public void clearTB()
        {
            File.Delete("C:/Users/andrey/Desktop/SKUD/ArduinoServer/bin/Debug/TestFile.txt");
            departmentTB.Text = null;
            nameTB.Text = null;
            uidTB.Text = null;
            emp_eventLV.ItemsSource = null;
            empPhoto.Source = null;
        }
    }
}
