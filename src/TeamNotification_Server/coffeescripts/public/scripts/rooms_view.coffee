define 'rooms_view', ['general_view'], (GeneralView) ->

    class RoomsView extends GeneralView

        id: 'rooms-container'

        initialize: ->
            @model.on 'change:rooms', @render, @

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

            link = room.links[0]
            room_key = get_field(room.data, 'room_key')
            if( room_key == '')
                @$el.append("""<p><a href="##{link.href}">#{link.name}</a></p>""")
            else
                @$el.append("""<p><a href="##{link.href}">#{link.name}</a><small> Room key: #{room_key}</small></p>""")
                

        append_to: (parent) ->
            @$el.appendTo parent
