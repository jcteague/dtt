define 'query_renderer', ['jquery', 'jquery.autocomplete', 'underscore', 'config'], ($, jquery_autocomplete, underscore, config) ->

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
            input = $('<input>',{"type":"text","name":template.data[0].name,"class":"search-query", 'placeholder':template.prompt})
            submit = $('<input>', {"type":"submit", "class": "btn btn-primary"})

#            label = $('<label>', {"for":template.data[0].name})
#            label.text(template.prompt)

            build_item = (item) ->
                data = item.data
                id = _.find data, (obj) ->
                    obj.name is 'id'
                name = _.find data, (obj) ->
                    obj.name is 'email'

                """<span class="name">#{name.value}</span><span class="hidden">#{id.value}</span>"""

            processor = (objs) ->
                return [] unless objs? and objs.length != 0
                (build_item item for item in objs[entity])

            on_select = (selected) ->
                element = $(selected.value)
                selection = element.filter('.name').text()
                input.val(selection)

            # To prevent duplicate results
            $('.acResults').remove()
            # Move this concern to happen only once globally
            if $.cookie('authtoken')?
                $.ajaxSetup
                    beforeSend: (jqXHR) ->
                        authToken = $.cookie('authtoken')
                        jqXHR.setRequestHeader('Authorization', authToken )
                        jqXHR.withCredentials = true

            input.autocomplete("#{config.api.url}#{template.href}", {
                remoteDataType: 'json'
                processData: processor
                onItemSelect: on_select
                mustMatch: false
                selectFirst: false
                autoFill: true
                minChars: 1
            })
            query_class = "form-horizontal"
            if typeof template.query_class != 'undefined'
                query_class = template.query_class
                
            $('<form>', {action: template.submit, "class":query_class}).append(input, submit)

        generator_selector: (field) =>
            @autocomplete if field is 'autocomplete'


