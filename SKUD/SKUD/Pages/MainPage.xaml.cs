using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using SKUD.DataBase;
using System.Windows.Controls;
using SKUD.Excel;

namespace SKUD.Pages
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            empLV.ItemsSource = DB.entities.employeers.ToList();
            empGuesLVFilling();
        }

        private void empLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            employeer employeer = (employeer)empLV.SelectedItem;
            fioTB.Text = employeer.surname + ' ' + employeer.name + ' ' + employeer.patronomyc;
            idTB.Text = Convert.ToString(employeer.employeer_id);
            departmentCB.SelectedIndex = employeer.department.department_id - 1;
            postTB.SelectedIndex = employeer.post.post_id - 1;
        }

        private void excelBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ExcelDoc.createTable();
        }

        public void empGuesLVFilling()
        {
            if (empRB.IsChecked == true)
            {
                List<@event> todayEvent_list = DB.entities.events.Where(c => c.date.Year == DateTime.Now.Year && c.date.Month == DateTime.Now.Month && c.date.Day == DateTime.Now.Day).ToList();

                foreach (@event evnt1 in todayEvent_list.ToArray())
                {
                    if (evnt1.event_type.event_type_id == 2)
                    {
                        foreach (@event evnt2 in todayEvent_list.ToArray())
                        {
                            if (evnt2.employeer == evnt1.employeer)
                            {
                                todayEvent_list.Remove(evnt1);
                                todayEvent_list.Remove(evnt2);
                            }
                        }
                    }
                }
                actualEmpGuestLV.ItemsSource = todayEvent_list;
            }
            else
            {
                List<guest_event> todayEvent_list = DB.entities.guest_event.Where(c => c.date.Year == DateTime.Now.Year && c.date.Month == DateTime.Now.Month && c.date.Day == DateTime.Now.Day).ToList();

                foreach (guest_event evnt1 in todayEvent_list.ToArray())
                {
                    if (evnt1.event_type.event_type_id == 2)
                    {
                        foreach (guest_event evnt2 in todayEvent_list.ToArray())
                        {
                            if (evnt2.guest == evnt1.guest)
                            {
                                todayEvent_list.Remove(evnt1);
                                todayEvent_list.Remove(evnt2);
                            }
                        }
                    }
                }
                actualEmpGuestLV.ItemsSource = todayEvent_list;
            }
        }

        private void empRB_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            empGuesLVFilling();
        }

        private void guestRB_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            empGuesLVFilling();
        }
    }
}
