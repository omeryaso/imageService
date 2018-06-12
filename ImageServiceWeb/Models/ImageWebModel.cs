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
        private static string outputDir;
        private const string stud = "~/App_Data/students.txt";
 
        /// <summary>
        /// ImageWebModel constructor.
        /// </summary>
        public ImageWebModel()
        {
            try
            {
                PicsNum = 0;
                GetStudentsList();
                WebClient = Communication.WebClient.Instance;
                IsConnected = WebClient.IsConnected;
                ConfigModel = new ConfigModel();
                ConfigModel.Notify += Notify;
            }
            catch (Exception e)
            {
                Console.WriteLine("ImageWebModel: " + e);
            }
        }

        /// <summary>
        /// when a change happens it will called to notify.
        /// </summary>
        void Notify()
        {
            if (ConfigModel.OutputDir != "")
            {
                PicsNum = GetPicsNum();
                outputDir = ConfigModel.OutputDir;
                ChangeNotifyer?.Invoke();
            }
        }

        /// <summary>
        /// GetPicsNum function.
        /// </summary>
        /// <returns>the nmber of pictures</returns>
        public static int GetPicsNum()
        {
            try
            {
                int counter = 0;
                if (outputDir != null && outputDir != "")
                {
                    while (outputDir == null && (counter < 2)) { System.Threading.Thread.Sleep(1000); counter++; }
                    DirectoryInfo info = new DirectoryInfo(outputDir);
                    return PicCounter(info) / 2;
                }
                return counter;
            }
            catch (Exception e)
            {
                Console.WriteLine("ImageWebModel - GetPicsNum: " + e);
                return 0;
            }
        }

        /// <summary>
        /// count the photos in a dirctory
        /// </summary>
        /// <param name="info"></param>
        /// <returns>number of photos found</returns>
        private static int PicCounter(DirectoryInfo info)
        {
            int count = 0;
            count += info.GetFiles("*.PNG", SearchOption.AllDirectories).Length;
            count += info.GetFiles("*.BMP", SearchOption.AllDirectories).Length;
            count += info.GetFiles("*.JPG", SearchOption.AllDirectories).Length;
            count += info.GetFiles("*.GIF", SearchOption.AllDirectories).Length;
            return count;
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
        public int PicsNum { get; set; }

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