define 'messages_view', ['backbone','socket.io'], (Backbone,socketio) ->

    class MessagesView extends Backbone.View

        id: 'messages-container'

        initialize: ->
            
        render: ->
            @$el.empty()
            me = @$el

            message_handler = (message) ->
                m = JSON.parse message
                name = m.name
                date = m.date
                body = JSON.parse(m.body).message 
                me.append("<p><b>#{name}(#{date}):</b>#{body}</p>")

            if @model? and @model.messages?
                @render_message(message) for message in @model.messages
                if typeof @socket != 'undefined'
                    #@socket.removeListener 'message', message_handler
                    @socket.removeAllListeners # 'message', message_handler
                    @socket.$events = {}
                
                @socket = new window.io.connect("http://localhost:3000#{@model.href}")
                @socket.on 'message', message_handler
            @

        render_message: (message) ->
            get_field = (field_name, data) ->
                for field in data
                    if field.name is field_name
                        return field.value
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
        
        add_message: (messages, response) =>
            if response.success
                new_message = response.newMessage
                messages.push {data:[{ 'name':'user', 'value': new_message.user.name},{ 'name':'body', 'value': JSON.parse(new_message.body).message},{ 'name':'datetime', 'value':new_message.date }]}
