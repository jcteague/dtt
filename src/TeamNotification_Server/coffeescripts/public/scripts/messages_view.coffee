define 'messages_view', ['general_view'], (GeneralView) ->
    class MessagesView extends GeneralView

        id: 'messages-container'
        initialize: ->
            #@model.on 'change:messages', @render, @
            
        render: ->
            me = @
            @$el.empty()
            @$el.attr("class","well scroll-box span8")
            render_model = () ->
                newDate = new Date()
                me.$el.empty()
                for message in me.model.attributes.messages
                    me.$el.append me.render_message message, newDate
                me.$el.scrollTop(me.$el.prop('scrollHeight'))
                
            if @model.has('messages')
                #setInterval((() -> render_model() ), 10000)
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
            return "#{month}/#{day}/#{year}"

        render_message: (message,curr_date) ->
            get_field = (field_name, data) ->
                for field in data
                    if field.name is field_name
                        return field.value
            name = get_field 'user', message.data
            body = get_field 'body', message.data
            date = get_field 'datetime', message.data
            parsedBody = JSON.parse(body)
            return  read_message_data(name:name, date:date, body:parsedBody})
            
        append_to: (parent) ->
            @$el.appendTo parent

        add_message: (message) =>
            m = JSON.parse message
            messages = @model.get('messages')
            messages.push {data:[{name:"body", value: JSON.parse(m.body).message}, {name:"user", value:m.name}, {name:"datetime", value:m.date}] }
            @model.set({messages: messages}, {silent: true})
            @$el.append read_message_data(m)
            
        read_message_data: (message) ->
            name = message.name
            date = parse_date  new Date(message.date), new Date())
            parsedBody = JSON.parse(message.body)
            if(typeof parsedBody.solution != 'undefined' && parsedBody.solution!='')
                return ("<p><b>#{name}<span class='chat_message_date'>(#{date})</span>:</b> <pre class='prettyprint lang-js'>#{parsedBody.message}</pre></p>")
            else
                return ("<p><b>#{name}<span class='chat_message_date'>(#{date})</span>:</b> #{parsedBody.message.replace(/\n/g,'<br/>')}</p>")
            
        
            
