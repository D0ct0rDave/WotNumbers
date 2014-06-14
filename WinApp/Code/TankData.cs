﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Code
{
	public static class TankData
	{
		#region DatabaseLookup

		public static DataTable tankList = new DataTable();

		public static void GetTankListFromDB()
		{
			tankList.Clear();
			tankList = DB.FetchData("SELECT * FROM tank");
			foreach (DataRow dr in tankList.Rows)
			{
				// Replace WoT API tank name with Phalynx Dossier tank name
				string tankName = dr["name"].ToString();
				tankName = tankName.Replace("ö", "?");
				tankName = tankName.Replace("ä", "?");
				tankName = tankName.Replace("â", "?");
				tankName = tankName.Replace("ß", "?");
				dr["name"] = tankName;
				dr.AcceptChanges();
			}
			tankList.AcceptChanges();
		}

		public static DataTable GetPlayerTankFromDB(int tankId)
		{
			string sql = "SELECT * FROM playerTank WHERE playerId=@playerId AND tankId=@tankId; ";
			DB.AddWithValue(ref sql, "@playerId", Config.Settings.playerId, DB.SqlDataType.Int);
			DB.AddWithValue(ref sql, "@tankId", tankId, DB.SqlDataType.Int);
			return DB.FetchData(sql);
		}

		public static DataTable GetPlayerTankBattleFromDB(int playerTankId, string battleMode)
		{
			string sql = "SELECT * FROM playerTankBattle WHERE playerTankId=@playerId AND battleMode=@battleMode; ";
			DB.AddWithValue(ref sql, "@playerId", playerTankId, DB.SqlDataType.Int);
			DB.AddWithValue(ref sql, "@battleMode", battleMode, DB.SqlDataType.VarChar);
			return DB.FetchData(sql);
		}


		public static int GetPlayerTankCount()
		{
			string sql = "SELECT count(id) AS count FROM playerTank WHERE playerId=@playerId; ";
			DB.AddWithValue(ref sql, "@playerId", Config.Settings.playerId, DB.SqlDataType.Int);
			DataTable dt = DB.FetchData(sql);
			int count = 0;
			if (dt.Rows.Count > 0) count = Convert.ToInt32(dt.Rows[0]["count"]);
			return count;
		}

		public static int ConvertWs2TankId(int wsTankId, int wsCountryId)
		{
			string sql = "SELECT tankId FROM wsTankId WHERE wsTankId=@wsTankId AND wsCountryId=@wsCountryId; ";
			DB.AddWithValue(ref sql, "@wsTankId", wsTankId, DB.SqlDataType.Int);
			DB.AddWithValue(ref sql, "@wsCountryId", wsCountryId, DB.SqlDataType.Int);
			DataTable dt = DB.FetchData(sql);
			int lookupTankId = 0;
			if (dt.Rows.Count > 0) lookupTankId = Convert.ToInt32(dt.Rows[0]["tankId"]);
			return lookupTankId;
		}


		public static int GetPlayerTankId(int tankId)
		{
			string sql = "SELECT playerTank.id " +
						 "FROM playerTank INNER JOIN tank ON playerTank.tankid = tank.id " +
						 "WHERE tank.id=@id and playerTank.playerId=@playerId; ";
			DB.AddWithValue(ref sql, "@playerId", Config.Settings.playerId, DB.SqlDataType.Int);
			DB.AddWithValue(ref sql, "@id", tankId, DB.SqlDataType.Int);
			DataTable dt = DB.FetchData(sql);
			int lookupTankId = 0;
			if (dt.Rows.Count > 0) lookupTankId = Convert.ToInt32(dt.Rows[0][0]);
			return lookupTankId;
		}

		public static int GetPlayerTankId(string tankName)
		{
			string sql = "SELECT playerTank.id " +
						 "FROM playerTank INNER JOIN tank ON playerTank.tankid = tank.id " +
						 "WHERE tank.name=@name and playerTank.playerId=@playerId; ";
			DB.AddWithValue(ref sql, "@playerId", Config.Settings.playerId, DB.SqlDataType.Int);
			DB.AddWithValue(ref sql, "@name", tankName, DB.SqlDataType.VarChar);
			DataTable dt = DB.FetchData(sql);
			int lookupTankId = 0;
			if (dt.Rows.Count > 0) lookupTankId = Convert.ToInt32(dt.Rows[0][0]);
			return lookupTankId;
		}

		public static DataTable GetBattleFromDB(int battleId)
		{
			string sql = "SELECT * FROM battle WHERE id=@id; ";
			DB.AddWithValue(ref sql, "@id", battleId, DB.SqlDataType.Int);
			return DB.FetchData(sql);
		}

		public static int GetBattleIdForImportedWsBattleFromDB(int wsId)
		{
			string sql = "SELECT Id FROM battle WHERE wsId=@wsId; ";
			DB.AddWithValue(ref sql, "@wsId", wsId, DB.SqlDataType.Int); 
			DataTable dt = DB.FetchData(sql);
			int lookupBattle = 0;
			if (dt.Rows.Count > 0) lookupBattle = Convert.ToInt32(dt.Rows[0]["Id"]);
			return (lookupBattle);
		}

		public static DataTable json2dbMapping = new DataTable();
		
		public static void GetJson2dbMappingFromDB()
		{
			json2dbMapping.Clear();
			json2dbMapping = DB.FetchData("SELECT * FROM json2dbMapping ORDER BY jsonMainSubProperty");
		}

		public static DataTable GetTankData2BattleMappingFromDB(string battleMode)
		{
			string sql =
				"SELECT  dbDataType, dbPlayerTank, dbPlayerTankMode, dbBattle " +
				"FROM    dbo.json2dbMapping " +
				"WHERE   (dbBattle IS NOT NULL) AND (dbPlayerTankMode IS NULL OR dbPlayerTankMode=@dbPlayerTankMode) " +
				"GROUP BY dbDataType, dbPlayerTank, dbBattle, dbPlayerTankMode ";
			DB.AddWithValue(ref sql, "@dbPlayerTankMode", battleMode, DB.SqlDataType.VarChar);
			return DB.FetchData(sql);
		}

		#endregion

		#region LookupData

		// TODO: just for testing
		public static string ListTanks()
		{
			string s = "";
			foreach (DataRow dr in tankList.Rows)
			{
				s += dr["id"] + ":" + dr["name"] + ", ";
			}
			return s;
		}

		public static int GetTankID(string TankName)
		{
			int tankID = 0;
			string expression = "name = '" + TankName + "'";
			DataRow[] foundRows = tankList.Select(expression);
			if (foundRows.Length > 0) // If tank exist in Tank table 
			{
				tankID = Convert.ToInt32(foundRows[0]["id"]);
			}
			return tankID;
		}


		public static int GetTankID(string TankName, out int TankTier)
		{
			int tankID = 0;
			TankTier = 0;
			string expression = "name = '" + TankName + "'";
			DataRow[] foundRows = tankList.Select(expression);
			if (foundRows.Length > 0) // If tank exist in Tank table 
			{
				tankID = Convert.ToInt32(foundRows[0]["id"]);
				TankTier = Convert.ToInt32(foundRows[0]["tier"]);
			}
			return tankID;
		}

		public static bool GetAchievmentExist(string achName)
		{
			bool exists = false;
			string sql = "SELECT ach.id FROM ach WHERE name=@name; ";
			DB.AddWithValue(ref sql, "@name", achName, DB.SqlDataType.VarChar);
			DataTable dt = DB.FetchData(sql);
			exists = (dt.Rows.Count > 0);
			return exists;
		}

		public static bool TankExist(int tankID)
		{
			string expression = "id = " + tankID.ToString();
			DataRow[] foundRows = tankList.Select(expression);
			return (foundRows.Length > 0);
		}

		public static DataRow TankInfo(int tankID)
		{
			string expression = "id = " + tankID.ToString();
			DataRow[] foundRows = tankList.Select(expression);
			if (foundRows.Length > 0)
				return foundRows[0];
			else
				return null;
		}
		
		#endregion
	   
	}
}
