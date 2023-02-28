using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace s20_project
{
    /// <summary>
    /// Interaction logic for AddCandidate.xaml
    /// </summary>
    public partial class AddCandidate : Window
    {
        public Contest Contest;
        public ListBox ListBoxCandidates;

        public AddCandidate(Contest contest, ListBox listBox)
        {
            InitializeComponent();
            Contest = contest;
            ListBoxCandidates = listBox;
        }


        private void Btn_AddOkay(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("You said: " + " Btn_Add_Candidate2: " + newName);
            AddTheName();
        }

        private void AddTheName()
        {
            try
            {
                string newName = AddName.Text;
                if (newName == "")
                {
                    MessageBox.Show("Name must not be blank");
                }
                else
                {
                    Contest.AddCandidate(new Candidate(newName));
                    ListBoxCandidates.Items.Refresh();
                    // this.Close();
                    AddName.Text = "";
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("error:" + " add-okay  " + exc.Message);
            }
        }



        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            // IsCancel="true" 
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddName.Focus();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                // textBlock1.Text = "You Entered: " + textBox1.Text;
                AddTheName();
            }
        }
    }
}
