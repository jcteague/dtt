define 'messages_view', ['general_view','socket.io'], (GeneralView,socketio,scrollspy) ->

    class MessagesView extends GeneralView

        id: 'messages-container'
        initialize: ->
            @model.on 'change:messages', @render, @
            
        render: ->
            @$el.empty()
            @$el.attr("class","well scroll-box span8")
            #@$el.attr("style","height:500px;")
            
            if @model.has('messages')
                @render_message(message) for message in @model.get('messages')
                if @socket?
                    @socket.removeAllListeners()
                    @socket.$events = {}
                @socket = new window.io.connect("http://localhost:3000#{@model.get('href')}")
                @socket.on 'message', @add_message
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
            @

        append_to: (parent) ->
            @$el.appendTo parent

        add_message: (message) =>
            m = JSON.parse message
            messages = @model.get('messages')
            messages.push {data:[{name:"body", value: JSON.parse(m.body).message}, {name:"user", value:m.name}, {name:"datetime", value:m.date}] }
            @model.set({messages: messages}, {silent: true})
            @append_message(m)
            @$el.scrollTop(@$el.prop('scrollHeight'))

        append_message: (message) ->
            name = message.name
            date = message.date
            body = JSON.parse(message.body).message 
            @$el.append("<p><b>#{name}(#{date}):</b>#{body}</p>")
