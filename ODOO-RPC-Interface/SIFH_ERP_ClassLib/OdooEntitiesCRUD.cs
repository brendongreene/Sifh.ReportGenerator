using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static SIFH_ERP_ClassLib.OdooModelsGet;
using System.Xml.Linq;

namespace SIFH_ERP_ClassLib
{
    public class OdooEntitiesCRUD
    {
        private const string Url = "https://sifhstaging.odoo.com"; //"htttps://adeel9911-sifh17-uat1-19004030.dev.odoo.com";

        private const string DbName = "adeel9911-sifh17-uat1-19357358"; //"adeel9911-sifh17-main-14380822"; //""sifhstaging";
        private const string Username = "admin";
        private const string Password = "1570294945baa5fb63997dbf7a8f4f86ab2e16a0"; //"admin@123!!"; //"
        public int User;
        public static int CreatePartner(int userId)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var partnerData = new XmlRpcStruct
                {
                    { "name", "New Partner" }
                };

                var result = client.ExecuteKw(
                    DbName,
                    userId,
                    Password,
                    "res.partner",
                    "create",
                    new object[] { partnerData },
                    new object[] { }
                );

                var newPartnerId = Convert.ToInt32(result);
                Console.WriteLine($"New Partner ID: {newPartnerId}");
                return newPartnerId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }

        public static bool UpdatePartner(int userId, int partnerId)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var updateData = new XmlRpcStruct
                {
                    { "name", "Newer Partner" }
                };

                object result = client.ExecuteKw(
                    DbName,
                    userId,
                    Password,
                    "res.partner",
                    "write",
                    new object[] { new int[] { partnerId }, updateData },
                    new object[] { }
                );

                bool success = Convert.ToBoolean(result);
                Console.WriteLine($"Update Successful: {success}");
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public static bool DeletePartner(int userId, int partnerId)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                object result = client.ExecuteKw(
                    DbName,
                    userId,
                    Password,
                    "res.partner",
                    "unlink",
                    new object[] { new int[] { partnerId } },
                    new object[] { }
                );

                bool success = Convert.ToBoolean(result);
                Console.WriteLine($"Delete Successful: {success}");
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
