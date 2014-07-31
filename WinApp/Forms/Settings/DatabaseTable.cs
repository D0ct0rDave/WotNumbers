﻿using System;
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
	public partial class DatabaseTable : Form
	{
		private string tableList = "";
		
		public DatabaseTable()
		{
			InitializeComponent();
		}

		// To be able to minimize from task bar
		const int WS_MINIMIZEBOX = 0x20000;
		const int CS_DBLCLKS = 0x8;
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.Style |= WS_MINIMIZEBOX;
				cp.ClassStyle |= CS_DBLCLKS;
				return cp;
			}
		}


		private void DatabaseTable_Load(object sender, EventArgs e)
		{
			// Make sure borderless form do not cover task bar when maximized
			Screen screen = Screen.FromControl(this);
			this.MaximumSize = screen.WorkingArea.Size;
			// Add Mouse Wheel handle
			dataGridViewShowTable.MouseWheel += new MouseEventHandler(dataGridViewShowTable_MouseWheel); 
			// Scroll and grid size
			scrollCorner.Left = DatabaseTableTheme.MainArea.Right - scrollCorner.Width;
			scrollCorner.Top = DatabaseTableTheme.MainArea.Bottom - scrollCorner.Height;
			scrollY.Top = DatabaseTableTheme.MainArea.Top + 45;
			scrollY.Left = DatabaseTableTheme.MainArea.Right - scrollY.Width;
			scrollY.Height = DatabaseTableTheme.MainArea.Height - 45 - scrollCorner.Height;
			scrollX.Left = DatabaseTableTheme.MainArea.Left;
			scrollX.Top = DatabaseTableTheme.MainArea.Bottom - scrollX.Height;
			scrollX.Width = DatabaseTableTheme.MainArea.Width - scrollCorner.Width;
			dataGridViewShowTable.Top = DatabaseTableTheme.MainArea.Top + 45;
			dataGridViewShowTable.Left = DatabaseTableTheme.MainArea.Left;
			dataGridViewShowTable.Width = DatabaseTableTheme.MainArea.Width - scrollY.Width;
			dataGridViewShowTable.Height = DatabaseTableTheme.MainArea.Height - 45 - scrollX.Height;
			// Style datagrid
			dataGridViewShowTable.BorderStyle = BorderStyle.None;
			dataGridViewShowTable.BackgroundColor = ColorTheme.FormBack;
			dataGridViewShowTable.GridColor = ColorTheme.GridBorders;
			dataGridViewShowTable.EnableHeadersVisualStyles = false;
			dataGridViewShowTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			dataGridViewShowTable.ColumnHeadersHeight = 30;
			dataGridViewShowTable.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			dataGridViewShowTable.ColumnHeadersDefaultCellStyle.BackColor = ColorTheme.GridHeaderBackLight;
			dataGridViewShowTable.ColumnHeadersDefaultCellStyle.ForeColor = ColorTheme.ControlFont;
			dataGridViewShowTable.ColumnHeadersDefaultCellStyle.SelectionForeColor = ColorTheme.ControlFont;
			dataGridViewShowTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = ColorTheme.GridSelectedHeaderColor;
			dataGridViewShowTable.RowHeadersWidth = 20;
			dataGridViewShowTable.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			dataGridViewShowTable.RowHeadersDefaultCellStyle.BackColor = ColorTheme.GridHeaderBackLight;
			dataGridViewShowTable.RowHeadersDefaultCellStyle.ForeColor = ColorTheme.ControlFont;
			dataGridViewShowTable.RowHeadersDefaultCellStyle.SelectionForeColor = ColorTheme.ControlFont;
			dataGridViewShowTable.RowHeadersDefaultCellStyle.SelectionBackColor = ColorTheme.GridSelectedHeaderColor;
			dataGridViewShowTable.DefaultCellStyle.BackColor = ColorTheme.FormBack;
			dataGridViewShowTable.DefaultCellStyle.ForeColor = ColorTheme.ControlFont;
			dataGridViewShowTable.DefaultCellStyle.SelectionForeColor = ColorTheme.ControlFont;
			dataGridViewShowTable.DefaultCellStyle.SelectionBackColor = ColorTheme.GridSelectedCellColor;
			// dropdown tables
			DataTable dt = DB.ListTables();
			foreach (DataRow dr in dt.Rows)
			{
				tableList += dr["TABLE_NAME"].ToString() + ",";
			}
			if (tableList.Length > 0)
				tableList = tableList.Substring(0, tableList.Length - 1); // remove last comma
			popupSelectTable.Text = ""; // Avoid NULL as default value
			RefreshScrollbars();
		}

		private void popupSelectTable_Click(object sender, EventArgs e)
		{
			// Show popup with available tables
			Code.DropDownGrid.Show(popupSelectTable, Code.DropDownGrid.DropDownGridType.List, tableList);
		}


		#region Grid

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			RefreshDataGrid();
		}

		private void RefreshDataGrid()
		{
			// Show content in grid
			string TableName = popupSelectTable.Text.ToString();
			if (TableName != "")
			{
				dataGridViewShowTable.DataSource = DB.FetchData("SELECT * FROM " + TableName);
			}
			ResizeNow();			
		}

		#endregion

		#region Resizing

		private void DatabaseTable_Resize(object sender, EventArgs e)
		{
			ResizeNow();
		}

		private void DatabaseTable_ResizeEnd(object sender, EventArgs e)
		{
			ResizeNow();
		}

		private void ResizeNow()
		{
			// First set scrollbars, size differs according to scrollbar visibility (ScrollNecessary)
			RefreshScrollbars();
			// Scroll and grid size
			scrollCorner.Left = DatabaseTableTheme.MainArea.Right - scrollCorner.Width;
			scrollCorner.Top = DatabaseTableTheme.MainArea.Bottom - scrollCorner.Height;
			scrollY.Left = DatabaseTableTheme.MainArea.Right - scrollY.Width;
			scrollX.Top = DatabaseTableTheme.MainArea.Bottom - scrollX.Height;
			// check if scrollbar is visible to determine width / height
			int scrollYWidth = 0; 
			int scrollXHeight = 0;
			if (scrollY.ScrollNecessary) scrollYWidth = scrollY.Width;
			if (scrollX.ScrollNecessary) scrollXHeight = scrollX.Height;
			dataGridViewShowTable.Width = DatabaseTableTheme.MainArea.Width - scrollYWidth;
			dataGridViewShowTable.Height = DatabaseTableTheme.MainArea.Height - 45 - scrollXHeight;
			scrollY.Height = dataGridViewShowTable.Height;
			scrollX.Width = dataGridViewShowTable.Width;
		}
		#endregion

		#region Scrolling

		// Set scrollbar properties according to grid content
		private void RefreshScrollbars()
		{
			int XVisible = 0;
			int XTotal = 0;
			int YVisible = 0;
			int YTotal = 0;
			// Calc scroll boundarys
			if (dataGridViewShowTable.RowCount > 0)
			{
				YTotal = dataGridViewShowTable.RowCount;
				YVisible = dataGridViewShowTable.DisplayedRowCount(false);
				XTotal = dataGridViewShowTable.ColumnCount;
				XVisible = dataGridViewShowTable.DisplayedColumnCount(false);
			}
			// Scroll init
			scrollX.ScrollElementsVisible = XVisible;
			scrollX.ScrollElementsTotals = XTotal;
			scrollY.ScrollElementsVisible = YVisible;
			scrollY.ScrollElementsTotals = YTotal;
			// Scroll corner
			scrollCorner.Visible = (scrollX.ScrollNecessary && scrollY.ScrollNecessary);
		}

		// Scrolling Y
		private bool scrollingY = false;
		private void scrollY_MouseDown(object sender, MouseEventArgs e)
		{
			scrollingY = true;
			ScrollY();
		}

		private void scrollY_MouseMove(object sender, MouseEventArgs e)
		{
			if (scrollingY) ScrollY();
		}

		private void ScrollY()
		{
			int posBefore = dataGridViewShowTable.FirstDisplayedScrollingRowIndex;
			dataGridViewShowTable.FirstDisplayedScrollingRowIndex = scrollY.ScrollPosition;
			if (posBefore != dataGridViewShowTable.FirstDisplayedScrollingRowIndex) Refresh();
		}

		private void scrollY_MouseUp(object sender, MouseEventArgs e)
		{
			scrollingY = false;
		}


		// Scrolling X
		private bool scrollingX = false;
		private void scrollX_MouseDown(object sender, MouseEventArgs e)
		{
			scrollingX = true;
			ScrollX();
		}

		private void scrollX_MouseUp(object sender, MouseEventArgs e)
		{
			scrollingX = false;
		}

		private void scrollX_MouseMove(object sender, MouseEventArgs e)
		{
			if (scrollingX) ScrollX();
		}

		private void ScrollX()
		{
			int posBefore = dataGridViewShowTable.FirstDisplayedScrollingColumnIndex;
			dataGridViewShowTable.FirstDisplayedScrollingColumnIndex = scrollX.ScrollPosition;
			if (posBefore != dataGridViewShowTable.FirstDisplayedScrollingColumnIndex) Refresh();
		}

		// Move scrollbar according to grid movements
		private void dataGridViewShowTable_SelectionChanged(object sender, EventArgs e)
		{
			MoveScrollBar();
		}

		private void MoveScrollBar()
		{
			scrollX.ScrollPosition = dataGridViewShowTable.FirstDisplayedScrollingColumnIndex;
			scrollY.ScrollPosition = dataGridViewShowTable.FirstDisplayedScrollingRowIndex;
		}

		// Enable mouse wheel scrolling for datagrid
		private void dataGridViewShowTable_MouseWheel(object sender, MouseEventArgs e)
		{
			try
			{
				// scroll in grid from mouse wheel
				int currentIndex = this.dataGridViewShowTable.FirstDisplayedScrollingRowIndex;
				int scrollLines = SystemInformation.MouseWheelScrollLines;
				if (e.Delta > 0)
				{
					this.dataGridViewShowTable.FirstDisplayedScrollingRowIndex = Math.Max(0, currentIndex - scrollLines);
				}
				else if (e.Delta < 0)
				{
					this.dataGridViewShowTable.FirstDisplayedScrollingRowIndex = currentIndex + scrollLines;
				}
				// move scrollbar
				MoveScrollBar();
			}
			catch (Exception)
			{
				// throw;
			}

		}

		#endregion

		private void popupSelectTable_TextChanged(object sender, EventArgs e)
		{
			RefreshDataGrid();
		}

		private void DatabaseTable_LocationChanged(object sender, EventArgs e)
		{
			Screen screen = Screen.FromControl(this);
			this.MaximumSize = screen.WorkingArea.Size;

		}

		private void DatabaseTable_FormClosed(object sender, FormClosedEventArgs e)
		{
			FormHelper.ClosedOne();
		}

	}
}
