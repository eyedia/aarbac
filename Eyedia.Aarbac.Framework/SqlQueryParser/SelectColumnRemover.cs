using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class SelectColumnRemover
    {
        #region Properties
        public string Query { get; private set; }
        public string ParsedQuery { get; private set; }
        public string SelectStatement { get; private set; }
        public string OtherStatement { get; private set; }
        public bool IsZeroSelectColumn { get; private set; }
        public RbacSelectColumn Column { get; private set; }        

        public string ColumnName
        {
            get
            {
                //if not found, possibly the column was referred more than once, was removed in last go.
                int position = GetPosition();
                if (position == -1)
                {
                    ParsedQuery = Query;
                    return string.Empty;
                }

                int nextommaPos = SelectStatement.IndexOf(",", position);
                if (nextommaPos == -1) //we hit the last column
                    nextommaPos = SelectStatement.Length;

                return SelectStatement.Substring(position, (nextommaPos - position));
            }
        }

        #endregion Properties

        public SelectColumnRemover(string query, RbacSelectColumn column)
        {
            Query = query;
            Column = column;

            int fromIndex = Query.IndexOf(" into", StringComparison.OrdinalIgnoreCase);
            if (fromIndex == -1)
                fromIndex = Query.IndexOf("from", StringComparison.OrdinalIgnoreCase);
            if (fromIndex == -1)
                RbacException.Raise("Something went wrong while applying permission on select columns, no 'into' or 'from' statement found in the query");


            SelectStatement = Query.Substring(0, fromIndex);
            OtherStatement = " " + Query.Substring(fromIndex, Query.Length - fromIndex);
        }

        public string Remove()
        {
            if (!SelectStatement.Contains(",")) //we hit the single/last column
            {

                ParsedQuery = "SELECT 'null' " + OtherStatement;
                IsZeroSelectColumn = true;

            }
            else if (string.IsNullOrEmpty(ColumnName))
            {
                ParsedQuery = Query;
            }
            else
            {
                //remove the column
                SelectStatement = SelectStatement.Replace(ColumnName, string.Empty);

                //handling comma
                SelectStatement = SelectStatement.Replace(",  ,", ",").Replace(", ,", ",").Replace(",,", ",")
                    .Replace("select ,", "select").Replace("SELECT , ", "SELECT").Trim();                
                if ((SelectStatement.Length > 0) && (SelectStatement.Substring(SelectStatement.Length - 1, 1) == ","))
                    SelectStatement = SelectStatement.Substring(0, SelectStatement.Length - 1) + " ";


                //No column left in the query?
                if (SelectStatement.Trim().Equals("SELECT", StringComparison.OrdinalIgnoreCase))
                {
                    SelectStatement = "SELECT 'null' ";
                    IsZeroSelectColumn = true;
                }

                ParsedQuery = SelectStatement + OtherStatement;
                //ParsedQuery = ParsedQuery.Replace("SELECT ,", "SELECT");
            }
            return ParsedQuery;
        }

        #region Helpers
        private int GetPosition()
        {
            string colName = Column.Name;
            int pos = -1;

            //if token found, this is default choice
            if (!string.IsNullOrEmpty(Column.Token))
            {
                pos = SelectStatement.IndexOf(Column.Token);
            }
            //try 1
            else if (!string.IsNullOrEmpty(Column.Table.Alias))
            {
                colName = string.Format("{0}.{1}", Column.Table.Alias, Column.Name);
                pos = SelectStatement.IndexOf(colName);
            }

            //try 2
            if ((pos == -1) && (!string.IsNullOrEmpty(Column.Table.Name)))
            {
                colName = string.Format("{0}.{1}", Column.Table.Name, Column.Name);
                pos = SelectStatement.IndexOf(colName);
            }

            //try 3
            if (pos == -1)
            {
                pos = SelectStatement.IndexOf(Column.Name);
            }

            return pos;
        }
        #endregion Helpers
    }
}
