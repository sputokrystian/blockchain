using System;
using System.Linq;

namespace BlockchainExample.Model
{
    [Serializable]
    public partial class Users
    {
        public Users()
        {
            this.Name = string.Empty;
            this.SomeData = string.Empty;
        }

        public Users(string Name, string SomeData)
        {
            this.Name = Name;
            this.SomeData = SomeData;
        }

        public int? SaveUserInDatabase()
        {
            using (var db = new BlockchainExampleEntities())
            {
                try
                {
                    db.Users.Add(this);
                    db.SaveChanges();
                    db.Dispose();
                    return this.ID;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Users GetUserByID()
        {
            using (var db = new BlockchainExampleEntities())
            {
                try
                {
                    return db.Users.FirstOrDefault(u => u.ID == this.ID);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public bool RemoveUser()
        {
            BlockchainExampleEntities db = new BlockchainExampleEntities();
            try
            {
                var UserToRemove = db.Users.FirstOrDefault(u => u.ID == this.ID);
                if (UserToRemove == null)
                {
                    throw new Exception();
                }
                db.Users.Remove(UserToRemove);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}