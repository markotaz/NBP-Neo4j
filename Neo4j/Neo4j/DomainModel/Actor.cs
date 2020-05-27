using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.DomainModel
{
    public class Actor
    {
        public String Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Birthdate { get; set; }

        public Actor(String name, String lName, String date)
        {
            this.FirstName = name;
            this.LastName = lName;
            this.Birthdate = date;
        }

        public static Actor ParseActor(IResult result)
        {
            List<IRecord> list = result.ToList();
            if (list.Count == 1)
            {
                INode inode = list[0][0] as INode;
                IReadOnlyDictionary<string, object> row = inode.Properties;
                Actor actor = new Actor(row["firstName"].ToString(), row["lastName"].ToString(),row["birthdate"].ToString());
                actor.Id = inode.Id.ToString();
                return actor;
            }
            else
                return null;
        }
    }
}
