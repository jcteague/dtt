define 'query_renderer', ['jquery', 'jquery.autocomplete'], ($, jquery_autocomplete) ->

    class QueryRenderer

        render: (collection) ->
            query_template = $('<div>', {action:collection.uri})
            queries = collection.queries
            for query in queries
                field_generator = @generator_selector(query.rel)
                field_elements = field_generator(query)
                query_template.append(f) for f in field_elements

            query_template.append($('<input>', {"type":"submit"}))
            query_template

        autocomplete: (template) ->
            input = $('<input>',{"type":"text","name":template.data.name})
            input.autocomplete("http://localhost:3000#{template.href}", remoteDataType: true)
            [$('<label>', {"for":template.data.name}).text(template.data.name), input]

        generator_selector: (field) =>
            @autocomplete if field is 'search'

