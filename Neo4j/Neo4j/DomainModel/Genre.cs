using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neo4j.DomainModel
{
    public class Genre
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public Genre(String name)
        {
            this.Name = name;
        }
        public static Genre ParseGenre(IResult result)
        {
            List<IRecord> list = result.ToList();
            if (list.Count == 1)
            {
                INode inode = list[0][0] as INode;
                IReadOnlyDictionary<string, object> row = inode.Properties;
                Genre genre = new Genre(row["name"].ToString());
                genre.Id = inode.Id.ToString();
                return genre;
            }
            else
                return null;

        }
    }
}
