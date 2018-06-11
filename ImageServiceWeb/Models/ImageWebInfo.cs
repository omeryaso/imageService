using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using static ImageServiceWeb.Models.Config;

namespace ImageServiceWeb.Models
{
    public class ImageWebInfo
    {
        private static Communication.IImageServiceClient GuiClient { get; set; }
        public event NotifyAboutChange NotifyEvent;
        private static Config m_config;
        private static string m_outputDir;

        /// <summary>
        /// ImageWebInfo constructor.
        /// initialize new ImageWebInfo obj.
        /// </summary>
        public ImageWebInfo()
        {
            try
            {
                GuiClient = Communication.ImageServiceClient.Instance;
                IsConnected = GuiClient.IsConnected;
                NumofPics = 0;
                m_config = new Config();
                m_config.Notify += Notify;
                Students = GetStudents();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Notify function.
        /// notifies controller about change.
        /// </summary>
        void Notify()
        {
            if (m_config.OutputDirectory != "")
            {
                m_outputDir = m_config.OutputDirectory;
                NumofPics = GetNumOfPics();
                NotifyEvent?.Invoke();
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
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// GetStudents function.
        /// gets student details.
        /// </summary>
        /// <returns></returns>
        public static List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            try
            {
                StreamReader file = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/StudentsDetails.txt"));
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    string[] param = line.Split(',');
                    students.Add(new Student() { FirstName = param[0], LastName = param[1], ID = param[2] });
                }
                file.Close();
            }
            catch (Exception ex)
            {

            }
            return students;
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
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "ID")]
            public string ID { get; set; }
        }
    }
}