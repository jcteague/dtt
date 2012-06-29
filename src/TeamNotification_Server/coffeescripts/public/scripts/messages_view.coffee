define 'messages_view', ['backbone'], (Backbone) ->

    class MessagesView extends Backbone.View

        id: 'messages-container'

        initialize: ->
            
        render: ->
            get_field = (field_name, data) ->
                for field in data
                    if field.name is field_name
                        return field.value
            @$el.empty()
            if @model?
                for message in @model
                    name = get_field 'user', message.data
                    body = get_field 'body', message.data
                    date = get_field 'datetime', message.data
                    @$el.append("<p><b>#{name}(#{date}):</b>#{body}</p>")
            @

        update: (messages) ->
            @model = messages

        append_to: (parent) ->
            @$el.appendTo parent

