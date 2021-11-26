using LeThanhDat_T2009M1.Data;
using LeThanhDat_T2009M1.Entities;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeThanhDat_T2009M1.Model
{
    public class ContactModel
    {
        private static string _selectStatementWithConditionTemplate = "SELECT * FROM contacts WHERE PhoneNumber Like @keyword";
        public ContactModel()
        {
            DatabaseMigration.UpdateDatabase();
        }
        public bool Save(Contact contact)
        {
            try
            {
                using (SqliteConnection cnn = new SqliteConnection($"Filename={DatabaseMigration._databasePath}"))
                {
                    cnn.Open();
                    SqliteCommand command = new SqliteCommand("INSERT INTO contacts (PhoneNumber, Name)" +
                " values (@phoneNumber, @name)", cnn);
                    command.Parameters.AddWithValue("@phoneNumber", contact.PhoneNumber);
                    command.Parameters.AddWithValue("@name", contact.Name);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }



        public List<Contact> FindAll()
        {
            List<Contact> result = new List<Contact>();
            try
            {
                using (SqliteConnection cnn = new SqliteConnection($"Filename={DatabaseMigration._databasePath}"))
                {
                    cnn.Open();
                    SqliteCommand command = new SqliteCommand("SELECT * FROM contacts", cnn);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var phoneNumber = reader.GetString(0);
                        var name = reader.GetString(1);
                        var contact = new Contact()
                        {
                            PhoneNumber = phoneNumber,
                            Name = name
                        };
                        result.Add(contact);
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return result;
        }


        public List<Contact> SearchByKeyword(string keyword)
        {
            List<Contact> contacts = new List<Contact>();
            try
            {
                //
                using (SqliteConnection cnn = new SqliteConnection($"Filename={DatabaseMigration._databasePath}"))
                {
                    cnn.Open();
                    //Tạo câu lệnh
                    SqliteCommand cmd = new SqliteCommand(_selectStatementWithConditionTemplate, cnn);
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    //Bắn lệnh vào và lấy dữ liệu.
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var phoneNumber = Convert.ToString(reader["PhoneNumber"]);
                        var name = Convert.ToString(reader["Name"]);
                        var contact = new Contact()
                        {
                            PhoneNumber = phoneNumber,
                            Name = name
                        };

                        contacts.Add(contact);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return contacts;
        }
    }
}
}
