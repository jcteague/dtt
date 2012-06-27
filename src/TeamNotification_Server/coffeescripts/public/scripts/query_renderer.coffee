define 'query_renderer', ['jquery', 'jquery.autocomplete'], ($, jquery_autocomplete) ->

    class QueryRenderer

        render: (queries) ->
            query_template = $('<div>')
            for query in queries
                field_generator = @generator_selector(query.rel)
                field_elements = field_generator(query)
                query_template.append(f) for f in field_elements

            query_template.append($('<input>', {"type":"submit"}))
            query_template

        autocomplete: (template) ->
            input = $('<input>',{"type":"text","name":template.data.name})

            build_item = (item) ->
                                

            processor = (objs) ->
                #(user.data.name for user in objs.users)
                (link.name for link in objs.links)

            on_select = () ->
                console.log arguments

            input.autocomplete("http://localhost:3000#{template.href}", {remoteDataType: 'json', processData: processor, onItemSelect: on_select})
            [$('<label>', {"for":template.data.name}).text(template.data.name), input]

        generator_selector: (field) =>
            @autocomplete if field is 'search'


