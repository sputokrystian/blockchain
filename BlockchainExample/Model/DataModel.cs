using System;
using System.Linq;

namespace BlockchainExample.Model
{
    [Serializable]
    public partial class Data
    {
        public Data()
        {
            this.Name = string.Empty;
            this.SomeData = string.Empty;
        }

        public Data(string Name, string SomeData)
        {
            this.Name = Name;
            this.SomeData = SomeData;
        }

        public int? SaveDataInDatabase()
        {
            using (var db = new BlockchainExampleEntities())
            {
                try
                {
                    db.Data.Add(this);
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

        public Data GetDataByID()
        {
            using (var db = new BlockchainExampleEntities())
            {
                try
                {
                    return db.Data.FirstOrDefault(u => u.ID == this.ID);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public bool RemoveData()
        {
            BlockchainExampleEntities db = new BlockchainExampleEntities();
            try
            {
                var DataToRemove = db.Data.FirstOrDefault(u => u.ID == this.ID);
                if (DataToRemove == null)
                {
                    throw new Exception();
                }
                db.Data.Remove(DataToRemove);
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