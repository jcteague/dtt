using System;
using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Content
{
    public class TableBuilder : IBuildTable
    {
        public TableRowGroup GetContentFor<T, R, S>(Tuple<T, R, S> columns) where T : Block where R : Block where S : Block
        {
            var tableRowGroup = new TableRowGroup();
            var row = GetRowFor(columns);
            tableRowGroup.Rows.Add(row);

            return tableRowGroup;
        }

        private TableRow GetRowFor<T, R, S>(Tuple<T, R, S> columns) where T : Block where R : Block where S : Block
        {
            var row = new TableRow();
            row.Cells.Add(new TableCell(columns.Item1));
            row.Cells.Add(new TableCell(columns.Item2));
            row.Cells.Add(new TableCell(columns.Item3));
            return row;
        }
    }
}