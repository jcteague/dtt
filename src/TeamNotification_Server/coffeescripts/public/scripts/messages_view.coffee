define 'messages_view', ['general_view','prettify'], (GeneralView,Prettify) ->
    class MessagesView extends GeneralView

        id: 'messages-container'
        initialize: -> 
        added_code: false
        render: ->
            me = @
            @$el.empty()
            @$el.attr("class","well scroll-box span8")
            update_dates = () -> 
            
                get_field = (field_name, data) ->
                    for field in data
                        if field.name is field_name
                            return field.value
                newDate = new Date()
                $('.chat_message_date').each (idx, element) ->
                    message_date = get_field 'datetime', me.model.attributes.messages[idx].data
                    element.innerHTML = (parse_date new Date(message_date), newDate)
                
            render_model = () ->
                newDate = new Date()
                me.$el.empty()
                for message in me.model.attributes.messages
                    me.$el.append me.render_message message, newDate
                me.$el.scrollTop(me.$el.prop('scrollHeight'))
                
            if @model.has('messages')
                setInterval((() -> update_dates() ), 10000)
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

            user_id = get_field 'user_id', message.data
            name = get_field 'user', message.data
            body = get_field 'body', message.data
            date = get_field 'datetime', message.data
            r = $(@read_message_data({user_id: user_id, name:name, date:date, body:body}))
            r.removeClass('new_message')
            r
            
        append_to: (parent) ->
            @$el.appendTo parent

        add_message: (message) =>
            fade_in_message = () ->
                if @added_code is true
                    @added_code = false
                    prettyPrint()
                    @$('.prettyprint').removeClass('prettyprint')
            m = JSON.parse message
            messages = @model.get('messages')
            messages.push {data:[{name:"body", value: m.body}, {name:"user", value:m.name}, {name:"datetime", value:m.date}] }
            @model.set({messages: messages}, {silent: true})
            @$el.append @read_message_data(m)
            @$el.scrollTop(@$el.prop('scrollHeight'))
            me = @
            $('.new_message').animate {backgroundColor: '#F07746'}, 300, () ->
                $('.new_message').removeClass('new_message')
                if me.added_code is true
                    me.added_code = false
                    prettyPrint()
                    me.$('.prettyprint').removeClass('prettyprint')

        read_message_data: (message) ->
            name = message.name
            date = parse_date  new Date(message.date), new Date()
            parsedBody = JSON.parse(message.body)
            $name_and_date = $("""<span><b>#{name}(<span class='chat_message_date'>#{date}</span>):</b></span>""")

            if @last_user_id_that_posted? and @last_user_id_that_posted is message.user_id
                $name_and_date.children().hide() 

            @last_user_id_that_posted = message.user_id
            if(typeof parsedBody.solution != 'undefined' && parsedBody.solution!='')
                @added_code = true
                return ("<p>#{$name_and_date.html()} <pre class='new_message prettyprint linenums'>#{parsedBody.message}</pre></p>")
            else
                return ("<p class='new_message'>#{$name_and_date.html()} #{parsedBody.message}</p>")
