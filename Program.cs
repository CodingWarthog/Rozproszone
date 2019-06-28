
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace Insertocepcja
{
    class Program
    {
        string connectionString = "Server=DESKTOP-ENN90IS\\SQLEXPRESS;Database=Rozproszone_v1;Trusted_Connection=True;";

        List<HumanBeing> ppl = new List<HumanBeing>();
        List<string> cityList = new List<string>();
        List<string> departmentNameList = new List<string>();
        List<PaymentDTO> paymentList = new List<PaymentDTO>();
        List<WorkBonus> workBonusList = new List<WorkBonus>();

        public Program()
        {
            Console.WriteLine("\nStart");

            //wczytuje dane
            for (int i = 1; i < 11; i++)
            {
                string csv = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Mocks\Employee" + i + ".csv");
                this.readEmployeeCSV(csv);
            }

            string cityPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Mocks\CityDATA.csv");
            string departmentPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Mocks\DepartmentDATA.csv");
            string paymentPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Mocks\PaymentDATA.csv");
            string workBonusPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Mocks\WorkBonusDATA.csv");
            
            string filePathEmployee = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Mocks\InsertEmployee.txt");
            string filePathWBA= Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Mocks\InsertWBA.txt");
            string mongoJSONPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Mocks\MongoJSON.json");

            this.readCityCSV(cityPath);
            this.readDepartmentCSV(departmentPath);
            this.readPaymentCSV(paymentPath);
            this.readWorkBonusCSV(workBonusPath);

            //usuwa pliki
           if (File.Exists(filePathEmployee))
            {
                File.Delete(filePathEmployee);
            }

            if (File.Exists(filePathWBA))
            {
                File.Delete(filePathWBA);
            }


           Console.WriteLine("\nZaczynam inserty do Employee");
            for (int i = 0; i < 100000; i++)
            {
                insertEmployeeToDB(filePathEmployee);
            }
            Console.WriteLine("\nSkończyłem dodawać do tabeli Employee");

            Console.WriteLine("\nZaczynam inserty do WBA");
            for (int i = 0; i < 170000; i++)
            {
                insertWBAToDB(filePathWBA);
            }

            ////////////////////////////////////////////////////////////////////
            //MONGODB
            
            Console.WriteLine("\n Zapisuje json'a");
            //usuwa plik json
            if (File.Exists(mongoJSONPath))
            {
                File.Delete(mongoJSONPath);
            }

            for(int i = 0; i < 100000; i++)
            {
                writeJSONToFile(mongoJSONPath);
            }     

            Console.WriteLine("\nKoooonieeeec");
        }

        private void insertWBAToDB(string filePathWBA)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string line = createWBALine();
                connection.Open();
                var cmnd = new SqlCommand(line, connection);
                cmnd.ExecuteNonQuery();
                connection.Close();

                using (StreamWriter file = new StreamWriter(filePathWBA, true))
                {
                    file.WriteLine(line);
                }
            }
        }

        private void insertEmployeeToDB(string filePath)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string line = createEmployeeLine();
                connection.Open();
                var cmnd = new SqlCommand(line, connection);
                cmnd.ExecuteNonQuery();
                connection.Close();

                using (StreamWriter file = new StreamWriter(filePath, true))
                {
                    file.WriteLine(line);
                }
            }
        }

        private string createWBALine()
        {
            //r.next(a,b) losuje z przedzialu <a,b)
            Random r = new Random();

            int randomYear = r.Next(1998,2019); 
            int randomMonth = r.Next(1, 13);
            int randomDay = r.Next(1,29);
            string randomDate = "'" + randomYear + "-" + randomMonth + "-" + randomDay + "'";

            //w bazie employee sa od id=2
            int randomEmployeeId = r.Next(2, 100001);
            int randomWorkBonusId = r.Next(1, 20);

            string insertLine = "INSERT INTO[dbo].[WorkBonusAssignment] ([AssignmentDate], [EmployeeId], [WorkBonusId]) VALUES( " +
                    randomDate + ", " + randomEmployeeId + ", " + randomWorkBonusId + ")";

            return insertLine;
        }

            private string createEmployeeLine()
            {
                //department 1-250
                //payment 1-80
                Random r = new Random();

                string randomFirstName = ppl[ r.Next(1, 10000) ].firstName;
                string randomLastName = ppl[ r.Next(1, 10000) ].lastName;
                int randomAge = r.Next(18, 56);
                int randomDepartment = r.Next(1, 251);
                int randomPayment = r.Next(1, 81);


                string insertLine = "INSERT INTO [dbo].[Employee] ([Firstname], [Lastname], [Age], [DepartmentId]," +
                    " [PaymentId]) VALUES ('" + randomFirstName + "', '" +
                        randomLastName + "', " + randomAge +
                        " , " + randomDepartment + " , " + randomPayment + " )";
            
                return insertLine;
            }
        private void readEmployeeCSV(string path)
        {
            StreamReader sr = new StreamReader(path);

            String line;

            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(',');

                HumanBeing hmn = new HumanBeing(parts[0], parts[1]);
                ppl.Add(hmn);
            }
        }

        private void readCityCSV(string path)
        {
            StreamReader sr = new StreamReader(path);

            String line;

            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                cityList.Add(parts[0]);
            }
        }


        private void readDepartmentCSV(string path)
        {
            StreamReader sr = new StreamReader(path);

            String line;

            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                departmentNameList.Add(parts[0]);
            }
        }

        private void readPaymentCSV(string path)
        {
            StreamReader sr = new StreamReader(path);

            String line;

            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(',');

                PaymentDTO pDTO = new PaymentDTO(Int32.Parse(parts[1]), parts[0]);
                paymentList.Add(pDTO);
            }
        }

        private void readWorkBonusCSV(string path)
        {
            StreamReader sr = new StreamReader(path);

            String line;

            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(',');

                WorkBonus wb = new WorkBonus(Int32.Parse(parts[1]), parts[0]);
                workBonusList.Add(wb);
            }
        }
        
        private string createOneEmployeeJSON()
        {
            //department 1-250
            //payment 1-80    
            Random r = new Random();

            string randomFirstName = ppl[r.Next(1, 10000)].firstName;
            string randomLastName = ppl[r.Next(1, 10000)].lastName;
            int randomAge = r.Next(18, 56);
            int randomDepartmentID = r.Next(1, 250);
            string randomDepartment = departmentNameList[ randomDepartmentID ];
            string randomCity = departmentNameList[ randomDepartmentID ];
            int randomPaymentID = r.Next(1, 80);
            string speciality = paymentList[randomPaymentID].speciality;
            int monthRate = paymentList[randomPaymentID].amount;

            int bonusesNumber = howManyBonuses();

            string bonusAssign = "";

            if (bonusesNumber != 0)
            {
                for (int i = 1; i <= bonusesNumber; i++)
                {
                    bonusAssign = bonusAssign + createBonusAssignForEmployeeJSON();

                    if (i != bonusesNumber)
                    {
                        bonusAssign = bonusAssign + ",";
                    }
                }
            }
            

            string oneEmployeeJSON = "" +

                "{ " +
                    "FirstName: '" + randomFirstName + "'," +
                    "LastName: '" + randomLastName + "'," +
                    "Age: " + randomAge + "," +
                    "DepartmentName: '" + randomDepartment + "'," +
                    "DepartmentCity: '" + randomCity + "'," +
                    "Speciality: '" + speciality + "'," +
                    "MonthRate: " + monthRate + "," +
                    "BonusAssign:[ " + bonusAssign +
                                "]" +
                "},";

            return oneEmployeeJSON;
        }

        private int howManyBonuses()
        {
            Random r = new Random();
            int x = r.Next(1,101);
            int howManyBonuses = 0;
            // 20%-0  50%-1  20%-2 10%-3
            if(x>0 && x <= 50)
            {
                howManyBonuses = 1;
            }else if(x>50 && x <= 70)
            {
                howManyBonuses = 2;
            }else if(x>70 && x <= 80)
            {
                howManyBonuses = 3;
            }

            return howManyBonuses;
        }

        private string createBonusAssignForEmployeeJSON()
        {
            Random r = new Random();
   
            int randomYear = r.Next(1998, 2019);
            int randomMonth = r.Next(1, 13);
            int randomDay = r.Next(1, 29);
            string assignmentDate = "'" + randomYear + "-" + randomMonth + "-" + randomDay + "'";
            int randomWorkBonusId = r.Next(1, 19);
            string bonusName = workBonusList[randomWorkBonusId].bonusName;
            int amount = workBonusList[randomWorkBonusId].amount;

            string oneBonusAssign = "" +
                "{" +
                    "AssignmentDate: " + assignmentDate + "," +
                    "BonusName: '" + bonusName + "'," +
                    "Amount: " + amount +
                "}";

            return oneBonusAssign;
        }

        private void writeJSONToFile(string filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath, true))
            {
                file.WriteLine(createOneEmployeeJSON());
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();
        }
    }
}
