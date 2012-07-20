define 'query_renderer', ['jquery', 'jquery.autocomplete', 'underscore'], ($, jquery_autocomplete, underscore) ->

    class QueryRenderer

        render: (queries) ->
            query_template = $('<div>')
            for query in queries
                field_generator = @generator_selector(query.type)
                field_elements = field_generator(query)
                query_template.append(f) for f in field_elements

            query_template

        autocomplete: (template) ->
            entity = template.rel
            input = $('<input>',{"type":"text","name":template.data[0].name,"class":"search-query"})
            hidden_input = $('<input>',{"type":"hidden","name": "id"})
            submit = $('<input>', {"type":"submit", "class": "btn btn-primary"})

            label = $('<label>', {"for":template.data[0].name})
            label.text(template.prompt)

            build_item = (item) ->
                data = item.data
                id = _.find data, (obj) ->
                    obj.name is 'id'
                name = _.find data, (obj) ->
                    obj.name is 'name'

                """<span class="name">#{name.value}</span><span class="hidden">#{id.value}</span>"""

            processor = (objs) ->
                return [] unless objs? and objs.length != 0
                (build_item item for item in objs[entity])

            on_select = (selected) ->
                element = $(selected.value)
                input.val(element.filter('.name').text())
                hidden_input.val(element.filter('.hidden').text())

            # To prevent duplicate results
            $('.acResults').remove()
            input.autocomplete("http://localhost:3000#{template.href}", {
                remoteDataType: 'json'
                processData: processor
                onItemSelect: on_select
                mustMatch: true
                selectFirst: true
                autoFill: true
                minChars: 1
                onNoMatch: ->
                    hidden_input.val ''
                onFinish: ->
                    $('input[name=id]').val($('.acSelect .hidden').text())

            })
            query_class = "well form-horizontal"
            if typeof template.query_class != 'undefined'
                query_class = template.query_class
                
            $('<form>', {action: template.submit, "class":query_class}).append(label, input, hidden_input, submit)

        generator_selector: (field) =>
            @autocomplete if field is 'autocomplete'


