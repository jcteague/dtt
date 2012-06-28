define 'messages_view', ['backbone'], (Backbone) ->

    class MessagesView extends Backbone.View

        id: 'messages-container'

<<<<<<< HEAD
        render: ->
            @$el.empty()
            @$el.append "<p>#{message}</p>" for message in @model
=======
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
                    @$el.append("<b>#{name}(#{date}):</b>")
                    @$el.append(body)
                    @$el.append("<br/>")
            console.log 'dsfgiohnsdfgin'
>>>>>>> Added the messages view
            @

        update: (messages) ->
            @model = messages

        append_to: (parent) ->
            @$el.appendTo parent
<<<<<<< HEAD
=======

>>>>>>> Added the messages view
