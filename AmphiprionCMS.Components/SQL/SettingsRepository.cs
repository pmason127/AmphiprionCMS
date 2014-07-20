using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amphiprion.Data;
using DapperExtensions;

namespace AmphiprionCMS.Components.SQL
{
    public interface ISettingsRepository
    {
        Amphiprion.Data.Entities.SiteSettings Get(int id);
        void Update(Amphiprion.Data.Entities.SiteSettings settings);
    }

    public class SettingsRepository : ISettingsRepository
    {
        private IConnectionManager _connectionManager = null;
        public SettingsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public Amphiprion.Data.Entities.SiteSettings Get(int id)
        {
            using (var con = _connectionManager.GetConnection())
            {
                var settings = con.Get<Amphiprion.Data.Entities.SiteSettings>(id);
                return settings;
            }
        }
        public void Update(Amphiprion.Data.Entities.SiteSettings settings)
        {
            using (var con = _connectionManager.GetConnection())
            {
                con.Update<Amphiprion.Data.Entities.SiteSettings>(settings);
            }
        }
    }
}
