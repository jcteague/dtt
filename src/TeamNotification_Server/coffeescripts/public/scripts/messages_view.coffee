define 'messages_view', ['general_view','socket.io'], (GeneralView,socketio,scrollspy) ->
    class MessagesView extends GeneralView

        id: 'messages-container'
        initialize: ->
            #@model.on 'change:messages', @render, @
            
        render: ->
            me = @
            @$el.empty()
            @$el.attr("class","well scroll-box span8")
            #@$el.attr("style","height:500px;")
            render_model = () ->
                newDate = new Date()
                me.$el.empty()
                for message in me.model.attributes.messages
                    me.$el.append me.render_message message, newDate
                me.$el.scrollTop(me.$el.prop('scrollHeight'))
                
            if @model.has('messages')
                setInterval((() -> render_model() ), 10000)
                if @socket?
                    @socket.removeAllListeners()
                    @socket.$events = {}
                @socket = new window.io.connect("#{@model.get('href')}")
                @socket.on 'message', @add_message
                render_model()
            @

        parse_date = (message_date, curr_date) ->
            message_time = message_date.getTime()/1000
            curr_time = Math.floor( curr_date.getTime() /1000)
            delta_time = curr_time - message_time
            if delta_time < 60
                return "just now"
            if delta_time < 120
                return "a minute ago"
            if delta_time < 3600
                return "#{Math.floor(delta_time/60)} minutes ago"
            if delta_time < 86400
                return "#{Math.floor(delta_time/3600)} hours ago"
            day = message_date.getDate()
            month = message_date.getMonth() + 1
            year = message_date.getFullYear()
            return "#{day}/#{month}/#{year}"

        render_message: (message,curr_date) ->
            get_field = (field_name, data) ->
                for field in data
                    if field.name is field_name
                        return field.value
            name = get_field 'user', message.data
            body = get_field 'body', message.data
            date = get_field 'datetime', message.data
            ("<p><b>#{name}<span class='chat_message_date'>(#{parse_date(new Date(date),curr_date) })</span>:</b> #{body}</p>")
        append_to: (parent) ->
            @$el.appendTo parent

        add_message: (message) =>
            m = JSON.parse message
            messages = @model.get('messages')
            messages.push {data:[{name:"body", value: JSON.parse(m.body).message}, {name:"user", value:m.name}, {name:"datetime", value:m.date}] }
            @model.set({messages: messages}, {silent: true})
            #@render_model()
            @append_message(m)

        append_message: (message) ->
            name = message.name
            date = parse_date  new Date(message.date), new Date()
            body = JSON.parse(message.body).message 
            @$el.append("<p><b>#{name}(#{date}):</b> #{body}</p>")
