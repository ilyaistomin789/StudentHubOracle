using System;
using System.Windows;
using StudentHub.University;

namespace StudentHub
{
    /// <summary>
    /// Логика взаимодействия для ShowInformationWindow.xaml
    /// </summary>
    public partial class ShowInformationWindow : Window
    {
        public ShowInformationWindow(Student student)
        {
            InitializeComponent();
            s_fioTextBlock.Text = student.Name;
            s_facultyTextBlock.Text = student.Faculty;
            s_specializationTextBlock.Text = student.Specialization;
            s_courseTextBlock.Text = student.Course.ToString();
            s_groupTextBlock.Text = student.Group.ToString();
            s_birthdayTextBlock.Text = DateTime.Parse(student.Birthday).ToString("D");
            s_emailTextBlock.Text = student.Email;
        }
    }
}
