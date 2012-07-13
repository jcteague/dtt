define 'rooms_view', ['backbone'], (Backbone) ->

    class RoomsView extends Backbone.View

        id: 'rooms-container'

        render: ->
            @$el.empty()
            if @model?
                @$el.append('<h1>User Rooms</h1>')
                @render_room(room) for room in @model.rooms
            @

        render_room: (room) ->
            link = room.links[0]
            @$el.append("""<p><a href="##{link.href}">#{link.name}</a></p>""")

        append_to: (parent) ->
            @$el.appendTo parent

        listen_to: (parent_view) ->

