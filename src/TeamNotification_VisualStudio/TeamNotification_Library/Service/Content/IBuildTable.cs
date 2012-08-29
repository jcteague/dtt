using System;
using System.Windows.Documents;

namespace TeamNotification_Library.Service.Content
{
    public interface IBuildTable
    {
        TableRowGroup GetContentFor<T, R, S>(Tuple<T, R, S> columns)
            where T : Block
            where R : Block
            where S : Block;
    }
}