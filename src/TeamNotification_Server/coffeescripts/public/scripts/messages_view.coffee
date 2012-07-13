define 'messages_view', ['backbone','socket.io'], (Backbone,socketio) ->

    class MessagesView extends Backbone.View

        id: 'messages-container'
        
        initialize: ->
           # @socket = null
        render: ->
            @$el.empty()
            me = @$el
            get_field = (field_name, data) ->
                for field in data
                    if field.name is field_name
                        return field.value
            message_handler = (message) ->
                    m = JSON.parse message
                    name = m.name
                    date = m.date
                    body = JSON.parse(m.body).message 
                    me.append("<p><b>#{name}(#{date}):</b>#{body}</p>")
                    
            if @model? and @model.messages?
                for message in @model.messages
                    name = get_field 'user', message.data
                    body = get_field 'body', message.data
                    date = get_field 'datetime', message.data
                    @$el.append("<p><b>#{name}(#{date}):</b>#{body}</p>")
                
                if typeof @socket != 'undefined'
                    #@socket.removeListener 'message', message_handler
                    @socket.removeAllListeners # 'message', message_handler
                    @socket.$events = {}
                
                @socket = new window.io.connect("http://localhost:3000#{@model.href}")
               # @socket.on 'connect', () ->
               #     console.log("Client Connected")
                @socket.on 'message', message_handler
            @

        update: (messages) ->
            @model = messages

        append_to: (parent) ->
            @$el.appendTo parent
