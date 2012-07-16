define 'rooms_view', ['backbone'], (Backbone) ->

    class RoomsView extends Backbone.View

        id: 'rooms-container'

        initialize: ->
            @model.on 'change:rooms', @render, @

        render: ->
            @$el.empty()
            if @model.has('rooms') and @model.get('rooms').length > 0
                @$el.append('<h1>User Rooms</h1>')
                @render_room(room) for room in @model.get('rooms')
            @

        render_room: (room) ->
            link = room.links[0]
            @$el.append("""<p><a href="##{link.href}">#{link.name}</a></p>""")

        append_to: (parent) ->
            @$el.appendTo parent

        listen_to: (parent_view) ->

