define 'query_renderer', ['jquery', 'jquery.autocomplete'], ($, jquery_autocomplete) ->

    class QueryRenderer

        render: (collection) ->
            query_template = $('<div>', {action:collection.uri})
            queries = collection.queries
            for query in queries
                field_generator = @generator_selector(query.rel)
                field_elements = field_generator(query.data)
                query_template.append(f) for f in field_elements

            query_template.append($('<input>', {"type":"submit"}))
            query_template

        autocomplete: (template) ->
            [$('<label>', {"for":template.name}).text(template.name), $('<input>',{"type":"text","name":template.name})]

        generator_selector: (field) =>
            @autocomplete if field is 'search'

