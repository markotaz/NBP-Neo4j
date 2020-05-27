using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.DomainModel
{
    public class User
    {
        public String Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String PhoneNumber { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }

        public User(String fn, String ln, String pn, String email, String pass)
        {
            this.FirstName = fn;
            this.LastName = ln;
            this.PhoneNumber = pn;
            this.Email = email;
            this.Password = pass;
        }
        public static User ParseUser(IResult result)
        {
            List<IRecord> list = result.ToList();
            if (list.Count == 1)
            {
                INode inode = list[0][0] as INode;
                IReadOnlyDictionary<string, object> row = inode.Properties;
                User user = new User(row["firstName"].ToString(),row["lastName"].ToString(),row["phoneNumber"].ToString(),
                    row["email"].ToString(),row["password"].ToString());
                user.Id = inode.Id.ToString();
                return user;
            }
            else
                return null;
        }
    }
}
