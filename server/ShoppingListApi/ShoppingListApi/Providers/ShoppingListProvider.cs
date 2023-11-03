using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using ShoppingListApi.Interfaces;
using ShoppingListApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShoppingListApi.Providers
{
    public class ShoppingListProvider : IShoppingListProvider
    {
        private readonly ShoppingDb _shoppingDb;

        public ShoppingListProvider(IOptions<ShoppingDb> shoppingDb)
        {
            _shoppingDb = shoppingDb.Value;
        }

        public async Task<List<ShoppingList>> GetShoppingList(string userId)
        {
            List<ShoppingList> shoppingList = new();

            using MySqlConnection myConn = new(_shoppingDb.DefaultConnection);
            try
            {
                myConn.Open();

                string sql = $"SELECT idx, userid, item_name, quantity FROM shoppinglist WHERE userid = @userid;";
                using MySqlCommand cmd = new(sql, myConn);
                cmd.Parameters.AddWithValue("@userid", userId);
                using MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    shoppingList.Add(new ShoppingList()
                    {
                        Index = int.Parse(rdr["idx"].ToString()),
                        UserId = rdr["userid"].ToString(),
                        Item = rdr["item_name"].ToString() ?? "",
                        Quantity = int.Parse(rdr["quantity"].ToString()),
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                myConn.Close();
            }

            return shoppingList;
        }

        public async Task<string> AddItem(ShoppingList item)
        {
            using MySqlConnection myConn = new(_shoppingDb.DefaultConnection);
            try
            {
                myConn.Open();

                string sql = $"INSERT INTO shoppinglist (userid,item_name,quantity) VALUES (@userid, @item, @quantity); SELECT LAST_INSERT_ID();";
                using MySqlCommand cmd = new(sql, myConn);
                cmd.Parameters.AddWithValue("@userid", item.UserId);
                cmd.Parameters.AddWithValue("@item", item.Item);
                cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                //cmd.ExecuteNonQuery();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result).ToString();
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                myConn.Close();
            }
        }

        public async Task<string> EditItem(ShoppingList item)
        {
            using MySqlConnection myConn = new(_shoppingDb.DefaultConnection);
            try
            {
                myConn.Open();

                string sql = $"UPDATE shoppinglist SET item_name = @item, quantity = @quantity WHERE idx = @index;";
                using MySqlCommand cmd = new(sql, myConn);
                cmd.Parameters.AddWithValue("@item", item.Item);
                cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                cmd.Parameters.AddWithValue("@index", item.Index);
                cmd.ExecuteNonQuery();

                return "Success";
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                myConn.Close();
            }
        }

        public async Task<string> DeleteItem(int index)
        {
            using MySqlConnection myConn = new(_shoppingDb.DefaultConnection);
            try
            {
                myConn.Open();

                string sql = $"DELETE FROM shoppinglist WHERE idx = @index;";
                using MySqlCommand cmd = new(sql, myConn);
                cmd.Parameters.AddWithValue("@index", index);
                cmd.ExecuteNonQuery();

                return "Success";
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                myConn.Close();
            }
        }
    }
}