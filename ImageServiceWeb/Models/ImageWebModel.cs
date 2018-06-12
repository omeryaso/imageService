using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using static ImageServiceWeb.Models.ConfigModel;

namespace ImageServiceWeb.Models
{
    public class ImageWebModel
    {
        private static Communication.IWebClient WebClient { get; set; }
        public event ChangeNotifyer ChangeNotifyer;
        private static ConfigModel ConfigModel;
        private static string m_outputDir;
        private const string stud = "~/App_Data/students.txt";

        /// <summary>
        /// ImageWebModel constructor.
        /// </summary>
        public ImageWebModel()
        {
            try
            {
                NumofPics = 0;
                WebClient = Communication.WebClient.Instance;
                IsConnected = WebClient.IsConnected;
                ConfigModel = new ConfigModel();
                ConfigModel.Notify += Notify;
                GetStudentsList();
            }
            catch (Exception e)
            {
                Console.WriteLine("ImageWebModel: " + e);
            }
        }

        /// <summary>
        /// Notify function.
        /// notifies controller about change.
        /// </summary>
        void Notify()
        {
            if (ConfigModel.OutputDir != "")
            {
                m_outputDir = ConfigModel.OutputDir;
                NumofPics = GetNumOfPics();
                ChangeNotifyer?.Invoke();
            }
        }

        /// <summary>
        /// GetNumOfPics function.
        /// </summary>
        /// <param name="outputDir">the pics output dir</param>
        /// <returns></returns>
        public static int GetNumOfPics()
        {
            try
            {
                if (m_outputDir == null || m_outputDir == "")
                {
                    return 0;
                }
                int counter = 0;
                while (m_outputDir == null && (counter < 2)) { System.Threading.Thread.Sleep(1000); counter++; }
                int sum = 0;
                DirectoryInfo di = new DirectoryInfo(m_outputDir);
                sum += di.GetFiles("*.PNG", SearchOption.AllDirectories).Length;
                sum += di.GetFiles("*.BMP", SearchOption.AllDirectories).Length;
                sum += di.GetFiles("*.JPG", SearchOption.AllDirectories).Length;
                sum += di.GetFiles("*.GIF", SearchOption.AllDirectories).Length;
                return sum / 2;
            }
            catch (Exception e)
            {
                Console.WriteLine("ImageWebModel: " + e);
                return 0;
            }
        }

        /// <summary>
        /// GetStudentsList.
        /// reads from a file the names of the students and their details
        /// </summary>
        public void GetStudentsList()
        {
            List<Student> studList = new List<Student>();
            try
            {
                StreamReader file = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(stud));
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(',');
                    studList.Add(new Student() { LastName = words[0], FirstName = words[1], ID = words[2], Group = words[3] });
                }
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("ImageWebModel -SetStudents: " + e);
            }
            Students = studList;
        }

        //members
        [Required]
        [Display(Name = "Is Connected")]
        public bool IsConnected { get; set; }

        [Required]
        [Display(Name = "Num of Pics")]
        public int NumofPics { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Students")]
        public List<Student> Students { get; set; }

        public class Student
        {
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "ID")]
            public string ID { get; set; }

            [Required]
            [Display(Name = "Group")]
            public string Group { get; set; }
        }
    }
}