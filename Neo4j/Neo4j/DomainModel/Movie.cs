using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.DomainModel
{
    public class Movie
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public String ReleaseYear { get; set; }
        public String ImdbRate { get; set; }

        public Movie(String title, String year, String rate)
        {
            this.Title = title;
            this.ReleaseYear = year;
            this.ImdbRate = rate;
        }

        public static Movie ParseMovie(IResult result)
        {
            List<IRecord> list = result.ToList();
            if (list.Count == 1)
            {
                INode inode = list[0][0] as INode;
                IReadOnlyDictionary<string, object> row = inode.Properties;
                Movie m = new Movie(row["title"].ToString(), row["releaseYear"].ToString(), row["imdbRate"].ToString());
                m.Id = inode.Id.ToString();
                return m;
            }
            else
                return null;
        }
        public static List<Movie> ParseMovies(IResult result)
        {
            List<IRecord> list = result.ToList();
            List<Movie> movies = new List<Movie>();
            Movie m = null;
            foreach(IRecord rec in list)
            {
                INode inode = rec[0] as INode;
                IReadOnlyDictionary<string, object> row = inode.Properties;
                m = new Movie(row["title"].ToString(), row["releaseYear"].ToString(), row["imdbRate"].ToString());
                m.Id = inode.Id.ToString();
                movies.Add(m);
            }
            return movies;
        }
    }
}
