using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// QueryClass의 요약 설명입니다.
/// </summary>

namespace AppCommon
{
	public class Query
	{
		private string strSelectQuery;
		private string strInsertQuery;
		private string strUpdateQuery;
		private string strDeleteQuery;
		private string strTotalRowQuery;

		private string strTable;
		private string strGroup;
		private string strTop;
		private bool bDistinct;
		private int nLimitStart;
		private int nLimitLength;
		private string strKeyColumn;

		private ArrayList aryJoinTable;
		private ArrayList aryJoinTableOn;
		private ArrayList aryColumn;
		private ArrayList aryColumnValue;
		private ArrayList aryColumnType;
		private ArrayList aryCondition;
		private ArrayList aryOrder;

		public Query()
		{
			this.aryJoinTable = new ArrayList();
			this.aryJoinTableOn = new ArrayList();
			this.aryColumn = new ArrayList();
			this.aryColumnValue = new ArrayList();
			this.aryColumnType = new ArrayList();
			this.aryCondition = new ArrayList();
			this.aryOrder = new ArrayList();
			this.Clear();
		}

		public void Clear()
		{
			this.nLimitStart = 0;
			this.nLimitLength = 0;
			this.strKeyColumn = "[no]";

			this.strSelectQuery = "";
			this.strInsertQuery = "";
			this.strUpdateQuery = "";
			this.strDeleteQuery = "";
			this.strTotalRowQuery = "";

			this.strTable = "";
			this.strGroup = "";
			this.strTop = "";
			this.bDistinct = false;

			this.aryJoinTable.Clear();
			this.aryJoinTableOn.Clear();
			this.aryColumn.Clear();
			this.aryColumnValue.Clear();
			this.aryColumnType.Clear();
			this.aryCondition.Clear();
			this.aryOrder.Clear();
		}

		public void SetTable(string table)
		{
			this.strTable = table;
		}

		public void SetGroup(string group)
		{
			this.strGroup = group;
		}

		public void SetDistinct()
		{
			this.bDistinct = true;
		}

		public void UnSetDistinct()
		{
			this.bDistinct = false;
		}

		public void AddLeftJoin(string join_table)
		{
			this.aryJoinTable.Add(join_table);
			this.aryJoinTableOn.Add("");
		}

		public void AddLeftJoin(string join_table, string join_condition)
		{
			this.aryJoinTable.Add(join_table);
			this.aryJoinTableOn.Add(join_condition);
		}

		public void AddColumn(string column_name)
		{
			this.aryColumn.Add(column_name);
			this.aryColumnValue.Add("");
			this.aryColumnType.Add("value");
		}
		public void AddColumn(string column_name, bool column_value)
		{
			this.aryColumn.Add(column_name);
			if (column_value)
			{
				this.aryColumnValue.Add("1");
			}
			else
			{
				this.aryColumnValue.Add("0");
			}
			this.aryColumnType.Add("value");
		}
		public void AddColumn(string column_name, int column_value)
		{
			this.aryColumn.Add(column_name);
			this.aryColumnValue.Add(column_value.ToString());
			this.aryColumnType.Add("value");
		}
		public void AddColumn(string column_name, string column_value)
		{
			this.aryColumn.Add(column_name);
			if (column_value == null || column_value == "")
				this.aryColumnValue.Add("");
			else
				this.aryColumnValue.Add(column_value.Replace("'", "''"));
			this.aryColumnType.Add("value");
		}
		public void AddColumn(string column_name, string column_value, bool is_function)
		{
			if (is_function)
			{
				this.aryColumn.Add(column_name);
				this.aryColumnValue.Add(column_value);
				this.aryColumnType.Add("func");
			}
			else
			{
				AddColumn(column_name, column_name);
			}
		}
		public void AddNVColumn(string column_name, string column_value)
		{
			this.aryColumn.Add(column_name);
			this.aryColumnValue.Add(column_value);
			this.aryColumnType.Add("NVARCHAR");
		}

		public void AddCondition(string condition)
		{
			this.aryCondition.Add(condition);
		}

		public void AddOrder(string order)
		{
			this.aryOrder.Add(order);
		}

		public void SetLimit(int start, int length)
		{
			this.nLimitStart = start;
			this.nLimitLength = length;
			this.strKeyColumn = "[no]";
		}

		public void SetLimit(int start, int length, string key_column)
		{
			this.nLimitStart = start;
			this.nLimitLength = length;
			this.strKeyColumn = key_column;
		}

		public string TotalRowQuery(bool bIncludeGroup)
		{
			this.strTotalRowQuery = "";
			this.strTotalRowQuery += " SELECT COUNT(*) AS total_row ";
			this.strTotalRowQuery += " FROM " + this.strTable;
			this.strTotalRowQuery += this.GetLeftJoinTable();
			this.strTotalRowQuery += this.GetConditionWord();
			if (bIncludeGroup)
			{
				if (this.strGroup != "")
				{
					this.strTotalRowQuery += " GROUP BY ";
					this.strTotalRowQuery += this.strGroup;
				}
			}
			return this.strTotalRowQuery;
		}

		public string SelectQueryNoLimit()
		{
			this.strSelectQuery = "";
			this.strSelectQuery += " SELECT ";
			if (this.bDistinct)
				this.strSelectQuery += "DISTINCT ";
			this.strSelectQuery += this.GetColumnWord();
			this.strSelectQuery += " FROM ";
			this.strSelectQuery += this.strTable;
			this.strSelectQuery += this.GetLeftJoinTable();
			this.strSelectQuery += this.GetConditionWord();
			if (this.strGroup != "")
			{
				this.strSelectQuery += " GROUP BY ";
				this.strSelectQuery += this.strGroup;
			}
			this.strSelectQuery += this.GetOrderWord();

			return this.strSelectQuery;
		}

		public string SelectQuery()
		{
			if (this.strSelectQuery != "") return this.strSelectQuery;

			bool bLimit = false;
			if (this.nLimitLength == 0 && this.nLimitStart == 0)
			{
				bLimit = false;
			}
			else
			{
				bLimit = true;
			}

			this.strSelectQuery = "";

			this.strSelectQuery += " SELECT ";
			if (this.bDistinct)
				this.strSelectQuery += "DISTINCT ";
			if (bLimit)
				this.strSelectQuery += " TOP " + this.nLimitLength + " ";
			this.strSelectQuery += this.GetColumnWord();
			this.strSelectQuery += " FROM ";
			this.strSelectQuery += this.strTable;
			this.strSelectQuery += this.GetLeftJoinTable();
			this.strSelectQuery += this.GetConditionWord();
			if (bLimit)
			{
				if ((string)this.GetConditionWord() == "")
				{
					this.strSelectQuery += " WHERE ";
				}
				else
				{
					this.strSelectQuery += " AND ";
				}
				this.strSelectQuery += " " + this.strKeyColumn
					+ " NOT IN (SELECT ";
				if (this.bDistinct)
					this.strSelectQuery += "DISTINCT ";
				this.strSelectQuery += "TOP " + this.nLimitStart + " " + this.strKeyColumn
					 + " FROM " + this.strTable + " "
					 + this.GetLeftJoinTable() + " "
					 + this.GetConditionWord();
				if (this.strGroup != "")
				{
					this.strSelectQuery += " GROUP BY ";
					this.strSelectQuery += this.strGroup;
				}
				this.strSelectQuery += " " + this.GetOrderWord();
				this.strSelectQuery += ") ";
			}
			if (this.strGroup != "")
			{
				this.strSelectQuery += " GROUP BY ";
				this.strSelectQuery += this.strGroup;
			}
			this.strSelectQuery += this.GetOrderWord();

			return this.strSelectQuery;
		}

		public string SelectFunctionQuery()
		{
			this.strSelectQuery = "";
			this.strSelectQuery += " SELECT ";
			this.strSelectQuery += this.GetColumnWord();

			return this.strSelectQuery;
		}

		public string InsertQuery()
		{
			if (this.strInsertQuery != "") return this.strInsertQuery;

			bool bLimit = false;
			if (this.nLimitLength == 0 && this.nLimitStart == 0)
			{
				bLimit = false;
			}
			else
			{
				bLimit = true;
			}

			this.strInsertQuery = "";

			this.strInsertQuery += " INSERT INTO " + this.strTable;

			this.strInsertQuery += "(" + this.GetColumnWord() + ")";

			this.strInsertQuery += " VALUES (" + this.GetColumnValueWord() + ")";

			return this.strInsertQuery;
		}

		public string UpdateQuery()
		{
			if (this.strUpdateQuery != "") return this.strUpdateQuery;

			bool bLimit = false;
			if (this.nLimitLength == 0 && this.nLimitStart == 0)
			{
				bLimit = false;
			}
			else
			{
				bLimit = true;
			}

			this.strUpdateQuery = "";

			this.strUpdateQuery += " UPDATE " + this.strTable;
			this.strUpdateQuery += " SET";

			if (this.aryColumn.Count > 0)
			{
				for (int i = 0; i < this.aryColumn.Count; i++)
				{
					if (i == 0)
					{
						if ((string)this.aryColumnType[i] == "func")
						{
							this.strUpdateQuery += " " + (string)this.aryColumn[i];
							this.strUpdateQuery += " = " + (string)this.aryColumnValue[i];
						}
						else if ((string)this.aryColumnType[i] == "NVARCHAR")
						{
							this.strUpdateQuery += " " + (string)this.aryColumn[i];
							this.strUpdateQuery += " = N'" + (string)this.aryColumnValue[i] + "'";
						}
						else
						{
							this.strUpdateQuery += " " + (string)this.aryColumn[i];
							this.strUpdateQuery += " = '" + (string)this.aryColumnValue[i] + "'";
						}
					}
					else
					{
						if ((string)this.aryColumnType[i] == "func")
						{
							this.strUpdateQuery += " ," + (string)this.aryColumn[i];
							this.strUpdateQuery += " = " + (string)this.aryColumnValue[i];
						}
						else if ((string)this.aryColumnType[i] == "NVARCHAR")
						{
							this.strUpdateQuery += " ," + (string)this.aryColumn[i];
							this.strUpdateQuery += " = N'" + (string)this.aryColumnValue[i] + "'";
						}
						else
						{
							this.strUpdateQuery += " ," + (string)this.aryColumn[i];
							this.strUpdateQuery += " = '" + (string)this.aryColumnValue[i] + "'";
						}
					}
				}
			}

			this.strUpdateQuery += this.GetConditionWord();

			return this.strUpdateQuery;
		}

		public string DeleteQuery()
		{
			if (this.strDeleteQuery != "") return this.strDeleteQuery;

			bool bLimit = false;
			if (this.nLimitLength == 0 && this.nLimitStart == 0)
			{
				bLimit = false;
			}
			else
			{
				bLimit = true;
			}

			this.strDeleteQuery = "";

			this.strDeleteQuery += " DELETE FROM " + this.strTable;

			this.strDeleteQuery += this.GetConditionWord();

			return this.strDeleteQuery;
		}

		public string GetLeftJoinTable()
		{
			string word = "";
			string table = "";
			string on = "";

			int length = this.aryJoinTable.Count;
			if (length > 0)
			{
				for (int i = 0; i < length; i++)
				{
					table = (string)this.aryJoinTable[i];
					word += " LEFT OUTER JOIN " + table + " ";
					if ((string)this.aryJoinTableOn[i] != "")
					{
						on = (string)this.aryJoinTableOn[i];
						word += " ON " + on + " ";
					}
				}
			}

			return word;
		}

		public string GetColumnWord()
		{
			string word = "";
			if (this.aryColumn.Count > 0)
			{
				for (int i = 0; i < this.aryColumn.Count; i++)
				{
					if (word == "")
					{
						word += (string)this.aryColumn[i];
					}
					else
					{
						word += ", " + (string)this.aryColumn[i];
					}
				}
			}

			return word;
		}

		public string GetColumnValueWord()
		{
			string word = "";
			if (this.aryColumnValue.Count > 0)
			{
				for (int i = 0; i < this.aryColumnValue.Count; i++)
				{
					string value;
					if (this.aryColumnType[i].Equals("func"))
					{
						value = (string)this.aryColumnValue[i];
					}
					else if (this.aryColumnType[i].Equals("NVARCHAR"))
					{
						value = "N'" + (string)this.aryColumnValue[i] + "'";
					}
					else
					{
						value = "'" + (string)this.aryColumnValue[i] + "'";
					}

					if (word == "")
						word += value;
					else
						word += "," + value;
				}
			}

			return word;
		}

		public string GetConditionWord()
		{
			string word = "";
			if (this.aryCondition.Count > 0)
			{
				for (int i = 0; i < this.aryCondition.Count; i++)
				{
					if (word == "")
					{
						word += (string)this.aryCondition[i];
					}
					else
					{
						word += " AND " + this.aryCondition[i];
					}
				}
				word = " WHERE " + word;
			}
			return word;
		}

		public string GetOrderWord()
		{
			string word = "";
			if (this.aryOrder.Count > 0)
			{
				for (int i = 0; i < this.aryOrder.Count; i++)
				{
					if (word == "")
					{
						word += (string)this.aryOrder[i];
					}
					else
					{
						word += ", " + (string)this.aryOrder[i];
					}
				}
				word = " ORDER BY " + word;
			}
			return word;
		}
	}
}
