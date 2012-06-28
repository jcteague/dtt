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
            input = $('<input>',{"type":"text","name":template.data[0].name})
            hidden_input = $('<input>',{"type":"hidden","name": "id"})
            submit = $('<input>', {"type":"submit", "class": "submit"})
            submit.click (e) ->
                if $('.acInput').val() is ''
                    hidden_input.val ''
                    return

                selected = $('.acSelect .hidden')
                return if (selected.length is 0)
                value = selected.text()
                hidden_input.val value

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

            input.autocomplete("http://localhost:3000#{template.href}", {
                remoteDataType: 'json'
                processData: processor
                onItemSelect: on_select
                mustMatch: true
                selectFirst: true
                autoFill: true
                minChars: 1
            })
            $('<form>', {action: template.submit}).append(label, input, hidden_input, submit)

        generator_selector: (field) =>
            @autocomplete if field is 'autocomplete'


