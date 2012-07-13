define 'messages_view', ['backbone'], (Backbone) ->

    class MessagesView extends Backbone.View

        id: 'messages-container'

        initialize: ->
            
        render: ->
            @$el.empty()
            if @model?
                @render_message(message) for message in @model.messages
            @

        render_message: (message) ->
            get_field = (field_name, data) ->
                for field in data
                    if field.name is field_name
                        return field.value

            name = get_field 'user', message.data
            body = get_field 'body', message.data
            date = get_field 'datetime', message.data
            @$el.append("<p><b>#{name}(#{date}):</b>#{body}</p>")

        append_to: (parent) ->
            @$el.appendTo parent

        listen_to: (parent_view) ->
            parent_view.on 'response:received', @update_messages, @

        update_messages: (unformatted_new_message) ->
            @add_message @model.messages, unformatted_new_message
        
        add_message: (messages, response) =>
            if response.success
                new_message = response.newMessage
                messages.push {data:[{ 'name':'user', 'value': new_message.user.first_name},{ 'name':'body', 'value': JSON.parse(new_message.body).message},{ 'name':'datetime', 'value':new_message.date }]}
