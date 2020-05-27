using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.DomainModel
{
    public class VideoClub
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String City{get;set;}
        public String Adress { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public List<Movie> Movies { get; set; }

        public VideoClub()
        {
            Movies = new List<Movie>();
        }
        public VideoClub(String name, String city, String adress, String mail, String phoneNum)
        {
            this.Name = name;
            this.City = city;
            this.Adress = adress;
            this.Email = mail;
            this.PhoneNumber = phoneNum;
            Movies = new List<Movie>();
        }
        public static VideoClub ParseVideoClub(IResult result)
        {
            List<IRecord> list = result.ToList();
            if (list.Count == 1)
            {
                INode inode = list[0][0] as INode;
                IReadOnlyDictionary<string, object> row = inode.Properties;
                VideoClub vc = new VideoClub(row["name"].ToString(), row["city"].ToString(),row["adress"].ToString(),row["email"].ToString(),row["phoneNumber"].ToString());
                vc.Id = inode.Id.ToString();
                return vc;
            }
            else
                return null;
        }
    }
}
