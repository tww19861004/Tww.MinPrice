﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;
using System.IO;
using Tww.MinPrice.Models;
using System.Threading;

namespace Tww.MinPrice.Services
{
    public  class UserService
    {
        public readonly static string ConnectionString = @"Data Source=" + Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "tww1.db") + ";Version=3;";
        //插入等操作建议用 using
        public readonly static IDatabase DataBase = new Database(ConnectionString, NPoco.DatabaseType.SQLite);
        public static List<User> GetAllUsers()
        {          
            using (IDatabase db = new Database(ConnectionString, NPoco.DatabaseType.SQLite))
            {
                return db.Query<User>().ToList();
            }            
        }

        public static async Task<List<User>> GetAllUsersAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();            
            //并发量比较大的时候推荐用 using
            using (IDatabase db = new Database(ConnectionString, NPoco.DatabaseType.SQLite))
            {
                //return Task<List<User>>.Run(() => { return db.Query<User>().ToList(); });
                var res = await DataBase.Query<User>().ToListAsync();
                return res;
            };
        }

        public static async Task<User> GetUserByIdAsync(CancellationToken ct,int id)
        {
            using (IDatabase db = new Database(ConnectionString, NPoco.DatabaseType.SQLite))
            {                
                var user = await db.Query<User>().Where(x => x.Id == id).SingleAsync();
                return user;
            }                    
        }

        public static async Task AddAsync(CancellationToken ct, User newUser)
        {
            ct.ThrowIfCancellationRequested();
            if (newUser.Id > 0)
                throw new InvalidDataException("wrong user's id to add");
            using (IDatabase DatabaseNew = new Database(ConnectionString, NPoco.DatabaseType.SQLite))
            {
                await DatabaseNew.InsertAsync(newUser);
            }
        }

        public static async Task<int> UpdateAsync(CancellationToken ct, User user)
        {
            ct.ThrowIfCancellationRequested();
            if (user.Id <= 0)
                throw new InvalidDataException("wrong user's id to update");
            using (IDatabase DatabaseNew = new Database(ConnectionString, NPoco.DatabaseType.SQLite))
            {
                int res = await DatabaseNew.UpdateAsync(user);
                return res;
            }
        }

        public static async Task<int> DeleteAsync(CancellationToken ct, User user)
        {
            ct.ThrowIfCancellationRequested();
            if (user.Id <= 0)
                throw new InvalidDataException("wrong user's id to delete");
            using (IDatabase DatabaseNew = new Database(ConnectionString, NPoco.DatabaseType.SQLite))
            {
                int res = await DatabaseNew.DeleteAsync(user);
                return res;
            }
        }
    }
}
