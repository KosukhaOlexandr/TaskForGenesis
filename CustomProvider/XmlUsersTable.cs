using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using TaskForGenesis.Models;

using System.Xml;
using System.Xml.Linq;

namespace TaskForGenesis.CustomProvider
{
    public class XmlUsersTable
    {
        private readonly string _path;
        private XDocument _doc;
        public XmlUsersTable(string path)
        {
            _path = path;
            _doc = new XDocument(XDocument.Load(path));
        }

        public IdentityResult Create(User user)
        {
            XElement users = _doc.Element("Users");
            XElement newUser = new XElement("User");
            newUser.Add(new XAttribute("Email", user.Email));
            newUser.Add(new XAttribute("Id", user.Id));
            newUser.Add(new XAttribute("UserName", user.UserName));
            newUser.Add(new XAttribute("PasswordHash", user.PasswordHash));
            users.Add(newUser);

            _doc.Save(_path);
            
            if (_doc.Descendants().ToList().Count > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = $"Could not create user {user.Email}." });
        }

        public User FindById (Guid userId)
        {
            var userSearched = (from obj in _doc.Descendants("User")
                                where
                                obj.Attribute("Id").Value.Equals(userId)
                                select new
                                {
                                    id = (string)obj.Attribute("Id"),
                                    username = (string)obj.Attribute("UserName"),
                                    email = (string)obj.Attribute("Email"),
                                    passwordHash = (string)obj.Attribute("PasswordHash")
                                }).FirstOrDefault();

            if (userSearched == null)
                return null;

            User user = new User
            {
                Id = userSearched.id,
                Email = userSearched.email,
                UserName = userSearched.username,
                PasswordHash = userSearched.passwordHash
            };
            return user; 
            
        }

        public User FindByName(string userName)
        {
            userName = userName.ToLower();
            var userNames = (from obj in _doc.Descendants("User")
                             select obj.Attribute("UserName")).ToList();

            var userSearched = (from obj in _doc.Descendants("User")
                                where
                                obj.Attribute("UserName").Value.Equals(userName)
                                select new
                                {
                                    id = (string)obj.Attribute("Id"),
                                    email = (string)obj.Attribute("Email"),
                                    username = (string)obj.Attribute("UserName"),
                                    passwordHash = (string)obj.Attribute("PasswordHash")
                                }).FirstOrDefault();

            if (userSearched == null)
                return null;

            User user = new User
            {
                Id = userSearched.id,
                Email = userSearched.email,
                UserName = userSearched.username,
                PasswordHash = userSearched.passwordHash
            };
            return user;

        }
    }
}
