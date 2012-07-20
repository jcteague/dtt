define 'rooms_view', ['general_view'], (GeneralView) ->

    class RoomsView extends GeneralView

        id: 'rooms-container'

        initialize: ->
            @model.on 'change:rooms', @render, @

        render: ->
            @$el.empty()
            @$el.attr('class','hero-unit')
            if @model.has('rooms') and @model.get('rooms').length > 0
                @$el.append('<h2>User Rooms</h2>')
                @render_room(room) for room in @model.get('rooms')
            @

        render_room: (room) ->
            link = room.links[0]
            @$el.append("""<p><a href="##{link.href}">#{link.name}</a></p>""")

        append_to: (parent) ->
            @$el.appendTo parent
