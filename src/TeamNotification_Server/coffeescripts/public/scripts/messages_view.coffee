define 'messages_view', ['general_view', 'underscore', 'prettify-languages', 'moment', 'config', 'audio'], (GeneralView, underscore, Prettify, Moment, config, Audio) ->

    class MessagesView extends GeneralView
        id: 'messages-container'
        added_code: false
        sound_file_mp3: config.site.url + '/sounds/notification.mp3'
        sound_file_wav: config.site.url + '/sounds/notification.wav'
        initialize: ->
            @authToken = $.cookie 'authtoken'
            @messages_table = $("<table/>")
            doFocus = true

            focus_func = () =>
                if(doFocus)
                    doFocus = false
                    last_timestamp =  @get_field('stamp', _.last(@model.get('messages')).data)
                    @get_messages_since last_timestamp
            $(window).bind "focus", focus_func
            $(window).bind "blur", () => 
                doFocus = true

        get_messages_since: (last_timestamp) ->
            path = "#{config.api.url}#{@model.get('href')}/since/#{last_timestamp}"
            @get_cross_domain_json path, (data) => 
                @add_message(@serialize_message(message)) for message in data.messages.slice(1)

        parsing_links: (message) ->
            message_words =  message.split ' '
            final_message = ''
            for word in message_words
                w = word
                if word.indexOf("://") != -1
                    w = "<a href='#{w}' target='_blank'>#{w}</a>"
                else
                    w = w.replace('&','&amp;').replace('<','&lt;').replace('>','&gt;').replace('"','&quot;').replace("'",'&#x27;').replace('/','&#x2F;')
                final_message = final_message + w + ' '
            final_message

        get_cross_domain_json: (url, callback) ->
            me = this
            parameters = {
                type: 'GET'
                accept : "application/json"
                contentType: 'application/json'
                dataType: 'json'
                url: url
                cache: false
                headers:
                    authorization: $.cookie 'authtoken'
                success: callback
                error: (jqXHR, textStatus)->
                    params = this
                    u = this.url
                    head = this.headers
                    $.ajax 
                        type:'GET' 
                        url:u
                        success: () ->
                            me.get_cross_domain_json(u,callback)
                            
                    return
            }
            $.ajax parameters

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
                messages = @model.get('messages')
                $('.chat_message_date').each (idx, element) =>
                    message_date = new Date(@flatten_message(messages[idx]).stamp)
                    element.innerHTML = (parse_date(message_date, newDate))
            as = ''
                
            append_message= (message)->
                m = JSON.parse message
                if !me.window_focus && m.user_id != me.model.get('user').user_id
                    if as == ''
                        as = audiojs.createAll()
                    as[0].load
                        mp3: @sound_file_mp3
                    as[0].play()
                me.add_message m
            render_model = () ->
                newDate = new Date()
                me.$el.empty()
                for message in me.model.attributes.messages
                    me.messages_table.append me.render_message message, newDate
                me.$el.append me.messages_table
                me.$el.scrollTop(me.$el.prop('scrollHeight'))

            if @model.has('messages')
                url = "#{config.api.url}#{@model.get('href')}"
                socket = new window.io.connect(url)
                socket.on 'message', append_message # @add_message
                render_model()
                
            sound_div = $("""<div style='display:block'><audio preload="auto" src="./sounds/notification.mp3"><source src="#{@sound_file_mp3}"></audio></div>""")
            me.$el.append sound_div
            
            $(window).focus () ->
                me.window_focus = true
                console.log me.window_focus
            $(window).blur ()->
                window_focus = false
                console.log me.window_focus
            @
                
        parse_date = (message_date, curr_date) ->
            if is_today(message_date, curr_date)
                return moment(message_date).format('h:mm A')

            day = message_date.getDate()
            month = message_date.getMonth() + 1
            year = message_date.getFullYear()
            "#{month}/#{day}/#{year}"

        is_today = (message_date, current_date) ->
            build_string = (date) -> "#{date.getUTCFullYear()}-#{date.getUTCMonth()}-#{date.getUTCDate()}"
            build_string(message_date) == build_string(current_date)


        render_message: (message) ->
            flattened_message = @flatten_message(message)
            message_row = @read_message_data(flattened_message)
            r = $(message_row)
            r.removeClass('new_message')
            r.children().removeClass('new_message')
            r

        get_field: (field_name, data) ->
            for field in data
                if field.name is field_name
                    return field.value

        append_to: (parent) ->
            @$el.appendTo parent

        add_message: (m) =>
            if $("##{m.stamp}").length  == 1
                @edit_message $("#message-#{m.stamp}"), m
            else
                messages = @model.get('messages')
                messages.push {data:[{name:"body", value: m.body}, {name:"user", value:m.name}, {name:"datetime", value:m.date},{name:"stamp", value:m.stamp}] }
                @model.set({messages: messages}, {silent: true})

                @messages_table.append @read_message_data(m)
                @$el.append @messages_table
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
            escaped_message = @parsing_links parsedBody.message
            p[0].innerHTML = escaped_message

        read_message_data: (message) ->
            name = message.name
            new_date = new Date()
            date = parse_date  new Date(message.stamp), new_date
            parsedBody = JSON.parse(message.body)
            $name_span = $("""<span><b>#{name}:</b></span>""")

            if !(parsedBody.notification?)
                if @last_user_id_that_posted? and @last_user_id_that_posted is message.user_id 
                    $name_span.children().hide()
            
            @last_user_id_that_posted = message.user_id
            
            escaped_message = @parsing_links parsedBody.message # $('<div/>').text(parsedBody.message).html()
            build_new_row = (name, message, time) ->
                return "<tr class='new_message'><td nowrap='nowrap'>#{name}</td><td style='width:100%'>#{message}</td><td nowrap='nowrap' width='60'><b><span class='chat_message_date'>#{time}</span></b></td></tr>"
            
            if(typeof parsedBody.solution != 'undefined' && parsedBody.solution!='')
                @added_code = true
                return build_new_row( $name_span.html(), "<pre class='prettyprint linenums'>#{escaped_message}</pre>", date)
            if parsedBody.notification?
                @last_user_id_that_posted = -1 
                add_links = (str) ->
                    str.replace(/\{0\}/, "<a target='_blank' href=\"#{parsedBody.repository_url}\">#{parsedBody.repository_name}</a>").replace(/\{1\}/, "<a target='_blank' href=\"#{parsedBody.url}\">Reference</a>")
                return build_new_row($name_span.html(), add_links(parsedBody.message), date)
            return build_new_row($name_span.html(), escaped_message, date)
