using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using banetexam2.Models;
using System.Linq; 

namespace banetexam2.Factories
{
    public class IdeaFactory
    {
        private readonly IOptions<MySqlOptions> MySqlConfig;
        public IdeaFactory(IOptions<MySqlOptions> config)
        {
            MySqlConfig = config;
        }
        internal IDbConnection Connection {
            get {
                return new MySqlConnection(MySqlConfig.Value.ConnectionString);
            }
        }
        public void CreateIdea(Idea newIdea)
        {
            using(IDbConnection dbConnection = Connection)
            {
                string query = "INSERT INTO ideas (Content, CreatedBy, CreatedAt, UpdatedAt) VALUES (@Content, @CreatedBy, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, newIdea);
            }
        }
        public void CreateLike(Like newLike)
        {
            using(IDbConnection dbConnection = Connection)
            {
                string query = "INSERT INTO Likes (Liker, Liked, CreatedAt, UpdatedAt) VALUES (@Liker, @Liked, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, newLike);
            }
        }
        public Idea GetIdeaById(int id){
            using (IDbConnection dbConnection = Connection){
                dbConnection.Open();
                Idea idea = dbConnection.Query<Idea>($"SELECT * FROM ideas WHERE (id={id})").SingleOrDefault();
                idea.CreatedByObj = dbConnection.Query<User>($"SELECT * FROM users WHERE (Id = {idea.CreatedBy})").SingleOrDefault();
                idea.Likes = dbConnection.Query<Like>($"SELECT * FROM likes WHERE Liked = {idea.Id}").ToList();
                foreach (var like in idea.Likes){
                        like.LikerObj = dbConnection.Query<User>($"SELECT * FROM users WHERE (Id = {like.Liker})").SingleOrDefault();
                    }
                return idea;
            }
        }
        public List<Idea> GetAllIdeas(){
            using (IDbConnection dbConnection = Connection){
                dbConnection.Open();
                var ideas = dbConnection.Query<Idea>("SELECT * FROM ideas").ToList();
                foreach (var idea in ideas){
                    idea.CreatedByObj = dbConnection.Query<User>($"SELECT * FROM users WHERE (Id = {idea.CreatedBy})").SingleOrDefault();
                    idea.Likes = dbConnection.Query<Like>($"SELECT * FROM likes WHERE Liked = {idea.Id}").ToList();
                    foreach (var like in idea.Likes){
                        like.LikerObj = dbConnection.Query<User>($"SELECT * FROM users WHERE (Id = {like.Liker})").SingleOrDefault();
                    }
                }
                List<Idea> sortedIdeas = ideas.OrderByDescending(x=>x.Likes.Count).ToList();
                return sortedIdeas;
            }
        }
        public void DeleteIdeaById(int id){
            using (IDbConnection dbConnection = Connection){
                dbConnection.Open();
                dbConnection.Execute($"DELETE FROM likes WHERE Liked = {id}");
                dbConnection.Execute($"DELETE FROM ideas WHERE Id = {id}");
                return;
            }
        }
        public List<Like> GetSpecificLike(int Liker, int Liked){
            using (IDbConnection dbConnection = Connection){
                dbConnection.Open();
                var like = dbConnection.Query<Like>($"SELECT * FROM likes WHERE (Liker = {Liker} AND Liked = {Liked})").ToList();
                return like;
            }
        }
    }

}