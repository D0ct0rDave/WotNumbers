using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApp.Code;

namespace WinApp.Forms
{
	public partial class RecalcBattleAccountId : FormCloseOnEsc
	{
		private static bool _autoRun = false;
		public RecalcBattleAccountId(bool autoRun = false)
		{
			InitializeComponent();
			_autoRun = autoRun;
		}

		private async void RecalcBattleAccountId_Shown(object sender, EventArgs e)
		{
			if (_autoRun)
				await RunNow();
		}

		private string GetProcessingString()
		{
			return "Recalc Battle Account ID ";
		}

		private void UpdateProgressBar(string statusText, int count)
		{
			lblProgressStatus.Text = statusText;
			if (statusText == "")
				badProgressBar.Value = 0;
			else
				badProgressBar.Value += count;
			Refresh();
		}

		private async Task RunNow()
		{
			this.Cursor = Cursors.WaitCursor;
			RecalcBattleAccountIdTheme.Cursor = Cursors.WaitCursor;
			btnStart.Enabled = false;
			badProgressBar.Value = 0;
			badProgressBar.Visible = true;
			// Get battles
			UpdateProgressBar("Getting battle count", 0);

			// De aquí tenemos que sacasr una row con la battle Id de la tabla battle y el accountId de la tabla Players
			string sql =
				"SELECT battle.id as battleId, battle.battleTime as battleTime, battlePlayer.accountId as accountId " +
				"from battle " +
				"inner join battlePlayer on battle.id = battlePlayer.battleId " +
				"inner join player on battlePlayer.accountId = player.accountId " +
				"group by battle.id, battle.battleTime " +
				"ORDER BY battle.id";

			DataTable dt = await DB.FetchData(sql);

			int tot = dt.Rows.Count;
			badProgressBar.ValueMax = tot + 2;
			sql = "";
			int loopCount = 0;
			string battleTime = "";

			UpdateProgressBar("Starting updates...", 1);
			foreach (DataRow dr in dt.Rows)
			{
				// Build SQL
				sql += "UPDATE battle SET accountId=" + dr["accountId"].ToString() + " WHERE id=" + dr["battleId"].ToString() + "; " + Environment.NewLine;
				loopCount++;
				if (loopCount >= Constants.RecalcDataBatchSize)
				{
					battleTime = dr["battleTime"].ToString();
					UpdateProgressBar(GetProcessingString() + badProgressBar.Value + "/" + tot.ToString() + " " + battleTime, loopCount);
					await DB.ExecuteNonQuery(sql, Config.Settings.showDBErrors, true);

					loopCount = 0;
					sql = "";
				}
			}

			if (sql != "") // Update last batch of sql's
			{
				UpdateProgressBar(GetProcessingString() + badProgressBar.Value + "/" + tot.ToString() + " " + battleTime, loopCount);
				await DB.ExecuteNonQuery(sql, Config.Settings.showDBErrors, true);

				sql = "";
			}

			// Done
			UpdateProgressBar("", 0);
			lblProgressStatus.Text = "Update finished: " + DateTime.Now.ToString();
			btnStart.Enabled = true;

			// Done
			this.Cursor = Cursors.Default;
			this.Close();
		}

		private async void btnStart_Click(object sender, EventArgs e)
		{
			await RunNow();
		}


	}
}
