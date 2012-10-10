define 'messages_view', ['general_view', 'underscore', 'prettify-languages'], (GeneralView, underscore, Prettify) ->

    class MessagesView extends GeneralView

        id: 'messages-container'
        added_code: false

        initialize: ->
            $(window).focus (e) =>
                last_timestamp =  @get_field('stamp', _.last(@model.get('messages')).data)
                @get_messages_since last_timestamp

        get_messages_since: (last_timestamp) ->
            path = "#{@model.get('href')}/since/#{last_timestamp}"
            $.getJSON path, (data) =>
                @add_message(@serialize_message(message)) for message in data.messages.slice(1)

        serialize_message: (message) ->
            JSON.stringify(@flatten_message(message))

        flatten_message: (message) ->
            user_id = @get_field 'user_id', message.data
            name = @get_field 'user', message.data
            body = @get_field 'body', message.data
            date = @get_field 'datetime', message.data
            stamp = @get_field 'stamp', message.data
            return {body: body, user_id: user_id, name: name, date: date, stamp: stamp}

        render: ->
            me = @
            @$el.empty()
            @$el.attr("class","well scroll-box span8")
            update_dates = () =>
                newDate = new Date()
                console.log 'during update', @get_field 'datetime', @model.get('messages')[0].data
                messages = @model.get('messages')
                $('.chat_message_date').each (idx, element) =>
                    message_date = @flatten_message(messages[idx]).date
                    console.log message_date.getTime(), newDate.getTime()
                    element.innerHTML = (parse_date(new Date(message_date), newDate))

            render_model = () ->
                newDate = new Date()
                me.$el.empty()
                for message in me.model.attributes.messages
                    me.$el.append me.render_message message, newDate
                me.$el.scrollTop(me.$el.prop('scrollHeight'))

            console.log 'in the beginning', @get_field 'datetime', @model.attributes.messages[0].data
            if @model.has('messages')
                setInterval((() -> update_dates() ), 10000)
                socket = new window.io.connect(@model.get('href'))
                socket.on 'message', @add_message
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

        render_message: (message) ->
            flattened_message = @flatten_message(message)
            message_paragraph = @read_message_data(flattened_message)
            r = $(message_paragraph)
            r.removeClass('new_message')
            r.children().removeClass('new_message')
            r

        get_field: (field_name, data) ->
            for field in data
                if field.name is field_name
                    return field.value

        append_to: (parent) ->
            @$el.appendTo parent

        add_message: (message) =>
            m = JSON.parse message

            if $("##{m.stamp}").length  == 1
                @edit_message $("#message-#{m.stamp}"), m
            else
                messages = @model.get('messages')
                messages.push {data:[{name:"body", value: m.body}, {name:"user", value:m.name}, {name:"datetime", value:m.date},{name:"stamp", value:m.stamp}] }
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

        edit_message: (p, message) ->
            parsedBody = JSON.parse(message.body)
            p.attr "class", "new_message"
            p[0].innerHTML = parsedBody.message

        read_message_data: (message) ->
            name = message.name
            new_date = new Date()
            date = parse_date  new Date(message.date), new_date
            console.log 'on read_message_data', new Date(message.date).getTime(), new_date.getTime()
            parsedBody = JSON.parse(message.body)
            $name_and_date = $("""<span><b>#{name}(<span class='chat_message_date'>#{date}</span>):</b></span>""")

            if @last_user_id_that_posted? and @last_user_id_that_posted is message.user_id
                $name_and_date.children().hide()

            @last_user_id_that_posted = message.user_id
            if(typeof parsedBody.solution != 'undefined' && parsedBody.solution!='')
                @added_code = true
                p = document.createElement("p")
                $(p).attr 'id',"#{message.stamp}"
                p.innerHTML = "#{$name_and_date.html()} <pre class='new_message prettyprint linenums'>#{parsedBody.message}</pre>"
                return p
            if parsedBody.notification?
                add_links = (str) ->
                    str.replace(/\{0\}/, "<a target='_blank' href=\"#{parsedBody.repository_url}\">#{parsedBody.repository_name}</a>").replace(/\{1\}/, "<a target='_blank' href=\"#{parsedBody.url}\">Reference</a>")
                return add_links("<p id='#{message.stamp}' class='new_message'><span id='message-#{message.stamp}'>#{parsedBody.message}</span></p>")

            return ("<p id='#{message.stamp}' class='new_message'>#{$name_and_date.html()} <span id='message-#{message.stamp}'>#{parsedBody.message}</span></p>")
