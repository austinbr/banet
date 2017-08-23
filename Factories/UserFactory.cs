using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using System.Linq; 
using banetexam2.Models;

namespace banetexam2.Factories
{
    public class UserFactory
    {
        private readonly IOptions<MySqlOptions> MySqlConfig;
 
        public UserFactory(IOptions<MySqlOptions> config)
        {
            MySqlConfig = config;
        }
        internal IDbConnection Connection {
            get {
                return new MySqlConnection(MySqlConfig.Value.ConnectionString);
            }
        }
        public void Add(User item)
        {
            using (IDbConnection dbConnection = Connection) {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO users (Name, Username, Email, Password, CreatedAt, UpdatedAt) VALUES (@Name, @Username, @Email, @Password, NOW(), NOW())", item);
            }
        }
        public IEnumerable<User> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                List<User> users = dbConnection.Query<User>("SELECT * FROM users").ToList();
                foreach(var user in users)
                {
                    user.Likes = dbConnection.Query<Like>($"SELECT * FROM likes WHERE Liker = {user.Id}").ToList();
                    user.Ideas = dbConnection.Query<Idea>($"SELECT * FROM ideas WHERE CreatedBy = {user.Id}").ToList();
                }
                return users;
            }
        }
        public User FindById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                User user = dbConnection.Query<User>($"SELECT * FROM users WHERE (Id={id})").FirstOrDefault();
                user.Likes = dbConnection.Query<Like>($"SELECT * FROM likes WHERE Liker = {user.Id}").ToList();
                user.Ideas = dbConnection.Query<Idea>($"SELECT * FROM ideas WHERE CreatedBy = {user.Id}").ToList();
                return user;
            }
        }
        public User FindByUsername(string username)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM users WHERE Username = @Username", new { Username = username }).FirstOrDefault();
            }
        }
        public User FindByEmail(string email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM users WHERE Email = @Email", new { Email = email }).FirstOrDefault();
            }
        }
    }
}