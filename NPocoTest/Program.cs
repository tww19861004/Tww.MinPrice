﻿using NPoco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tww.MinPrice.Models;

namespace NPocoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            String fileName = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "tww1.db");
            // create a database "context" object t
            String connectionString = @"Data Source="+ fileName + ";Version=3;";

            using (IDatabase db = new Database(connectionString, NPoco.DatabaseType.SQLite))
            {
                List<User> users = db.Fetch<User>("select * from user");

                User u = new User();
                //u.Id = i;
                u.Name = "1";
                u.Phone = "15062437243";
                u.Email = "382233701@qq.com";
                u.Password = "1234";
                u.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
            }

            QueryAsync();
        }

        public static async Task<List<User>> QueryAsync()
        {
            return await Task.Run<List<User>>(()=>
            {
                String fileName = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "tww1.db");
                // create a database "context" object t
                String connectionString = @"Data Source=" + fileName + ";Version=3;";

                using (IDatabase db = new Database(connectionString, NPoco.DatabaseType.SQLite))
                {
                    return Task<List<User>>.Run(() => { return db.Query<User>().Where(x => x.Id == 1).ToListAsync(); });
                }                
            });           
        }
    }
}
