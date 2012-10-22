define 'rooms_view', ['general_view','config'], (GeneralView, config) ->

    class RoomsView extends GeneralView

        id: 'rooms-container'

        initialize: ->
            @model.on 'change:rooms', @render, @
            authToken = $.cookie("authtoken")
            $.ajaxSetup
                beforeSend: (jqXHR) ->
                    jqXHR.setRequestHeader('Authorization', authToken )
                    jqXHR.withCredentials = true

        render: ->
            @$el.empty()
            @$el.attr('class','hero-unit')
            if @model.has('rooms') and @model.get('rooms').length > 0
                @rooms = @model.get('rooms')
                @$el.append('<h2>User Rooms</h2>')
                @render_room(room) for room in @rooms
            @

        render_room: (room) ->
            get_field = (data, name) ->
                for field in data
                    if(field.name == name)
                        return field.value
                return ""
            generate_unsubscribe_link = (room_id, container) ->
                anchor = $('<a>Unsubscribe</a>')
                anchor.bind 'click',() ->
                    $.post "#{config.api.url}/room/#{room_id}/unsubscribe", (data) ->
                        alert data 
                        container.hide()
                anchor
            link = room.links[0]
            room_key = get_field(room.data, 'room_key')
            if( room_key == '')
                p = $("<p/>")
                p.append """<a href="##{link.href}">#{link.name}</a>"""
                p.append " - "
                unsubscribe_room_link = generate_unsubscribe_link get_field(room.data,'id'), p
                p.append unsubscribe_room_link
                #@$el.append("""<p><a href="##{link.href}">#{link.name}</a> - #{unsubscribe_room_link} </p>""")
                @$el.append p
            else
                @$el.append("""<p><a href="##{link.href}">#{link.name}</a><small> Room key: #{room_key}</small></p>""")
                

        append_to: (parent) ->
            @$el.appendTo parent
