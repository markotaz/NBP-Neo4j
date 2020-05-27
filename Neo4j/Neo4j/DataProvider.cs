using Neo4j.DomainModel;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Neo4j.DomainModel;
using System.Windows.Forms;

namespace Neo4j
{
    public class DataProvider : IDisposable
    {
        public static IDriver Driver { get; set; }
        private static DataProvider _instance;
        private static readonly object _lock = new object();
        public FormLogin MainForm { get; set; }
        public User User { get; set; }
        private DataProvider()
        {
            Driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "nbp"));
        }
        public static DataProvider Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        return _instance = new DataProvider();
                    }
                    else
                    {
                        return _instance;
                    }
                }
                
            }
        }
        public void Dispose()
        {
            Driver?.Dispose();
        }

        #region User
        public IResult CreateUser(ITransaction tx, User user)
        {
            String query = $"CREATE (u:User {{firstName: '{user.FirstName}', lastName: '{user.LastName}', phoneNumber: '{user.PhoneNumber}'," +
                $" email:'{user.Email}',password:'{user.Password}'}})";
            return tx.Run(query);
        }
       public IResult UpdateUser(ITransaction tx, User user, String email, String pass)
        {
            String query = $"match (u:User{{email:'{user.Email}', password:'{user.Password}'}}) set u.email='{email}', u.password='{pass}'";
            return tx.Run(query);
        }
        public User GetUser(ITransaction tx, String email)
        {
            var query = tx.Run($"match (u:User{{email:'{email}'}}) return u");
            return DomainModel.User.ParseUser(query);
        }
        public IResult DeleteUser(ITransaction tx, User user)
        {
            String query = $"match (u:User {{firstName:'{user.FirstName}', lastName:'{user.LastName}', email:'{user.Email}', password:'{user.Password}'}})" +
                $"detach delete u";
            return tx.Run(query);
        }
        #endregion

        #region Movie
        public IResult CreateMovie(ITransaction tx, Movie movie)
        {
            String query = $"create (m:Movie {{title:'{movie.Title}', releaseYear:'{movie.ReleaseYear}', imdbRate:'{movie.ImdbRate}'}})";
            return tx.Run(query);
        }
        public IResult UpdateMovie(ITransaction tx, Movie movie, String newImdbRate)
        {
            String query = $"match (m:Movie {{title:'{movie.Title}', releaseYear:'{movie.ReleaseYear}'}}) set m.imdbRate='{newImdbRate}'";
            return tx.Run(query);
        }
        public Movie GetMovie(ITransaction tx, String title)
        {
            var query = tx.Run($"match(m:Movie {{title:'{title}'}}) return m");
            return DomainModel.Movie.ParseMovie(query);
        }
        public IResult DeleteMovie(ITransaction tx, Movie m)
        {
            String query = $"match (m:Movie {{title:'{m.Title}', releaseYear:'{m.ReleaseYear}', imdbRate:'{m.ImdbRate}'}}) detach delete m";
            return tx.Run(query);
        }
        #endregion

        #region Actor
        public IResult CreateActor(ITransaction tx, Actor actor)
        {
            String query = $"create (a:Actor{{firstName:'{actor.FirstName}', lastName: '{actor.LastName}', birthdate:'{actor.Birthdate}'}})";
            return tx.Run(query);
        }
        public IResult DeleteActor(ITransaction tx, String fName, String lName)
        {
            String query = $"match (a:Actor{{firstName:'{fName}', lastName: '{lName}'}}) detach delete a";
            return tx.Run(query);
        }
        public Actor GetActor(ITransaction tx, String fName, String lName)
        {
            var query = tx.Run($"match (a:Actor{{firstName:'{fName}', lastName: '{lName}'}}) return a");
            return DomainModel.Actor.ParseActor(query);
        }
        #endregion

        #region VideoClub
        public IResult CreateVideoClub(ITransaction tx, VideoClub vc)
        {
            String query = $"create (vc:VideoClub{{name:'{vc.Name}', city:'{vc.City}', adress:'{vc.Adress}', " +
                $"email:'{vc.Email}', phoneNumber:'{vc.PhoneNumber}'}})";
            return tx.Run(query);
        }
        public IResult UpdateVideoClub(ITransaction tx, String name, String adress, String email, String phoneNum)
        {
            String query = $"match (vc:VideoClub {{name:'{name}'}}) set vc.adress='{adress}', vc.email='{email}', vc.phoneNumber='{phoneNum}'";
            return tx.Run(query);
        }
        public IResult DeleteVideoClub(ITransaction tx, String name)
        {
            String query = $"match (vc:VideoClub {{name:'{name}'}}) detach delete vc";
            return tx.Run(query);
        }
        public List<Movie> GetMoviesFromVideoClub(ITransaction tx, String vcName)
        {
            var query = tx.Run($"match(m: Movie) < -[r: contains] - (vc: VideoClub) where vc.name = '{vcName}' and r.rented='no' return m");
            return DomainModel.Movie.ParseMovies(query);
        }
        public VideoClub GetVideoClub(ITransaction tx, String vcName)
        {
            var query = tx.Run($"match(vc: VideoClub) where vc.name = '{vcName}' return vc");
            return DomainModel.VideoClub.ParseVideoClub(query);
        }
        #endregion

        #region Relations
        public bool AddFavouriteMovie(ITransaction tx, User user, String movieTitle, String favourite)
        {
            try
            {
                String query = $"match (u:User),(m:Movie) WHERE u.email='{user.Email}' AND m.title='{movieTitle}'" +
                    $" MERGE (u)-[relation:watched {{favourite: '{favourite}'}}]-(m)";
                tx.Run(query);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
        public bool AddMovieToVideoClub(ITransaction tx, String vcName, String movieTitle)
        {
            try
            {
                String query = $"match (vc:VideoClub),(m:Movie) WHERE vc.name='{vcName}' AND m.title='{movieTitle}'" +
                    $" MERGE (vc)-[relation:contains {{rented: 'no'}}]-(m)";
                tx.Run(query);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
        public bool JoinVideoClub(ITransaction tx, User user, String vcName)
        {
            try
            {
                String query = $"match (u:User),(vc:VideoClub) where u.email='{user.Email}' AND vc.name='{vcName}' MERGE (u)-[relation:memberOf]-(vc)";
                tx.Run(query);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
        public bool AddFavouriteActor(ITransaction tx, User user, String fName, String lName)
        {
            try
            {
                String query = $"match (u:User),(a:Actor) where u.email='{user.Email}' AND a.firstName='{fName}' AND a.lastName='{lName}' MERGE (u)-[relation:favourite]-(a)";
                tx.Run(query);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
        public bool AddMovieGenre(ITransaction tx, String genreName, String movieTitle)
        {
            try
            {
                String query = $"match (m:Movie),(g:Genre) where m.title='{movieTitle}' AND g.name='{genreName}' MERGE (m)-[relation:from]-(g)";
                tx.Run(query);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
        public bool AddActorToMovie(ITransaction tx, String fName, String lName, String movieTitle)
        {
            try
            {
                String query = $"match (m:Movie),(a:Actor) where m.title='{movieTitle}' AND a.firstName='{fName}' AND a.lastName='{lName}' MERGE (a)-[relation:actsIn]-(m)";
                tx.Run(query);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
        public bool RentMovie(ITransaction tx, String vcName, String movieTitle)
        {
            try
            {
                String query = $"match (m:Movie)<-[r:contains]-(vc:VideoClub) where m.title='{movieTitle}' AND vc.name='{vcName}' AND r.rented='no' SET r.rented='yes'";
                tx.Run(query);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
        public bool RentRelation(ITransaction tx, User user,String movieTitle)
        {
            try
            {
                String query = $"match (m:Movie),(u:User) where m.title='{movieTitle}' AND u.email='{user.Email}' MERGE (m)<-[relation:rented]-(u)";
                tx.Run(query);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
       public IResult GetRecommendation(ITransaction tx, User user)
        {
           // String query = $"match (g:Genre)<-[:from]-(m:Movie)<-[:watched]-(u:User)-[omi:favourite]->(a:Actor)-[gl:actsIn]->(rec:Movie)-[:from]->(g) " +
           //     $" where rec.imdbRate > 7 and m <>rec and not exists((rec)<-[:rented]-(u)) and u.email='{user.Email}' return rec";
            String query = $"match (m:Movie)-[:watched{{favourite: 'yes'}}]-(u:User{{email:'{user.Email}'}}), " +
                $"(s:Movie) with min(m.imdbRate) as minOcena, s ,u where not Exists ((u)-[:watched]-(s)) " +
                $"and not Exists ((u)-[:rented]-(s)) and s.imdbRate > minOcena return s";
            return tx.Run(query);
        }
        #endregion

        #region Genre
        public IResult CreateGenre(ITransaction tx, String name)
        {
            String query = $"create (g:Genre {{name:'{name}'}})";
            return tx.Run(query);
        }
        public Genre GetGenre(ITransaction tx, String name)
        {
            var query = tx.Run($"match (g:Genre {{name:'{name}'}}) return g");
            return DomainModel.Genre.ParseGenre(query);
        }
        #endregion
    }
}
