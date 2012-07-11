define 'messages_view', ['backbone','socket.io'], (Backbone,socketio) ->

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
                for message in @model.messages
                    name = get_field 'user', message.data
                    body = get_field 'body', message.data
                    date = get_field 'datetime', message.data
                    @$el.append("<p><b>#{name}(#{date}):</b>#{body}</p>")
                
                @socket = new window.io.connect("http://localhost:3000#{@model.href}")
                me = @$el
                @socket.on 'connect', () ->
                    console.log("Client Connected")

                @socket.on 'message', (message) ->
                    m = JSON.parse message
                    name = m.name
                    date = m.date
                    body = JSON.parse(m.body).message 
                    me.append("<p><b>#{name}(#{date}):</b>#{body}</p>")
            @

        update: (messages) ->
            @model = messages

        append_to: (parent) ->
            @$el.appendTo parent
