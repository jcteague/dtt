 using System;
 using System.Windows.Documents;
 using Machine.Specifications;
 using TeamNotification_Library.Service.Content;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
 using TeamNotification_Library.Extensions;

namespace TeamNotification_Test.Library.Service.Content
{  
    [Subject(typeof(TableBuilder))]  
    public class TableBuilderSpecs
    {
        public abstract class Concern : Observes<IBuildTable,
                                            TableBuilder>
        {
        
        }

   
        public class when_getting_the_content_for_the_columns : Concern
        {
            Establish context = () =>
            {
                cell0 = "blah";
                cell1 = "foo";
                cell2 = "bar";
                var paragraph0 = new Paragraph(new Run(cell0));
                var paragraph1 = new Paragraph(new Run(cell1));
                var paragraph2 = new Paragraph(new Run(cell2));
                columns = new Tuple<Paragraph, Paragraph, Paragraph>(paragraph0, paragraph1, paragraph2);
            };

            Because of = () =>
                result = sut.GetContentFor(columns);
        
            It should_return_a_table_row_filled_with_the_columns = () =>
            {
                var tableRow = result.Rows[0];
                GetText(tableRow.Cells[0]).ShouldEqual(cell0);
                GetText(tableRow.Cells[1]).ShouldEqual(cell1);
                GetText(tableRow.Cells[2]).ShouldEqual(cell2);
            };

            private static Tuple<Paragraph, Paragraph, Paragraph> columns;
            private static TableRowGroup result;
            private static string cell0;
            private static string cell1;
            private static string cell2;

            private static string GetText(TableCell cell)
            {
                return ((Paragraph) cell.Blocks.FirstBlock).GetText();
            }
        }
    }
}
