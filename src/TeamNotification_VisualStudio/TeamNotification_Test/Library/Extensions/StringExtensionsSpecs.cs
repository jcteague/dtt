using Machine.Specifications;
using TeamNotification_Library.Extensions;
using developwithpassion.specifications.rhinomocks;

namespace TeamNotification_Test.Library.Extensions
{
    [Subject(typeof(StringExtensions))]
    public class StringExtensionsSpecs
    {
        public class Concern : Observes
        {
            
        }

        public class when_formatting_a_string : Concern
        {
            Establish context = () => str = "Hello {0}, Goodbye {1}";

            Because of = () =>
                result = str.FormatUsing("foo", "bar");

            It should_return_the_string_with_the_placeholders_subtituted = () =>
                result.ShouldEqual("Hello foo, Goodbye bar");

            private static string str;
            private static string result;
        }
    }
}